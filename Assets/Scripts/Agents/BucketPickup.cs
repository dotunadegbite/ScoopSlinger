using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

//using Unity.FPS.Game;
//using UnityEngine;

//namespace Unity.FPS.Gameplay
//{
//    public class AmmoPickup : Pickup
//    {
//        [Tooltip("Weapon those bullets are for")]
//        public WeaponController Weapon;

//        [Tooltip("Number of bullets the player gets")]
//        public int BulletCount = 30;

//        protected override void OnPicked(PlayerCharacterController byPlayer)
//        {
//            PlayerWeaponsManager playerWeaponsManager = byPlayer.GetComponent<PlayerWeaponsManager>();
//            if (playerWeaponsManager)
//            {
//                WeaponController weapon = playerWeaponsManager.HasWeapon(Weapon);
//                if (weapon != null)
//                {
//                    weapon.AddCarriablePhysicalBullets(BulletCount);

//                    AmmoPickupEvent evt = Events.AmmoPickupEvent;
//                    evt.Weapon = weapon;
//                    EventManager.Broadcast(evt);

//                    PlayPickupFeedback();
//                    Destroy(gameObject);
//                }
//            }
//        }
//    }
//}

