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
            var playerType = m_PlayerInventory.CurrentPlayerType;

            Debug.Log("Player has type: " + playerType + " enemy has type " + monsterScoop);
            if (monsterScoop == playerType)
            {
                m_Health.TakeDamage(monsterStats.Damage, other.gameObject);
            }
        }
    }
}
