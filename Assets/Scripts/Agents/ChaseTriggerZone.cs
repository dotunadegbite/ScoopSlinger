using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTriggerZone : MonoBehaviour
{
    [SerializeField] SwarmAgent _agent;
    [SerializeField] MeshRenderer _mesh;

    void Awake()
    {
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
            Debug.Log("Player entered chase zone");
            _agent.HandlePlayerEnterChaseZone(other.transform);
        }
    }
}
