using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChaseTriggerZone : MonoBehaviour
{
    // [SerializeField] SwarmAgent _agent;
    [SerializeField] MeshRenderer _mesh;
    [SerializeField] MonsterController _monsterController;
    [SerializeField] MonsterAttackZone _attackZone;
    

    public UnityAction onDetectedTarget;
    public UnityAction onLostTarget;
    public Animator Animator { get; private set; }
    public GameObject KnownDetectedTarget { get; private set; }
    public bool IsTargetInAttackRange { get; private set; }
    public bool HasSeenPlayer { get; private set; }
    public bool HadKnownTarget { get; private set; }

    public float AttackRange { get; private set; }
    public Transform DetectionSourcePoint { get => _detectionSourcePoint;}

    [SerializeField] private Transform _detectionSourcePoint;
    
    /* public float DetectionRange = 20f;
    public float AttackRange = 10f; */

     const string k_AnimAttackParameter = "Attack";
     const string k_AnimOnDamagedParameter = "OnDamaged";

    void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
        _mesh.enabled = false;

        _attackZone.onPlayerEnterZone += HandleEnterAttackZone;
        _attackZone.onPlayerExitZone += HandleExitAttackZone;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HadKnownTarget = KnownDetectedTarget != null;
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && !HadKnownTarget)
        {
            OnDetect();
            KnownDetectedTarget = other.gameObject;
            HasSeenPlayer = true;
        }
    }

    void HandleEnterAttackZone()
    {
        IsTargetInAttackRange = true;
    }

    void HandleExitAttackZone()
    {
        IsTargetInAttackRange = false;
    }

    public virtual void OnLostTarget() => onLostTarget?.Invoke();

    public virtual void OnDetect() => onDetectedTarget?.Invoke();

    public virtual void OnDamaged(GameObject damageSource)
    {
        if (Animator)
        {
            Animator.SetTrigger(k_AnimOnDamagedParameter);
        }
    }

    public virtual void OnAttack()
    {
        if (Animator)
        {
            Animator.SetTrigger(k_AnimAttackParameter);
        }
    }
}
