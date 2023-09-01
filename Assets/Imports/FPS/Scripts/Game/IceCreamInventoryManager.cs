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
    PlayerInputHandler m_InputHandler;

    public FlavorType CurrentPlayerType => m_ScoopType;

    public event EventHandler<ScoopAmmoChangedEventArgs> OnScoopAmmoChangedEvent;

    public int CurrentAmmo
    {
        get => m_ScoopAmmoCounts[m_ScoopType];
    }

    void Awake()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initalize();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void Initalize()
    {
        m_ScoopType = FlavorType.CHOCOLATE;
        m_ScoopAmmoCounts = new Dictionary<FlavorType, int>();

        foreach (var flavor in AllScoopTypes)
        {
            m_ScoopAmmoCounts[flavor] = MaxScoopAmount;
        }

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs (m_ScoopType, m_ScoopAmmoCounts[m_ScoopType], MaxScoopAmount);
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs);
    }

    public int GetAmmoCountByFlavor(FlavorType flavor)
    {
        return m_ScoopAmmoCounts[flavor];
    }

    public ProjectileBase GetCurrentScoopType()
    {
        return m_ScoopProjectiles[(int)m_ScoopType];
    }

    public void UpdateAmmo()
    {
        m_ScoopAmmoCounts[m_ScoopType] = Mathf.Max(0, m_ScoopAmmoCounts[m_ScoopType] - 1);
        Debug.Log("New scoop amount: " + m_ScoopAmmoCounts[m_ScoopType]);

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs (m_ScoopType, m_ScoopAmmoCounts[m_ScoopType], MaxScoopAmount);
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs);
    }

    public void AddAmmoByType(int scoopDelta, FlavorType iceCreamType)
    {
        var updatedAmmoAmmount = Mathf.Min(MaxScoopAmount, m_ScoopAmmoCounts[iceCreamType] + scoopDelta);
        m_ScoopAmmoCounts[iceCreamType] = updatedAmmoAmmount;

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs(iceCreamType, m_ScoopAmmoCounts[iceCreamType], MaxScoopAmount); // set the event arguments (refer to top)
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs); // fire the event
    }

    public void SwitchScoop (bool switchUp)
    {
        var currentIndex = (int)m_ScoopType;
        int nextIndex = switchUp ? Mathf.Max(0, currentIndex - 1) :  Mathf.Min(AllScoopTypes.Length - 1, currentIndex + 1);

        m_ScoopType = (FlavorType)nextIndex;

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs(m_ScoopType, m_ScoopAmmoCounts[m_ScoopType], MaxScoopAmount); // set the event arguments (refer to top)
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs); // fire the event
    }

    public void SwitchScoopToIndex (int scoopIndex)
    {
        m_ScoopType = (FlavorType)scoopIndex - 1;

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs(m_ScoopType, m_ScoopAmmoCounts[m_ScoopType], MaxScoopAmount); // set the event arguments (refer to top)
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs); // fire the event
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
