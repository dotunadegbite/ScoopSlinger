using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum EnemyType
{
    PAWN,
    AVERAGE,
    TANK
}

[RequireComponent(typeof(Health), typeof(Actor), typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("The Y height at which the enemy will be automatically killed (if it falls off of the level)")]
    public float SelfDestructYHeight = -20f;

    [Tooltip("The distance at which the enemy considers that it has reached its current path destination point")]
    public float PathReachingRadius = 2f;

    [Tooltip("The speed at which the enemy rotates")]
    public float OrientationSpeed = 10f;

    [Tooltip("Delay after death where the GameObject is destroyed (to allow for animation)")]
    public float DeathDuration = 0f;

    [Header("Sounds")] [Tooltip("Sound played when recieving damages")]
    public AudioClip DamageTick;

    [Header("VFX")] [Tooltip("The VFX prefab spawned when the enemy dies")]
    public GameObject DeathVfx;

    [Tooltip("The point at which the death VFX is spawned")]
    public Transform DeathVfxSpawnPoint;

    [Header("Debug Display")] [Tooltip("Color of the sphere gizmo representing the path reaching range")]
    public Color PathReachingRangeColor = Color.yellow;

    public UnityAction onAttack;
    public UnityAction onDetectedTarget;
    public UnityAction onLostTarget;
    public UnityAction onDamaged;

    float m_LastTimeDamaged = float.NegativeInfinity;

    public MonsterPath PatrolPath { get; set; }
    /* public GameObject KnownDetectedTarget => DetectionModule.KnownDetectedTarget;
    public bool IsTargetInAttackRange => DetectionModule.IsTargetInAttackRange;
    public bool IsSeeingTarget => DetectionModule.IsSeeingTarget;*/


    [SerializeField]public GameObject KnownDetectedTarget => ChaseTriggerModule.KnownDetectedTarget;
    public bool IsTargetInAttackRange => ChaseTriggerModule.IsTargetInAttackRange;
    public bool HasSeenPlayer => ChaseTriggerModule.HasSeenPlayer;

    public NavMeshAgent NavMeshAgent { get; private set; }
    

    [SerializeField] private ChaseTriggerZone _chaseTriggerModule;
    [SerializeField] private GameObject _humanPrefab;
    public ChaseTriggerZone ChaseTriggerModule
    {
        get =>  _chaseTriggerModule;
    }

    int m_PathDestinationNodeIndex;
    MonsterManager m_MonsterManager;
    ActorsManager m_ActorsManager;
    Health m_Health;
    Actor m_Actor;
    Collider[] m_SelfColliders;
    GameFlowManager m_GameFlowManager;
    MonsterHitBox m_MonsterHitBox;

    private Vector3 m_CurrentDestination;

    void Start()
    {
        m_MonsterManager = FindObjectOfType<MonsterManager>();
        DebugUtility.HandleErrorIfNullFindObject<MonsterManager, MonsterController>(m_MonsterManager, this);

        m_ActorsManager = FindObjectOfType<ActorsManager>();
        DebugUtility.HandleErrorIfNullFindObject<ActorsManager, MonsterController>(m_ActorsManager, this);

        m_MonsterManager.RegisterEnemy(this);

        m_Health = GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, MonsterController>(m_Health, this, gameObject);

        m_Actor = GetComponent<Actor>();
        DebugUtility.HandleErrorIfNullGetComponent<Actor, MonsterController>(m_Actor, this, gameObject);

        NavMeshAgent = GetComponent<NavMeshAgent>();
        m_SelfColliders = GetComponentsInChildren<Collider>();

        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, MonsterController>(m_GameFlowManager, this);

        m_MonsterHitBox = GetComponentInChildren<MonsterHitBox>();

        // Subscribe to damage & death actions
        m_Health.OnDie += OnDie;
        m_Health.OnDamaged += OnDamaged;

        ChaseTriggerModule.onDetectedTarget += OnDetectedTarget;
        onAttack += ChaseTriggerModule.OnAttack;
    }

    void Update()
    {
        EnsureIsWithinLevelBounds();
    }

    void EnsureIsWithinLevelBounds()
    {
        // at every frame, this tests for conditions to kill the enemy
        if (transform.position.y < SelfDestructYHeight)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnLostTarget()
    {
        onLostTarget.Invoke();
    }

    void OnDetectedTarget()
    {
        onDetectedTarget.Invoke();
    }

    public void OrientTowards(Vector3 lookPosition)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * OrientationSpeed);
        }
    }

    bool IsPathValid()
    {
        return PatrolPath && PatrolPath.PathNodes.Count > 0;
    }

    public void StopAgent()
    {
        SetNavDestination(transform.position, /* saveDestination */ false);
    }

    public void ResumeAgent()
    {
        SetNavDestination(m_CurrentDestination, /* saveDestination */ true);
    }

    public void ResetPathDestination()
    {
        m_PathDestinationNodeIndex = 0;
    }

    public void SetPathDestinationToClosestNode()
    {
        if (IsPathValid())
        {
            int closestPathNodeIndex = 0;
            for (int i = 0; i < PatrolPath.PathNodes.Count; i++)
            {
                float distanceToPathNode = PatrolPath.GetDistanceToNode(transform.position, i);
                if (distanceToPathNode < PatrolPath.GetDistanceToNode(transform.position, closestPathNodeIndex))
                {
                    closestPathNodeIndex = i;
                }
            }

            m_PathDestinationNodeIndex = closestPathNodeIndex;
        }
        else
        {
            m_PathDestinationNodeIndex = 0;
        }
    }

    public Vector3 GetDestinationOnPath()
    {
        if (IsPathValid())
        {
            return PatrolPath.GetPositionOfPathNode(m_PathDestinationNodeIndex);
        }
        else
        {
            return transform.position;
        }
    }

    public void SetNavDestination(Vector3 destination, bool saveDestination)
    {
        if (NavMeshAgent)
        {
            if (saveDestination)
            {
                m_CurrentDestination = destination;
            }

            NavMeshAgent.SetDestination(destination);
        }
    }

    public void UpdatePathDestination(bool inverseOrder = false)
    {
        if (IsPathValid())
        {
            // Check if reached the path destination
            if ((transform.position - GetDestinationOnPath()).magnitude <= PathReachingRadius)
            {
                // increment path destination index
                m_PathDestinationNodeIndex =
                    inverseOrder ? (m_PathDestinationNodeIndex - 1) : (m_PathDestinationNodeIndex + 1);
                if (m_PathDestinationNodeIndex < 0)
                {
                    m_PathDestinationNodeIndex += PatrolPath.PathNodes.Count;
                }

                if (m_PathDestinationNodeIndex >= PatrolPath.PathNodes.Count)
                {
                    m_PathDestinationNodeIndex -= PatrolPath.PathNodes.Count;
                }
            }
        }
    }

    void OnDamaged(float damage, GameObject damageSource)
    {
        // test if the damage source is the player
        if (damageSource && !damageSource.GetComponent<MonsterController>())
        {
            ChaseTriggerModule.OnDamaged(damageSource);
            
            onDamaged?.Invoke();
            m_LastTimeDamaged = Time.time;
        
            // play the damage tick sound
            /* if (DamageTick && !m_WasDamagedThisFrame)
                AudioUtility.CreateSFX(DamageTick, transform.position, AudioUtility.AudioGroups.DamageTick, 0f); */
        }
    }

    void OnDie()
    {
        // spawn a particle system when dying
        var vfx = Instantiate(DeathVfx, DeathVfxSpawnPoint.position, Quaternion.identity);
        Destroy(vfx, 5f);

        // tells the game flow manager to handle the enemy destuction
        m_MonsterManager.UnregisterEnemy(this);

        var human = Instantiate(_humanPrefab, DeathVfxSpawnPoint.position, transform.rotation);
        /* var humanController = human.GetComponentInChildren<HumanController>();
        humanController.TriggerPostTransformation(); */

        // this will call the OnDestroy function
        Destroy(gameObject, DeathDuration);
    }

    void OnDrawGizmosSelected()
    {
        // Path reaching range
        Gizmos.color = PathReachingRangeColor;
        Gizmos.DrawWireSphere(transform.position, PathReachingRadius);
    }

    public bool TryAttack(Vector3 enemyPosition)
    {
        if (m_GameFlowManager.GameIsEnding)
            return false;

        onAttack.Invoke();
        return true;
    }
}
