using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        [Tooltip("Root GameObject of the menu used to toggle its activation")]
        public GameObject MenuRoot;

        [Tooltip("Master volume when menu is open")] [Range(0.001f, 1f)]
        public float VolumeWhenMenuOpen = 0.5f;

        [Tooltip("Slider component for look sensitivity")]
        public Slider LookSensitivitySlider;

        public GameObject AmmoMenuRoot;

        public GameObject PopupRoot;
        public bool IsAmmoMenuOpen { get; private set;}

        PlayerInputHandler m_PlayerInputsHandler;
        Health m_PlayerHealth;
        FramerateCounter m_FramerateCounter;

        PlayerPopupHandler m_PlayerPopupHandler;

        void Start()
        {
            m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler,
                this);

            m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

            m_PlayerPopupHandler = m_PlayerInputsHandler.GetComponent<PlayerPopupHandler>();

            m_PlayerPopupHandler.OnPlayerTriggerPopup += ActivatePopup;

            MenuRoot.SetActive(false);
            AmmoMenuRoot.SetActive(false);
            PopupRoot.SetActive(false);

            LookSensitivitySlider.value = m_PlayerInputsHandler.LookSensitivity;
            LookSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        }

        void Update()
        {
            // Lock cursor when clicking outside of menu
            if ((!MenuRoot.activeSelf && !AmmoMenuRoot.activeSelf && !PopupRoot.activeSelf) && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if ((Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu) && !AmmoMenuRoot.activeSelf)
                || (MenuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
            {
                SetPauseMenuActivation(!MenuRoot.activeSelf);
            } 

            if (m_PlayerInputsHandler.GetAmmoMenuInputDown()
                || (AmmoMenuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
            {
                if (!MenuRoot.activeSelf)
                {
                    SetAmmoMenuActivation(true);
                }
            }

            if (m_PlayerInputsHandler.GetAmmoMenuInputReleased())
            {
                if (!MenuRoot.activeSelf)
                {
                    SetAmmoMenuActivation(false);
                }
            }

            if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    LookSensitivitySlider.Select();
                }
            }
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }

        public void ClosePauseMenu()
        {
            SetPauseMenuActivation(false);
        }

        public void ClosePopup()
        {
            PopupRoot.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            AudioUtility.SetMasterVolume(1);

        }

        void SetPauseMenuActivation(bool active)
        {
            if (active && (AmmoMenuRoot.activeSelf || PopupRoot.activeSelf))
            {
                return;
            }

            MenuRoot.SetActive(active);

            if (MenuRoot.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);

                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                AudioUtility.SetMasterVolume(1);
            }

        }

        void SetAmmoMenuActivation(bool active)
        {
            if ((MenuRoot.activeSelf || PopupRoot.activeSelf))
            {
                return;
            }
            AmmoMenuRoot.SetActive(active);

            if (AmmoMenuRoot.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0.15f;
                var radialMenu = GetComponent<RadialMenu>();
                if (radialMenu)
                {
                    radialMenu.Open();
                    radialMenu.SetCloseMenuAction(SetAmmoMenuActivation);
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1.0f;
                var radialMenu = GetComponent<RadialMenu>();
                if (radialMenu)
                {
                    radialMenu.Close();
                }
            }
        }

        void ActivatePopup(PopupInfo info)
        {
            Debug.Log("Activate popup with title: " + info.Title);
            if (!PopupRoot.activeSelf)
            {
                var popupManager = GetComponent<PopupManager>();
                if (popupManager)
                {
                    popupManager.InstatiatePopup(info);
                    PopupRoot.SetActive(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Time.timeScale = 0f;
                    AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);
                    EventSystem.current.SetSelectedGameObject(null);
                }

            }
        }


        void OnMouseSensitivityChanged(float newValue)
        {
            m_PlayerInputsHandler.LookSensitivity = newValue;
        }

        void OnShadowsChanged(bool newValue)
        {
            QualitySettings.shadows = newValue ? ShadowQuality.All : ShadowQuality.Disable;
        }
    }
}