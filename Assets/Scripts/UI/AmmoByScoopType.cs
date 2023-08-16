using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoByScoopType : MonoBehaviour
{
    public TMP_Text currentAmmoChocolate;
    public TMP_Text currentAmmoMint;
    public TMP_Text currentAmmoBerry;
    public TMP_Text currentAmmoVanilla;
    IceCreamInventoryManager m_ScoopUIManager;

    // Start is called before the first frame update
    void Start()
    {
        m_ScoopUIManager = FindObjectOfType<IceCreamInventoryManager>();
        m_ScoopUIManager.OnScoopAmmoChangedEvent += UpdateAmmoCountUI;
        currentAmmoChocolate.text = "10";
        currentAmmoMint.text = "10";
        currentAmmoBerry.text = "10";
        currentAmmoVanilla.text = "10";
    }

    void UpdateAmmoCountUI(object sender, ScoopAmmoChangedEventArgs e)
    {
        if(e.AmmoType == FlavorType.CHOCOLATE)
        {
            currentAmmoChocolate.text = e.CurrentAmmoCount.ToString();
        } else if (e.AmmoType == FlavorType.MINT)
        {
            currentAmmoMint.text = e.CurrentAmmoCount.ToString();
        } else if (e.AmmoType == FlavorType.BERRY)
        {
            currentAmmoBerry.text = e.CurrentAmmoCount.ToString();
        } else if (e.AmmoType == FlavorType.VANILLA)
        {
            currentAmmoVanilla.text = e.CurrentAmmoCount.ToString();
        }
    }
}
