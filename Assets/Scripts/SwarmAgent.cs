using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwarmAgent : MonoBehaviour
{
    public enum ENEMY_STATE
    {
        IDLE,
        CHASING
    }

    NavMeshAgent _agent;
    ENEMY_STATE _currentState = ENEMY_STATE.IDLE;
    Animator _animator;

    [SerializeField]
    Transform _targetToFollow;

    [SerializeField]
    float _maxDistance = 5.0f;

    [SerializeField]
    float _currentDistance;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentDistance = DistanceBetweenPoints(new Vector2(_targetToFollow.position.x, _targetToFollow.position.z), new Vector2(transform.position.x, transform.position.z));
        switch (_currentState)
        {
            case ENEMY_STATE.IDLE:
                if (_currentDistance >= _maxDistance)
                {
                    ChaseTarget(_currentDistance);
                }
                break;
            case ENEMY_STATE.CHASING:
                if (_currentDistance <= _maxDistance)
                {
                    StopChasing();
                }
                else
                {
                    ChaseTarget(_currentDistance);
                }
                break;
            default:
                break;
        }
    }

    void ChaseTarget(float distanceToTarget)
    {
        var animatorSpeed = distanceToTarget <= 30 && distanceToTarget > 5 ? 5f : 2f;
        _animator.SetFloat("speed", animatorSpeed);

        _agent.SetDestination(_targetToFollow.position);
        _currentState = ENEMY_STATE.CHASING;
    }

    void StopChasing()
    {
        _animator.SetFloat("speed", 0.0f);
        _agent.ResetPath();
        _currentState = ENEMY_STATE.IDLE;
    }


    float DistanceBetweenPoints(Vector2 x, Vector2 y)
    {
        var sqrX = Mathf.Pow(y.x - x.x, 2);
        var sqrY = Mathf.Pow(y.y - x.y, 2);
        
        return Mathf.Sqrt(sqrX + sqrY);
    }
}
