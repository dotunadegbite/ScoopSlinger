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

    [SerializeField] private float PawnSpeed = 10.0f;
    [SerializeField] private float AverageSpeed = 8.0f;
    [SerializeField] private float TankSpeed = 5.0f;

    [SerializeField] private float PawnHealth = 1.0f;
    [SerializeField] private float AverageHealth = 2.0f;
    [SerializeField] private float TankHealth = 3.0f;

    [SerializeField] private int PawnDamage = 1;
    [SerializeField] private int AverageDamage = 2;
    [SerializeField] private int TankDamage = 3;

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
                stats = new MonsterStats(PawnHealth, PawnSpeed, PawnDamage);
                break;
            case EnemyType.AVERAGE:
                stats = new MonsterStats(AverageHealth, AverageSpeed, AverageDamage);
                break;
            case EnemyType.TANK:
                stats = new MonsterStats(TankHealth, TankSpeed, TankDamage);
                break;
        }

        Stats = stats;
    }

    

    public EnemyType AgentType => m_AgentType;
    public FlavorType IceCreamType => m_ScoopNeeded;

}
