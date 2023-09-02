using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScoopHUDManager : MonoBehaviour
{
    [SerializeField] private Image CurrentScoopIcon;
    [SerializeField] private TMP_Text CurrentAmmoText;
    
    [SerializeField] private Sprite[] ScoopImages; 
    IceCreamInventoryManager m_InventoryManager;

    private const string AmmoFormat = "{0} | {1}";
    

    // Start is called before the first frame update
    void Start()
    {
        m_InventoryManager = FindObjectOfType<IceCreamInventoryManager>();
        m_InventoryManager.OnScoopAmmoChangedEvent += UpdateCurrentAmmoUI;
    }

    void UpdateCurrentAmmoUI(object sender, ScoopAmmoChangedEventArgs e)
    {
        var currentFlavor = e.AmmoType;
        var currentAmmo = e.CurrentAmmoCount;
        var maxAmmo = e.MaxAmmoCount;

        // CurrentAmmoText.text = string.Format(AmmoFormat, currentAmmo, maxAmmo);
        CurrentAmmoText.text = currentAmmo < 10 ? "0" + currentAmmo.ToString() : currentAmmo.ToString();
        CurrentScoopIcon.sprite = ScoopImages[(int)currentFlavor];
    }
    
}