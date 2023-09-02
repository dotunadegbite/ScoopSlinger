using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public TMP_Text PopupTitle;
    public TMP_Text PopupText;

    public Image PopupImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstatiatePopup(PopupInfo info)
    {
        PopupImage.sprite = info.Image;
        PopupText.text = info.Text;
        PopupTitle.text = info.Title;
    }
}
