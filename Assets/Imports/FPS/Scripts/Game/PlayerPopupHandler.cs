using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPopupHandler : MonoBehaviour
{
    public UnityAction<PopupInfo> OnPlayerTriggerPopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerPopup(PopupInfo info)
    {
        Debug.Log("Trigger popup with title: " + info.Title);
        OnPlayerTriggerPopup?.Invoke(info);
    }
}
