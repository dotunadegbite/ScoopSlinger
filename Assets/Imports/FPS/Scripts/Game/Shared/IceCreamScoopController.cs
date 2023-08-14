using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct ScoopCrosshairData
{
    [Tooltip("The image that will be used for this weapon's crosshair")]
    public Sprite ScoopCrosshairSprite;

    [Tooltip("The size of the crosshair image")]
    public int ScoopCrosshairSize;

    [Tooltip("The color of the crosshair image")]
    public Color ScoopCrosshairColor;
}
public class IceCreamScoopController : MonoBehaviour
{
    [Tooltip("Default data for the crosshair")]
    public CrosshairData CrosshairDataDefault;

    [Tooltip("Data for the crosshair when targeting an enemy")]
    public CrosshairData CrosshairDataTargetInSight;

    [Header("Internal References")]
    [Tooltip("The root object for the weapon, this is what will be deactivated when the weapon isn't active")]
    public GameObject WeaponRoot;

    [Tooltip("Tip of the weapon, where the projectiles are shot")]
    public Transform WeaponMuzzle;

    [Tooltip("The projectile prefab")] 
    public ProjectileBase ProjectilePrefab;

    [Tooltip("Minimum duration between two shots")]
    public float DelayBetweenShots = 0.5f;

    [Tooltip("Number of bullets in a clip")]
    public int ClipSize = 30;

    [Tooltip("Maximum amount of ammo in the gun")]
    public int MaxAmmo = 15;

    [Tooltip("Translation to apply to weapon arm when aiming with this weapon")]
    public Vector3 AimOffset;

    [Tooltip("Ratio of the default FOV that this weapon applies while aiming")] [Range(0f, 1f)]
    public float AimZoomRatio = 1f;


   [Header("Audio & Visual")]
   [Tooltip("Optional weapon animator for OnShoot animations")]
   public Animator WeaponAnimator;

   [Tooltip("sound played when shooting")]
   public AudioClip ShootSfx;

   [Tooltip("Sound played when changing to this weapon")]
   public AudioClip ChangeWeaponSfx;

   public UnityAction OnShoot;
   public event Action OnShootProcessed;

   public GameObject Owner { get; set; }
   public GameObject SourcePrefab { get; set; }
   public float CurrentAmmoRatio { get; private set; }
   public bool IsWeaponActive { get; private set; }
   public Vector3 MuzzleWorldVelocity { get; private set; }

   public int GetCurrentAmmo() => Mathf.FloorToInt(m_CurrentAmmo);

   float m_CurrentAmmo = 100000000f;
   float m_LastTimeShot = Mathf.NegativeInfinity;
   Vector3 m_LastMuzzlePosition;
   AudioSource m_ShootAudioSource;

   const string k_AnimShootParameter = "ShootTrigger";

   void Awake()
   {
        m_CurrentAmmo = MaxAmmo;
        m_LastMuzzlePosition = WeaponMuzzle.position;
        m_ShootAudioSource = GetComponent<AudioSource>();
   }

   void Update()
   {
        // UpdateAmmo();
        
        if (Time.deltaTime > 0)
        {
            MuzzleWorldVelocity = (WeaponMuzzle.position - m_LastMuzzlePosition) / Time.deltaTime;
            m_LastMuzzlePosition = WeaponMuzzle.position;
        }
   }

   public void ShowWeapon(bool show)
    {
        WeaponRoot.SetActive(show);

        if (show && ChangeWeaponSfx)
        {
            m_ShootAudioSource.PlayOneShot(ChangeWeaponSfx);
        }

        IsWeaponActive = show;
    }

   public void UseAmmo(float amount)
    {
        m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo - amount, 0f, MaxAmmo);
        m_LastTimeShot = Time.time;
    }

    public bool HandleShootInputs(bool inputDown, ProjectileBase currentScoop)
    {
        if (inputDown)
        {
            return TryShoot(currentScoop);
        }
        
        return false;
    }

    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere, 0f);
        return spreadWorldDirection;
    }

    bool TryShoot(ProjectileBase currentScoop)
    {
        if (m_CurrentAmmo >= 1f && m_LastTimeShot + DelayBetweenShots < Time.time)
        {
            
            HandleShoot(currentScoop);
            m_CurrentAmmo -= 1f;

            return true;
        }

        return false;
    }

    void HandleShoot(ProjectileBase currentScoop)
    {
        int bulletsPerShotFinal = 1;
        
        // spawn all bullets with random direction
        for (int i = 0; i < bulletsPerShotFinal; i++)
        {
            Vector3 shotDirection = GetShotDirectionWithinSpread(WeaponMuzzle);
            ProjectileBase newProjectile = Instantiate(currentScoop, WeaponMuzzle.position,
                Quaternion.LookRotation(shotDirection));
            
            newProjectile.Shoot(this);
        }

        m_LastTimeShot = Time.time;

        // play shoot SFX
        if (ShootSfx)
        {
            m_ShootAudioSource.PlayOneShot(ShootSfx);
        }

        // Trigger attack animation if there is any
        if (WeaponAnimator)
        {
            WeaponAnimator.SetTrigger(k_AnimShootParameter);
        }

        OnShoot?.Invoke();
        OnShootProcessed?.Invoke();
    }

    void UpdateAmmo()
    {
        m_CurrentAmmo -= 1;
    }

}
