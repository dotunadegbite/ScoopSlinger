using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleObjectVisibility : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToToggle;
    // [SerializeField] private bool ResetSelectionAfterClick;

    private bool _isObjectActive = false;

    public bool IsObjectActive
    {
        get => _isObjectActive;
        private set
        {
            _isObjectActive = value;
        }

    }


        /* void Update()
        {
            if (ObjectToToggle.activeSelf && Input.GetKeyDown(KeyCode.Mouse0) && IsOneClickToggle)
            {
                SetGameObjectActive(false);
            }
        }*/

    public void ShowObject()
    {
        IsObjectActive = true;
        ObjectToToggle.SetActive(IsObjectActive);
    }

    public void HideObject()
    {
        IsObjectActive = false;
        ObjectToToggle.SetActive(IsObjectActive);
    }
}
