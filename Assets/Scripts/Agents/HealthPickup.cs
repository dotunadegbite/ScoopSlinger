using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Start is called before the first frame update
    Health m_PlayerHealth;
    private float playerHealth;

    void Start()
    {
        PlayerCharacterController playerCharacterController = GameObject.FindObjectOfType<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, PlayerHeartBar>(playerCharacterController, this);
        m_PlayerHealth = playerCharacterController.GetComponent<Health>();
        playerHealth = m_PlayerHealth.CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(playerHealth + " Health pickup check 1");
        Debug.Log("entered ontrigger");
        if (other.tag == "Player")
        {
            Debug.Log("showtime");
            playerHealth += 1;
            Destroy(gameObject); // remove the cherry on pickup
            Debug.Log(playerHealth + " Health pickup check 2");
        }
    }
}
