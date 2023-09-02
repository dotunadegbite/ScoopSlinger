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
    [SerializeField] private FlavorType[] m_AllScoopTypes;
    [SerializeField] private int MaxScoopAmount = 10;

    [SerializeField] private int StartingChocolateAmount = 0;
    [SerializeField] private int StartingVanillaAmount = 0;
    [SerializeField] private int StartingBerryAmount = 0;
    [SerializeField] private int StartingMintAmount = 0;

    FlavorType m_ScoopType;
    Dictionary<FlavorType, int> m_ScoopAmmoCounts;

     Dictionary<FlavorType, int> m_StartingScoopAmmoCounts;
    PlayerInputHandler m_InputHandler;

    public FlavorType CurrentPlayerType => m_ScoopType;

    public event EventHandler<ScoopAmmoChangedEventArgs> OnScoopAmmoChangedEvent;

    public int CurrentAmmo
    {
        get => m_ScoopAmmoCounts[m_ScoopType];
    }

    public FlavorType[] AllScoopTypes { get => m_AllScoopTypes;} 

    void Awake()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();

        m_StartingScoopAmmoCounts = new Dictionary<FlavorType, int>
        {
            { FlavorType.CHOCOLATE, StartingChocolateAmount },
            { FlavorType.VANILLA, StartingVanillaAmount },
            { FlavorType.BERRY, StartingBerryAmount },
            { FlavorType.MINT, StartingMintAmount },
        };
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

        foreach (var flavor in m_AllScoopTypes)
        {
            m_ScoopAmmoCounts[flavor] = m_StartingScoopAmmoCounts[flavor];
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

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs (m_ScoopType, m_ScoopAmmoCounts[m_ScoopType], MaxScoopAmount);
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs);
    }

    public void AddAmmoByType(int scoopDelta, FlavorType iceCreamType)
    {
        m_ScoopType = iceCreamType;
        var updatedAmmoAmmount = Mathf.Min(MaxScoopAmount, m_ScoopAmmoCounts[m_ScoopType] + scoopDelta);
        m_ScoopAmmoCounts[m_ScoopType] = updatedAmmoAmmount;

        var ammoChangedEventArgs = new ScoopAmmoChangedEventArgs(m_ScoopType, m_ScoopAmmoCounts[m_ScoopType], MaxScoopAmount); // set the event arguments (refer to top)
        this.OnRaiseScoopAmmoChangedEvent(ammoChangedEventArgs); // fire the event
    }

    public void SwitchScoop (bool switchUp)
    {
        var currentIndex = (int)m_ScoopType;
        int nextIndex = switchUp ? Mathf.Max(0, currentIndex - 1) :  Mathf.Min(m_AllScoopTypes.Length - 1, currentIndex + 1);

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
