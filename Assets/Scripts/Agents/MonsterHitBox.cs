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
        //find flavor types for ice cream scoop based on game manager
        GameObject manager = GameObject.Find("GameManager");
        FlavorType scoopFlavorType = manager.GetComponent<IceCreamHandler>().managerFlavor;

        //grab monster flavor type
        FlavorType myFlavorType = GetComponent<MyFlavorType>().scoopFlavor;
        
        //check the flavor types of both (can remove debug statements once UI is implemented)
        //Debug.Log("Enemy flavor is " + myFlavorType);
        //Debug.Log("Scoop flavor is " + scoopFlavorType);

        //check based on both the tag (to assure the projectile is an ice cream scoop) and flavor type
        if (other.CompareTag("IceCreamScoop") && scoopFlavorType == myFlavorType)
        {
            m_Health.TakeDamage(10f, other.gameObject);
        }
    }
}
