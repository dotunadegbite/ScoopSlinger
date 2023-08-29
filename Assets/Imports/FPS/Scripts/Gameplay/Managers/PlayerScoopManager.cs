using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerScoopManager : MonoBehaviour
{
    [Header("References")] [Tooltip("Secondary camera used to avoid seeing weapon go throw geometries")]
    public Camera WeaponCamera;

    [Tooltip("Parent transform where all weapon will be added in the hierarchy")]
    public Transform WeaponParentSocket;

    [Tooltip("Position for weapons when active but not actively aiming")]
    public Transform DefaultWeaponPosition;

    [Tooltip("Position for weapons when aiming")]
    public Transform AimingWeaponPosition;

    [Tooltip("Position for innactive weapons")]
    public Transform DownWeaponPosition;

    [Header("Weapon Bob")]
    [Tooltip("Frequency at which the weapon will move around in the screen when the player is in movement")]
    public float BobFrequency = 10f;

    [Tooltip("How fast the weapon bob is applied, the bigger value the fastest")]
    public float BobSharpness = 10f;

    [Tooltip("Distance the weapon bobs when not aiming")]
    public float DefaultBobAmount = 0.05f;

    [Tooltip("Distance the weapon bobs when aiming")]
    public float AimingBobAmount = 0.02f;

    [Header("Misc")] [Tooltip("Speed at which the aiming animatoin is played")]
    public float AimingAnimationSpeed = 10f;

    [Tooltip("Field of view when not aiming")]
    public float DefaultFov = 60f;

    [Tooltip("Portion of the regular FOV to apply to the weapon camera")]
    public float WeaponFovMultiplier = 1f;

    [Tooltip("Delay before switching weapon a second time, to avoid recieving multiple inputs from mouse wheel")]
    public float WeaponSwitchDelay = 1f;

    [Tooltip("Layer to set FPS weapon gameObjects to")]
    public LayerMask FpsWeaponLayer;

    public bool IsAiming { get; private set; }
    public bool IsPointingAtEnemy { get; private set; }

    [SerializeField] private IceCreamScoopController scoopPrefabObject;
    

    
    PlayerInputHandler m_InputHandler;
    PlayerCharacterController m_PlayerCharacterController;
    float m_WeaponBobFactor;
    Vector3 m_LastCharacterPosition;
    Vector3 m_WeaponMainLocalPosition;
    Vector3 m_WeaponBobLocalPosition;

    IceCreamScoopController scoopController;
    IceCreamInventoryManager m_ScoopInventoryManager;

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, PlayerWeaponsManager>(m_InputHandler, this,
            gameObject);

        m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, PlayerWeaponsManager>(
            m_PlayerCharacterController, this, gameObject);
        
        m_ScoopInventoryManager = GetComponent<IceCreamInventoryManager>();

        // m_ScoopInventoryManager.OnScoopAmmoChangedEvent += PrintOutAmmo;

        SetFov(DefaultFov);
        InitalizeScoop();
    }

    void Update()
    {
        if (scoopController != null)
        {
            IsAiming = m_InputHandler.GetAimInputHeld();

            // handle shooting
            bool hasFired = scoopController.HandleShootInputs(
                m_InputHandler.GetFireInputDown(), m_ScoopInventoryManager);

        }

        // weapon switch handling
        if (!IsAiming)
        {
            int switchWeaponInput = m_InputHandler.GetSwitchWeaponInput();
            if (switchWeaponInput != 0)
            {
                bool switchUp = switchWeaponInput > 0;
                m_ScoopInventoryManager.SwitchScoop(switchUp);
            }
            else
            {
                switchWeaponInput = m_InputHandler.GetSelectWeaponInput();
                if (switchWeaponInput != 0)
                {
                    m_ScoopInventoryManager.SwitchScoopToIndex(switchWeaponInput);
                }
            }
        }

        // Pointing at enemy handling
        IsPointingAtEnemy = false;
        if (scoopController)
        {
            if (Physics.Raycast(WeaponCamera.transform.position, WeaponCamera.transform.forward, out RaycastHit hit,
                1000, -1, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.GetComponentInParent<Health>() != null)
                {
                    IsPointingAtEnemy = true;
                }
            }
        }
    }


    // Update various animated features in LateUpdate because it needs to override the animated arm position
    void LateUpdate()
    {
        UpdateWeaponAiming();
        UpdateWeaponBob();

        // Set final weapon socket position based on all the combined animation influences
        WeaponParentSocket.localPosition = m_WeaponMainLocalPosition + m_WeaponBobLocalPosition;
    }

    // Sets the FOV of the main camera and the weapon camera simultaneously
    public void SetFov(float fov)
    {
        m_PlayerCharacterController.PlayerCamera.fieldOfView = fov;
        WeaponCamera.fieldOfView = fov * WeaponFovMultiplier;
    }

    public IceCreamScoopController GetWeapon()
    {
        return scoopController;
    }

    // Updates weapon position and camera FoV for the aiming transition
    void UpdateWeaponAiming()
    {
        if (IsAiming && scoopController)
        {
            m_WeaponMainLocalPosition = Vector3.Lerp(m_WeaponMainLocalPosition,
                AimingWeaponPosition.localPosition + scoopController.AimOffset,
                AimingAnimationSpeed * Time.deltaTime);
            SetFov(Mathf.Lerp(m_PlayerCharacterController.PlayerCamera.fieldOfView,
                scoopController.AimZoomRatio * DefaultFov, AimingAnimationSpeed * Time.deltaTime));
        }
        else
        {
            m_WeaponMainLocalPosition = Vector3.Lerp(m_WeaponMainLocalPosition,
                DefaultWeaponPosition.localPosition, AimingAnimationSpeed * Time.deltaTime);
            SetFov(Mathf.Lerp(m_PlayerCharacterController.PlayerCamera.fieldOfView, DefaultFov,
                AimingAnimationSpeed * Time.deltaTime));
        }
    }

    // Updates the weapon bob animation based on character speed
    void UpdateWeaponBob()
    {
        if (Time.deltaTime > 0f)
        {
            Vector3 playerCharacterVelocity =
                (m_PlayerCharacterController.transform.position - m_LastCharacterPosition) / Time.deltaTime;

            // calculate a smoothed weapon bob amount based on how close to our max grounded movement velocity we are
            float characterMovementFactor = 0f;
            if (m_PlayerCharacterController.IsGrounded)
            {
                characterMovementFactor =
                    Mathf.Clamp01(playerCharacterVelocity.magnitude /
                                    (m_PlayerCharacterController.MaxSpeedOnGround *
                                    m_PlayerCharacterController.SprintSpeedModifier));
            }

            m_WeaponBobFactor =
                Mathf.Lerp(m_WeaponBobFactor, characterMovementFactor, BobSharpness * Time.deltaTime);

            // Calculate vertical and horizontal weapon bob values based on a sine function
            float bobAmount = IsAiming ? AimingBobAmount : DefaultBobAmount;
            float frequency = BobFrequency;
            float hBobValue = Mathf.Sin(Time.time * frequency) * bobAmount * m_WeaponBobFactor;
            float vBobValue = ((Mathf.Sin(Time.time * frequency * 2f) * 0.5f) + 0.5f) * bobAmount *
                                m_WeaponBobFactor;

            // Apply weapon bob
            m_WeaponBobLocalPosition.x = hBobValue;
            m_WeaponBobLocalPosition.y = Mathf.Abs(vBobValue);

            m_LastCharacterPosition = m_PlayerCharacterController.transform.position;
        }
    }

    void InitalizeScoop()
    {
        // spawn the weapon prefab as child of the weapon socket
        IceCreamScoopController weaponInstance = Instantiate(scoopPrefabObject, WeaponParentSocket);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly
        weaponInstance.Owner = gameObject;
        weaponInstance.SourcePrefab = scoopPrefabObject.gameObject;
        weaponInstance.ShowWeapon(true);

        scoopController = weaponInstance;

        // Assign the first person layer to the weapon
        int layerIndex =
            Mathf.RoundToInt(Mathf.Log(FpsWeaponLayer.value,
                2)); // This function converts a layermask to a layer index
        foreach (Transform t in weaponInstance.gameObject.GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = layerIndex;
        }

        scoopController.OnShoot += m_ScoopInventoryManager.UpdateAmmo;
    }

    void PrintOutAmmo(object sender, ScoopAmmoChangedEventArgs e)
    {
        Debug.Log("Scoop Type: " + e.AmmoType + " now has ammo count: " + e.CurrentAmmoCount + " out of " + e.MaxAmmoCount);
    }
}