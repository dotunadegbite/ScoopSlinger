using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAttackZone : MonoBehaviour
{
    public UnityAction onPlayerEnterZone;
    public UnityAction onPlayerExitZone;

    private MeshRenderer _mesh;

    void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
        _mesh.enabled = false;
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
        if (other.CompareTag("Player"))
        {
            OnPlayerEnterZone();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExitZone();
        }
    }

    public virtual void OnPlayerEnterZone() => onPlayerEnterZone?.Invoke();

    public virtual void OnPlayerExitZone() => onPlayerExitZone?.Invoke();
}
