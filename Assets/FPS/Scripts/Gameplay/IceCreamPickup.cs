//using Unity.FPS.Game;
//using UnityEngine;

//namespace Unity.FPS.Gameplay
//{
//    public class IceCreamPickup : Pickup
//    {
//        //[Header("Parameters")]
//        //[Tooltip("Amount of ice cream gained on pickup")]
//        public float IceCreamAmount;

//        protected override void OnPicked(PlayerCharacterController player)
//        {
//            AmmoPickup playerHealth = player.GetComponent<Health>();
//            if (playerHealth && playerHealth.CanPickup())
//            {
//                playerHealth.Heal(IceCreamAmount);
//                PlayPickupFeedback();
//                Destroy(gameObject);
//            }
//        }
//    }
//}