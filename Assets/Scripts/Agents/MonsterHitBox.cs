using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterHitBox : MonoBehaviour
{
    [SerializeField] private Health m_Health;

    void Awake()
    {
        m_Health = GetComponentInParent<Health>();
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
        if (other.CompareTag("IceCreamScoop"))
        {
            m_Health.TakeDamage(10f, other.gameObject);
        }
    }
}
