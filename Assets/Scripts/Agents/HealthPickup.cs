using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private float playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<Health>().CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered ontrigger");
        if (other.tag == "Player")
        {
            Debug.Log("showtime");
            playerHealth += 1;
            Destroy(gameObject); // remove the cherry on pickup
        }
    }
}
