using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlavorType
{
    CHOCOLATE,
    MINT,
    BERRY,
    VANILLA
}

public class IceCreamScoop : MonoBehaviour
{
    [SerializeField] private FlavorType ScoopType;

    void Awake()
    {
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FlavorType Flavor => ScoopType;
}
