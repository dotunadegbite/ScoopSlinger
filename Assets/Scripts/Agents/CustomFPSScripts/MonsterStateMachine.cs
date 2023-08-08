using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : MonoBehaviour
{
    public enum AIState
        {
            Patrol,
            Follow,
            Attack,
        }

        public Animator Animator;

        [Tooltip("Fraction of the enemy's attack range at which it will stop moving towards target while attacking")]
        [Range(0f, 1f)]
        public float AttackStopDistanceRatio = 0.5f;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;

        public ParticleSystem[] OnDetectVfx;
        public AudioClip OnDetectSfx;

        [Header("Sound")] public AudioClip MovementSound;
        public MinMaxFloat PitchDistortionMovementSpeed;

        public AIState AiState { get; private set; }
        MonsterController m_MonsterController;
        AudioSource m_AudioSource;
        bool canAttack = false;

        const string k_AnimMoveSpeedParameter = "MoveSpeed";
        const string k_AnimAttackParameter = "Attack";
        const string k_AnimAlertedParameter = "Alerted";
        const string k_AnimOnDamagedParameter = "OnDamaged";
        const string k_AnimChasingParameter = "IsChasing";

        const float k_WalkingSpeed = 3.0f;
        const float k_RunningSpeed = 10.0f;

        void Start()
        {
            m_MonsterController = GetComponent<MonsterController>();
            DebugUtility.HandleErrorIfNullGetComponent<MonsterController, MonsterStateMachine>(m_MonsterController, this,
                gameObject);

            m_MonsterController.onAttack += OnAttack;
            m_MonsterController.onDetectedTarget += OnDetectedTarget;
            m_MonsterController.onLostTarget += OnLostTarget;
            m_MonsterController.SetPathDestinationToClosestNode();
            m_MonsterController.onDamaged += OnDamaged;

            // Start patrolling
            AiState = AIState.Patrol;

            // adding a audio source to play the movement sound on it
            m_AudioSource = GetComponent<AudioSource>();
            DebugUtility.HandleErrorIfNullGetComponent<AudioSource, MonsterStateMachine>(m_AudioSource, this, gameObject);
            m_AudioSource.clip = MovementSound;
            m_AudioSource.Play();
        }

        void Update()
        {
            UpdateAiStateTransitions();
            UpdateCurrentAiState();

            float moveSpeed = m_MonsterController.NavMeshAgent.velocity.magnitude;
            float animatorSpeed = moveSpeed;
            if (AiState == AIState.Patrol)
            {
                animatorSpeed = 3.0f;
                m_MonsterController.NavMeshAgent.speed = 3.0f;
            }
            else if (!canAttack && (AiState == AIState.Follow || AiState == AIState.Attack))
            {
                animatorSpeed = 10.0f;
                m_MonsterController.NavMeshAgent.speed = 10.0f;
            }
            else if (canAttack && (AiState == AIState.Follow || AiState == AIState.Attack))
            {
                animatorSpeed = 0.0f;
                m_MonsterController.NavMeshAgent.speed = 0.0f;
            }

            Animator.SetFloat(k_AnimMoveSpeedParameter, animatorSpeed);

            // changing the pitch of the movement sound depending on the movement speed
            m_AudioSource.pitch = Mathf.Lerp(PitchDistortionMovementSpeed.Min, PitchDistortionMovementSpeed.Max,
                moveSpeed / m_MonsterController.NavMeshAgent.speed);
        }

        void UpdateAiStateTransitions()
        {
            // Handle transitions 
            switch (AiState)
            {
                case AIState.Follow:
                    // Transition to attack when there is a line of sight to the target
                    if (m_MonsterController.IsSeeingTarget && m_MonsterController.IsTargetInAttackRange)
                    {
                        AiState = AIState.Attack;
                        m_MonsterController.SetNavDestination(transform.position);
                    }

                    break;
                case AIState.Attack:
                    // Transition to follow when no longer a target in attack range
                    if (!m_MonsterController.IsTargetInAttackRange)
                    {
                        AiState = AIState.Follow;
                    }

                    break;
            }
        }

        void UpdateCurrentAiState()
        {
            // Handle logic 
            switch (AiState)
            {
                case AIState.Patrol:
                    m_MonsterController.UpdatePathDestination();
                    m_MonsterController.SetNavDestination(m_MonsterController.GetDestinationOnPath());
                    break;
                case AIState.Follow:
                    m_MonsterController.SetNavDestination(m_MonsterController.KnownDetectedTarget.transform.position);
                    m_MonsterController.OrientTowards(m_MonsterController.KnownDetectedTarget.transform.position);
                    break;
                case AIState.Attack:
                    canAttack = false;
                    if (Vector3.Distance(m_MonsterController.KnownDetectedTarget.transform.position,
                            m_MonsterController.DetectionModule.DetectionSourcePoint.position)
                        >= (AttackStopDistanceRatio * m_MonsterController.DetectionModule.AttackRange))
                    {
                        m_MonsterController.SetNavDestination(m_MonsterController.KnownDetectedTarget.transform.position);
                    }
                    else
                    {
                        canAttack = true;
                        m_MonsterController.SetNavDestination(transform.position);
                    }

                    m_MonsterController.OrientTowards(m_MonsterController.KnownDetectedTarget.transform.position);

                    if (canAttack)
                    {
                        m_MonsterController.TryAttack(m_MonsterController.KnownDetectedTarget.transform.position);
                    }
                    
                    break;
            }
        }

        void OnAttack()
        {
            Animator.SetTrigger(k_AnimAttackParameter);
        }

        void OnDetectedTarget()
        {
            if (AiState == AIState.Patrol)
            {
                AiState = AIState.Follow;
            }

            for (int i = 0; i < OnDetectVfx.Length; i++)
            {
                OnDetectVfx[i].Play();
            }

            if (OnDetectSfx)
            {
                AudioUtility.CreateSFX(OnDetectSfx, transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
            }

            Animator.SetBool(k_AnimAlertedParameter, true);
        }

        void OnLostTarget()
        {
            if (AiState == AIState.Follow || AiState == AIState.Attack)
            {
                AiState = AIState.Patrol;
            }

            for (int i = 0; i < OnDetectVfx.Length; i++)
            {
                OnDetectVfx[i].Stop();
            }

            Animator.SetBool(k_AnimAlertedParameter, false);
        }

        void OnDamaged()
        {
            /* if (RandomHitSparks.Length > 0)
            {
                int n = Random.Range(0, RandomHitSparks.Length - 1);
                RandomHitSparks[n].Play();
            }*/

            Animator.SetTrigger(k_AnimOnDamagedParameter);
        }
}
