using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterClaw : MonoBehaviour
{

    [SerializeField] private MonsterType _monster;
    private bool _isAttacking = false;

    void Awake()
    {
        _monster = GetComponentInParent<MonsterType>();
    }
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
        if (other.CompareTag("Player") && !_isAttacking)
        {
            _isAttacking = true;
            var otherDamage = other.gameObject.GetComponent<Damageable>();
            Debug.Log("Enemy: " + _monster.IceCreamType + " does " + _monster.Stats.Damage + " damage");
            otherDamage.InflictDamage(_monster.Stats.Damage, false, _monster.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _isAttacking)
        {
            _isAttacking = false;
        }
    }
}
