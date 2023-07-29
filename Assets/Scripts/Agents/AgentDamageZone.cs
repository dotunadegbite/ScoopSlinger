using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

public class AgentDamageZone : MonoBehaviour
{
    [SerializeField] private SwarmAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            var otherDamage = other.gameObject.GetComponent<Damageable>();
            _agent.DamagePlayer();

            otherDamage.InflictDamage(10, false, _agent.gameObject);
        }
    }
}
