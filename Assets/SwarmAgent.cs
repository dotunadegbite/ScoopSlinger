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

    [SerializeField]
    Transform _targetToFollow;

    [SerializeField]
    float _maxDistance = 5.0f;

    [SerializeField]
    float _currentDistance;

    void Awake()
    {
        _agent = GetComponentInChildren<NavMeshAgent>();
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
                    ChaseTarget();
                }
                break;
            case ENEMY_STATE.CHASING:
                if (_currentDistance <= _maxDistance)
                {
                    StopChasing();
                }
                else
                {
                    ChaseTarget();
                }
                break;
            default:
                break;
        }
    }

    void ChaseTarget()
    {
        _agent.SetDestination(_targetToFollow.position);
        _currentState = ENEMY_STATE.CHASING;
    }

    void StopChasing()
    {
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
