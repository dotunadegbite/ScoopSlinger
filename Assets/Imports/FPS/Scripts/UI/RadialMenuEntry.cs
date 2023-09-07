using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RadialMenuEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void RadialMenuEntryDelegate();
    
   [SerializeField] TMP_Text m_AmmoCount;

   [SerializeField] Image m_Icon;

   [SerializeField] Image m_Backdrop;

   [SerializeField] Color m_BaseColor;
   [SerializeField] Color m_SelectedColor;

   RadialMenuEntryDelegate m_OnClickCallback;

   public void SetAmmoCount(int count)
   {
        m_AmmoCount.text = count < 10 ? "0" + count : count.ToString();
   }

   public void SetOnClickCallback(RadialMenuEntryDelegate entryDelegate)
   {
        m_OnClickCallback = entryDelegate;
   }

   public void CreateOnClickCallback(IceCreamInventoryManager inventory, int index)
   {
        m_OnClickCallback = (() => inventory.SwitchScoopToIndex(index));
   }

   public void OnPointerClick(PointerEventData eventData)
   {
        m_OnClickCallback?.Invoke();
   }
   
   public void OnPointerEnter(PointerEventData eventData)
   {
        m_Backdrop.color = m_SelectedColor;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
        m_Backdrop.color = m_BaseColor;
   }

   public void SetIcon(Sprite icon)
   {
        m_Icon.sprite = icon;
   }
}
