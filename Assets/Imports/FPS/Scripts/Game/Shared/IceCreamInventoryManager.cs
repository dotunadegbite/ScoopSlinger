using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoopAmmoChangedEventArgs : EventArgs
{
    public FlavorType AmmoType { get; }
    public int CurrentAmmoCount { get; }
    public int MaxAmmoCount { get; }

    public ScoopAmmoChangedEventArgs(FlavorType flavor, int current, int max)
    {
        AmmoType = flavor;
        CurrentAmmoCount = current;
        MaxAmmoCount = max;
    }
}
public class IceCreamInventoryManager : MonoBehaviour
{
    [SerializeField] private ProjectileBase[] m_ScoopProjectiles;
    [SerializeField] private FlavorType[] AllScoopTypes;
    [SerializeField] private int MaxScoopAmount = 10;

    FlavorType m_ScoopType;
    Dictionary<FlavorType, int> m_ScoopAmmoCounts;

    public FlavorType CurrentPlayerType => m_ScoopType;

    public event EventHandler<ScoopAmmoChangedEventArgs> OnScoopAmmoChangedEvent;

    public int CurrentAmmo
    {
        get => m_ScoopAmmoCounts[m_ScoopType];
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ScoopType = FlavorType.CHOCOLATE;
        m_ScoopAmmoCounts = new Dictionary<FlavorType, int>();

        foreach (var flavor in AllScoopTypes)
        {
            m_ScoopAmmoCounts[flavor] = MaxScoopAmount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_ScoopType = FlavorType.CHOCOLATE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_ScoopType = FlavorType.VANILLA;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_ScoopType = FlavorType.BERRY;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_ScoopType = FlavorType.MINT;
        }
    }

    public ProjectileBase GetCurrentScoopType()
    {
        return m_ScoopProjectiles[(int)m_ScoopType];
    }

    public void UpdateAmmo()
    {
        m_ScoopAmmoCounts[m_ScoopType] = Mathf.Max(0, m_ScoopAmmoCounts[m_ScoopType] - 1);

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs (m_ScoopType, m_ScoopAmmoCounts[m_ScoopType], MaxScoopAmount);
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs);
    }
    
    protected virtual void OnRaiseScoopAmmoChangedEvent(ScoopAmmoChangedEventArgs e)
    {
        EventHandler<ScoopAmmoChangedEventArgs> raiseEvent = OnScoopAmmoChangedEvent;
        if (raiseEvent != null)
        {
            raiseEvent(this, e);
        }
    }
}
