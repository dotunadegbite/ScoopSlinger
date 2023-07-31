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

        if (other.CompareTag("IceCreamScoop"))
        {
            _agent.TakeDamage(10); // Max damage to enemy. Needs to be adjusted if max HP increased
        }
    }

}
