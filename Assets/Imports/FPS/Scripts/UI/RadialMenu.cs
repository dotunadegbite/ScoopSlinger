using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RadialMenu : MonoBehaviour
{
    [SerializeField]
    GameObject m_EntryPrefab;

    [SerializeField]
    float m_MenuRadius = 300f;

    [SerializeField]
    RectTransform m_MenuRectTransform;

    [SerializeField]
    private Sprite[] ScoopImages;

    List<RadialMenuEntry> m_Entries;

    IceCreamInventoryManager m_IceCreamInventory;

    Action<bool> m_CloseMenuCallback;

    // Start is called before the first frame update
    void Start()
    {
        m_IceCreamInventory = FindObjectOfType<IceCreamInventoryManager>();
        m_Entries = new List<RadialMenuEntry>();
    }

    public void Open()
    {
        /* for (var i = 0; i < 4; i++)
        {
            AddEntry(i);
        } */

        for (int i = 0; i < m_IceCreamInventory.AllScoopTypes.Length; i++)
        {
            var flavor = m_IceCreamInventory.AllScoopTypes[i];
            var icon = ScoopImages[(int)flavor];
            var ammoCount = m_IceCreamInventory.GetAmmoCountByFlavor(flavor);
            AddEntry(i + 1, icon, ammoCount);
        }

        RearrangeEntries();
    }

    public void Close()
    {
        foreach (RectTransform item in m_MenuRectTransform)
        {
            Destroy(item.gameObject);
        }

        m_Entries.Clear();
    }

    public void SetCloseMenuAction(Action<bool> onCloseMenuCallback)
    {
        m_CloseMenuCallback = onCloseMenuCallback;
    }

    void AddEntry(int index, Sprite icon, int ammoCount)
    {
        var entry = Instantiate(m_EntryPrefab, m_MenuRectTransform);

        void OnClickScoop()
        {
            m_IceCreamInventory.SwitchScoopToIndex(index);
            m_CloseMenuCallback(false);
        }

        RadialMenuEntry rmComponent = entry.GetComponent<RadialMenuEntry>();
        rmComponent.SetIcon(icon);
        rmComponent.SetAmmoCount(ammoCount);
        rmComponent.SetOnClickCallback(() => OnClickScoop());
        // rmComponent.CreateOnClickCallback(m_IceCreamInventory, index);

        m_Entries.Add(rmComponent);
    }

    void RearrangeEntries()
    {
        var radiansOfSeparation = (Mathf.PI  * 2) / m_Entries.Count;

        for (int i = 0; i < m_Entries.Count; i++)
        {
            var x  = Mathf.Sin(radiansOfSeparation * i) * m_MenuRadius;
            var y = Mathf.Cos(radiansOfSeparation * i) * m_MenuRadius;

            m_Entries[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
        }

    }
}
