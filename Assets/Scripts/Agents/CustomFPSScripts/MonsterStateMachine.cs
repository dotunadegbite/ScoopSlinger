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
        public float MinDistanceForAttack = 5.0f;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;

        public ParticleSystem[] OnDetectVfx;
        public AudioClip OnDetectSfx;

        [Header("Sound")] public AudioClip MovementSound;
        public MinMaxFloat PitchDistortionMovementSpeed;

        public AIState AiState { get; private set; }
        MonsterController m_MonsterController;
        MonsterType m_MonsterType;
        AudioSource m_AudioSource;

        const string k_AnimMoveSpeedParameter = "MoveSpeed";
        const string k_AnimAttackParameter = "Attack";
        const string k_AnimAlertedParameter = "Alerted";
        const string k_AnimOnDamagedParameter = "OnDamaged";
        const string k_AnimChasingParameter = "IsChasing";

        private float _agentRunningSpeed = 10.0f;

        private bool _isInAnimationOverride = false;
        void Start()
        {
            m_MonsterController = GetComponent<MonsterController>();
            DebugUtility.HandleErrorIfNullGetComponent<MonsterController, MonsterStateMachine>(m_MonsterController, this,
                gameObject);

            m_MonsterType = GetComponent<MonsterType>();

            m_MonsterController.onAttack += OnAttack;
            m_MonsterController.onDetectedTarget += OnDetectedTarget;
            m_MonsterController.onLostTarget += OnLostTarget;
            m_MonsterController.SetPathDestinationToClosestNode();
            m_MonsterController.onDamaged += OnDamaged;
            _agentRunningSpeed = m_MonsterType.Stats.MaxSpeed;

            // Start patrolling
            AiState = AIState.Patrol;

            // adding a audio source to play the movement sound on it
            m_AudioSource = GetComponent<AudioSource>();
            DebugUtility.HandleErrorIfNullGetComponent<AudioSource, MonsterStateMachine>(m_AudioSource, this, gameObject);
        }

        void Update()
        {
            UpdateAiStateTransitions();
            UpdateCurrentAiState();

            float moveSpeed = m_MonsterController.NavMeshAgent.velocity.magnitude;
            float animatorSpeed = moveSpeed;

            if (_isInAnimationOverride)
            {
                animatorSpeed = 0.0f;
                m_MonsterController.NavMeshAgent.speed = 0.0f;
            }
            else if (AiState == AIState.Patrol)
            {
                animatorSpeed = 3.0f;
                m_MonsterController.NavMeshAgent.speed = 3.0f;
            }
            else if (!m_MonsterController.ChaseTriggerModule.IsTargetInAttackRange && (AiState == AIState.Follow || AiState == AIState.Attack))
            {
                animatorSpeed = _agentRunningSpeed;
                m_MonsterController.NavMeshAgent.speed = _agentRunningSpeed;
            }
            else if (m_MonsterController.ChaseTriggerModule.IsTargetInAttackRange && (AiState == AIState.Follow || AiState == AIState.Attack))
            {
                animatorSpeed = 0.0f;
                m_MonsterController.NavMeshAgent.speed = 0.0f;
            }

            Animator.SetFloat(k_AnimMoveSpeedParameter, animatorSpeed);
        }

        public void StartAnimationOverride()
        {
            _isInAnimationOverride = true;
            m_MonsterController.StopAgent();
        }

        public void StopAnimationOverride()
        {
            _isInAnimationOverride = false;
            m_MonsterController.ResumeAgent();
        }

        void UpdateAiStateTransitions()
        {
            // Handle transitions 
            switch (AiState)
            {
                case AIState.Follow:
                    // Transition to attack when there is a line of sight to the target
                    if (m_MonsterController.HasSeenPlayer && m_MonsterController.IsTargetInAttackRange)
                    {
                        AiState = AIState.Attack;
                        m_MonsterController.SetNavDestination(transform.position, /* saveDestination */ true);
                    }

                    break;
                case AIState.Attack:
                    if (Vector3.Distance(m_MonsterController.KnownDetectedTarget.transform.position,
                            m_MonsterController.ChaseTriggerModule.DetectionSourcePoint.position)
                        >= MinDistanceForAttack)
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
                    m_MonsterController.SetNavDestination(m_MonsterController.GetDestinationOnPath(), /* saveDestination */ true);
                    break;
                case AIState.Follow:
                    m_MonsterController.SetNavDestination(m_MonsterController.KnownDetectedTarget.transform.position, /* saveDestination */ true);
                    m_MonsterController.OrientTowards(m_MonsterController.KnownDetectedTarget.transform.position);
                    break;
                case AIState.Attack:
                    if (Vector3.Distance(m_MonsterController.KnownDetectedTarget.transform.position,
                            m_MonsterController.ChaseTriggerModule.DetectionSourcePoint.position)
                        >= MinDistanceForAttack)
                    {
                        m_MonsterController.SetNavDestination(m_MonsterController.KnownDetectedTarget.transform.position, /* saveDestination */ true);
                    }
                    else
                    {
                        if (m_MonsterController.IsTargetInAttackRange)
                        {
                            var isInAttackingRange = Vector3.Distance(m_MonsterController.KnownDetectedTarget.transform.position,
                            m_MonsterController.ChaseTriggerModule.DetectionSourcePoint.position) >= MinDistanceForAttack;

                            m_MonsterController.SetNavDestination(transform.position, /* saveDestination */ true);
                            m_MonsterController.TryAttack();
                        }
                        else
                        {
                            m_MonsterController.SetNavDestination(m_MonsterController.KnownDetectedTarget.transform.position, /* saveDestination */ true);
                        }
                    }

                    m_MonsterController.OrientTowards(m_MonsterController.KnownDetectedTarget.transform.position);
                    
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
