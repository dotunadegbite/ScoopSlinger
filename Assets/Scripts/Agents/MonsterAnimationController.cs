using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    public AudioClip OnAttackSfx;
    public AudioClip OnDamageSfx;

    MonsterStateMachine m_StateMachine;
    AudioSource m_AudioSource;
    void Awake()
    {
        m_StateMachine = GetComponentInParent<MonsterStateMachine>();
        m_AudioSource = GetComponentInParent<AudioSource>();
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
        if (OnDamageSfx)
        {
            // AudioUtility.CreateSFX(OnAttackSfx, transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
            m_AudioSource.PlayOneShot(OnDamageSfx);
        }
    }

    public void StopEating()
    {
        m_StateMachine.StopAnimationOverride();
    }

    public void StartAttack()
    {
        m_StateMachine.StartAnimationOverride();
        if (OnAttackSfx)
        {
            // AudioUtility.CreateSFX(OnAttackSfx, transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
            m_AudioSource.PlayOneShot(OnAttackSfx);
        }
    }

    public void StopAttack()
    {
        m_StateMachine.StopAnimationOverride();
    }
}
