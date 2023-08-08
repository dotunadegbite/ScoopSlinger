using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SwarmAgent : MonoBehaviour
{
    public enum ENEMY_STATE
    {
        IDLE,
        PATROL,
        CHASING,
        RESTORED
    }

    NavMeshAgent _agent;
    [SerializeField]ENEMY_STATE _currentState = ENEMY_STATE.PATROL;
    Animator _animator;
    float _currentDistance;
    [SerializeField] Transform _currentPatrolPoint;
    [SerializeField] Transform _currentTarget;
    [SerializeField] int _patrolPointIndex;
    List<Transform> _patrolWaypoints;
    float _currentHealth;
    bool _isAlive;
    GameObject _currentModel;
    [SerializeField] float _maxDistanceFromOrigin = 30.0f;
    [SerializeField] float _minDistanceFromTarget = 3.0f;
    [SerializeField] float _minDistanceFromOrigin = 3.0f;
    [SerializeField] GameObject _patrolPointsParent;
    [SerializeField] Transform _originPoint;
    [SerializeField] float MaxHealth = 10f;
    [SerializeField] float Damage = 10f;
    
    [SerializeField] GameObject _monsterModel;
    [SerializeField] GameObject _humanModel;
    [SerializeField] GameObject _smokeEffect;

    [SerializeField] private float distanceToTarget;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = 0.0f;
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _patrolWaypoints = _patrolPointsParent.GetComponentsInChildren<Transform>().ToList();
        _currentHealth = MaxHealth;
        _isAlive = true;

        _monsterModel.SetActive(true);
        _currentModel = _monsterModel;
        _animator = _currentModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current Agent Speed: " + _agent.speed);
        float distanceFromOrigin, distanceFromTarget;

        distanceToTarget = _agent.remainingDistance;
        

        switch (_currentState)
        {
            case ENEMY_STATE.IDLE:
                distanceFromOrigin = DistanceBetweenObjects(_originPoint, this.transform);
                distanceFromTarget = DistanceBetweenObjects(_currentTarget, this.transform);
                distanceFromOrigin = DistanceBetweenObjects(_originPoint, this.transform);
                if (distanceFromOrigin >= _maxDistanceFromOrigin)
                {
                    ReturnToOrigin();
                }
                else if (distanceFromOrigin <= _minDistanceFromOrigin)
                {
                    BeginPatrol();
                }
                break;
            case ENEMY_STATE.CHASING:
                distanceFromOrigin = DistanceBetweenObjects(_originPoint, this.transform);
                distanceFromTarget = DistanceBetweenObjects(_currentTarget, this.transform);
                if (distanceFromOrigin >= _maxDistanceFromOrigin)
                {
                    ReturnToOrigin();
                }
                else if (distanceFromTarget <= _minDistanceFromTarget)
                {
                    AttackTarget();
                }
                else
                {
                    ChaseTarget();
                }
                break;
            case ENEMY_STATE.PATROL:
                PatrolArea();
                break;
            default:
                break;
        }
    }

    public void HandlePlayerEnterChaseZone(Transform target)
    {
        Debug.Log("Player enter Chase zone");
        var aimPointObject = GameObject.FindWithTag("AimPoint").GetComponent<Transform>();
        // 0. If not patrolling then continue
        if (_currentState != ENEMY_STATE.PATROL)
            return;
        // 2. Clear Patrol
        _patrolPointIndex = 0;
        _currentPatrolPoint = null;

        // 3. Update state to chase
        _currentState = ENEMY_STATE.CHASING;
        _currentTarget = aimPointObject;
        Debug.Log("Current target: " + _currentTarget.position.ToString());
    }

    public void DamagePlayer()
    {
        AttackTarget();
    }

    public void TakeDamage(float damage)
    {
        if (!_isAlive)
            return;
        
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _isAlive = false;
            StartCoroutine(TriggerDeath());
        }
    }

    void ChaseTarget()
    {
        _agent.speed = 10.0f;
        _animator.SetFloat("speed", _agent.speed);

        _agent.SetDestination(_currentTarget.position);
        _currentState = ENEMY_STATE.CHASING;
    }

    void StopChasing()
    {
        _agent.speed = 0.0f;
        _animator.SetFloat("speed", _agent.speed);

        _agent.ResetPath();
        _currentState = ENEMY_STATE.IDLE;
    }

    void AttackTarget()
    {
       _agent.speed = 0.0f;
       _animator.SetFloat("speed", _agent.speed);

       _animator.SetTrigger("attack");
       
       _agent.ResetPath();
       _currentState = ENEMY_STATE.CHASING;
    }

    float DistanceBetweenObjects (Transform a, Transform b)
    {
        var aPosition = new Vector2(a.position.x, a.position.z);
        var bPosition = new Vector2(b.position.x, b.position.z);
        return DistanceBetweenPoints(aPosition, bPosition);
    }

    float DistanceBetweenPoints(Vector2 x, Vector2 y)
    {
        var sqrX = Mathf.Pow(y.x - x.x, 2);
        var sqrY = Mathf.Pow(y.y - x.y, 2);
        
        return Mathf.Sqrt(sqrX + sqrY);
    }

    void BeginPatrol()
    {
        _agent.speed = 2.0f;
        _animator.SetFloat("speed", _agent.speed);

        _patrolPointIndex = 0;
        UpdateDestination(/* isPatrolling */ true);
        _currentState = ENEMY_STATE.PATROL;
    }

    void PatrolArea()
    {
        if (_currentPatrolPoint == null)
        {
            BeginPatrol();
        }
        else if (Vector3.Distance(this.transform.position, _currentPatrolPoint.position) <= 1)
        {
            IteratePatrolIndex();
            Debug.Log("Patrolling area");
            UpdateDestination(/* isPatrolling */ true);
        }
    }

    void IteratePatrolIndex()
    {
        _patrolPointIndex++;
        if (_patrolPointIndex >= _patrolWaypoints.Count)
        {
            _patrolPointIndex = 0;
        }
    }

    void UpdateDestination (bool isPatrolling)
    {
        if (isPatrolling)
        {
            _currentPatrolPoint = _patrolWaypoints[_patrolPointIndex];
            _currentTarget = _currentPatrolPoint;
            _agent.SetDestination(_currentTarget.position);
        }
    }

    void ReturnToOrigin()
    {
        if (_currentState != ENEMY_STATE.IDLE)
            _currentState = ENEMY_STATE.IDLE;
        
        _agent.speed = 2.0f;
        _animator.SetFloat("speed", _agent.speed);

        _agent.SetDestination(_originPoint.position);
    }

    private IEnumerator TriggerDeath()
    {
        _agent.speed = 0.0f;
        _animator.SetFloat("speed", _agent.speed);

        _agent.ResetPath();
        _currentState = ENEMY_STATE.RESTORED;

        _humanModel.SetActive(true);
        _currentModel = _humanModel;
        _animator = _currentModel.GetComponent<Animator>();

        _monsterModel.SetActive(false);
        _smokeEffect.SetActive(true);
        _animator.SetTrigger("dance");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        Destroy(this.transform.parent.gameObject);
    }
}
