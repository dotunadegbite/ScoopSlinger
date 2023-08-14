using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamInventoryManager : MonoBehaviour
{
    [SerializeField] private ProjectileBase[] m_ScoopProjectiles;
    [SerializeField] private int m_CurrentScoopIndex = 0;
    [SerializeField] private FlavorType[] AllScoopTypes;

    FlavorType m_ScoopType;

    public FlavorType CurrentPlayerType => m_ScoopType;

    // Start is called before the first frame update
    void Start()
    {
        m_ScoopType = AllScoopTypes[m_CurrentScoopIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("NextFlavor"))
        {
            m_CurrentScoopIndex += 1;
            if (m_CurrentScoopIndex > AllScoopTypes.Length - 1)
            {
                m_CurrentScoopIndex = 0;
            }

            m_ScoopType = AllScoopTypes[m_CurrentScoopIndex];

            Debug.Log("Current Flavor is: " + m_ScoopType);
        }
    }

    public ProjectileBase GetCurrentScoopType()
    {
        return m_ScoopProjectiles[m_CurrentScoopIndex];
    }
}
