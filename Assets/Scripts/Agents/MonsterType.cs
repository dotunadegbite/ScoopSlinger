using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterType : MonoBehaviour
{
    [System.Serializable]
    public struct MonsterStats
    {
        public float MaxHealth { get; private set;}
        public float MaxSpeed { get; private set; }

        public int Damage { get; private set; }

        public MonsterStats(float maxHealth, float maxSpeed, int damage)
        {
            MaxHealth = maxHealth;
            MaxSpeed = maxSpeed;
            Damage = damage;
        }
    }

    [SerializeField] private EnemyType m_AgentType;
    [SerializeField] private FlavorType m_ScoopNeeded;

    [Tooltip("Max Health, Speed and Damage for this monster")]
    public MonsterStats Stats;

    // Start is called before the first frame update
    void Start()
    {
        SetMonsterStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetMonsterStats()
    {
        MonsterStats stats = new MonsterStats(0f, 0f, 0);
        switch (m_AgentType)
        {
            case EnemyType.PAWN:
                stats = new MonsterStats(1.0f, 10.0f, 1);
                break;
            case EnemyType.AVERAGE:
                stats = new MonsterStats(2.0f, 8.0f, 2);
                break;
            case EnemyType.TANK:
                stats = new MonsterStats(3.0f, 5.0f, 3);
                break;
        }

        Stats = stats;
    }

    

    public EnemyType AgentType => m_AgentType;
    public FlavorType IceCreamType => m_ScoopNeeded;

}
