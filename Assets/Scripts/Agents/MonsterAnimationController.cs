using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    MonsterStateMachine m_StateMachine;

    void Awake()
    {
        m_StateMachine = GetComponentInParent<MonsterStateMachine>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEating()
    {
        m_StateMachine.StartAnimationOverride();
    }

    public void StopEating()
    {
        m_StateMachine.StopAnimationOverride();
    }

    public void StartAttack()
    {
        m_StateMachine.StartAnimationOverride();
    }

    public void StopAttack()
    {
        m_StateMachine.StopAnimationOverride();
    }
}
