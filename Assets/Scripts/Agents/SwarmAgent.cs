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
        CHASING
    }

    NavMeshAgent _agent;
    ENEMY_STATE _currentState = ENEMY_STATE.PATROL;
    Animator _animator;
    float _currentDistance;
    Transform _currentPatrolPoint;
    Transform _currentTarget;
    int _patrolPointIndex;
    List<Transform> _patrolWaypoints;


    [SerializeField] Transform _targetToFollow;
    [SerializeField] float _maxDistanceFromOrigin = 30.0f;
    [SerializeField] float _minDistanceFromTarget = 3.0f;
    [SerializeField] float _minDistanceFromOrigin = 3.0f;
    [SerializeField] GameObject _patrolPointsParent;
    [SerializeField] Transform _originPoint;
    

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _patrolWaypoints = _patrolPointsParent.GetComponentsInChildren<Transform>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromOrigin, distanceFromTarget;
        switch (_currentState)
        {
            case ENEMY_STATE.IDLE:
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
                Debug.Log("Origin distance: " + distanceFromOrigin + " Target Distance: " + distanceFromTarget);
                if (distanceFromOrigin >= _maxDistanceFromOrigin)
                {
                    ReturnToOrigin();
                }
                else if (distanceFromTarget <= _minDistanceFromTarget)
                {
                    StopChasing();
                    // Attack / Stop Chasing
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
        // 0. If not patrolling then continue
        if (_currentState != ENEMY_STATE.PATROL)
            return;
        // 2. Clear Patrol
        _patrolPointIndex = 0;
        _currentPatrolPoint = null;

        // 3. Update state to chase
        _currentState = ENEMY_STATE.CHASING;
        _currentTarget = target;

        Debug.Log("New state: " + _currentState.ToString());
    }

    void ChaseTarget()
    {
        // var animatorSpeed = distanceToTarget <= 30 && distanceToTarget > 5 ? 5f : 2f;
        _animator.SetFloat("speed", 5f);

        _agent.SetDestination(_currentTarget.position);
        _currentState = ENEMY_STATE.CHASING;
    }

    void StopChasing()
    {
        _animator.SetFloat("speed", 0.0f);
        _agent.ResetPath();
        _currentState = ENEMY_STATE.IDLE;
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
        _animator.SetFloat("speed", 2f);
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
        else if (Vector3.Distance(this.transform.position, _currentPatrolPoint.position) < 1)
        {
            IteratePatrolIndex();
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
        
        _animator.SetFloat("speed", 2f);
        _agent.SetDestination(_originPoint.position);
    }
}
