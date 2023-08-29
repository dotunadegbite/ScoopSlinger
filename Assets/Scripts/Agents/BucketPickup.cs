using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPickup : MonoBehaviour
{
    IceCreamInventoryManager m_ScoopAmmoManager;
    private int currentAmmo;
    public FlavorType bucketFlavor;
    public int ammoIncrement;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = FindObjectOfType<IceCreamInventoryManager>().CurrentAmmo; // grabs the current ammo.
        bucketFlavor = GetComponent<MyFlavorType>().scoopFlavor; // grabs the specific bucket's type
    }

    private void Update()
    {
        transform.Rotate(0f, -50 * Time.deltaTime, 50 * Time.deltaTime, Space.Self); //rotation for ammo pickup
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            IceCreamInventoryManager playerIceCreamInvenManager = other.GetComponent<IceCreamInventoryManager>();
            playerIceCreamInvenManager.AddAmmoByType(ammoIncrement, bucketFlavor);
            Destroy(gameObject); // remove the bucket on pickup
        }
    }
}
