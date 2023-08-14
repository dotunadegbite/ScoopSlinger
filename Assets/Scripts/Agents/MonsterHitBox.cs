using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterHitBox : MonoBehaviour
{
    [SerializeField] private Health m_Health;

    private MonsterType m_MonsterType;
    private IceCreamInventoryManager m_PlayerInventory;

    
    void Awake()
    {
        m_Health = GetComponentInParent<Health>();
        m_MonsterType = GetComponentInParent<MonsterType>();

        m_PlayerInventory = FindObjectOfType<IceCreamInventoryManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Health.MaxHealth = m_MonsterType.Stats.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //check based on both the tag (to assure the projectile is an ice cream scoop) and flavor type
        if (other.CompareTag("IceCreamScoop"))
        {
            var monsterScoop = m_MonsterType.IceCreamType;
            var monsterStats = m_MonsterType.Stats;
            Debug.Log("Monster type: " + monsterScoop);

            var playerType = m_PlayerInventory.CurrentPlayerType;
            Debug.Log("Current Player Type: " + playerType);

            // && scoop.GetScoopType().Equals())
            // && scoop.F.Equals(m_MonsterType.IceCreamType)
            if (monsterScoop == playerType)
            {
                m_Health.TakeDamage(monsterStats.Damage, other.gameObject);
                Debug.Log("Current Health: " + m_Health.CurrentHealth);
            }
            // m_Health.TakeDamage(10f, other.gameObject);
        }
    }
}
