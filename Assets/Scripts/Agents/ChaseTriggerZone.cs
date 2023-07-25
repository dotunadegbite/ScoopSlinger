using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTriggerZone : MonoBehaviour
{
    [SerializeField] SwarmAgent _agent;
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
        if (other.CompareTag("Player"))
        {
            _agent.HandlePlayerEnterChaseZone(other.transform);
        }
    }
    
    /* void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            var playerComponent = other.gameObject.GetComponentInParent<PlayerController>();
            if (playerComponent != null)
            {
                playerComponent.HandleEnterConversationArea(_characterToSpeak);
                playerComponent.PlayerTriggeredConversationEvent += _characterToSpeak.HandleConversationTriggered;
                _characterToSpeak.ConversationEndedEvent += playerComponent.HandleConversationEnd;
            }
        }
    }*/
}
