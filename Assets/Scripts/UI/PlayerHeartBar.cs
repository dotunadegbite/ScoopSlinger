using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class PlayerHeartBar : MonoBehaviour
    {
        [Tooltip("Image component dispplaying current health")]
        public Image HealthFillImage;

        public Sprite FullHeartSprite;
        public Sprite EmptyHeartSprite;

        public GameObject HeartPrefab;


        Health m_PlayerHealth;
        List<HeartHealth> m_PlayerHearts = new List<HeartHealth>();

        void Start()
        {
            PlayerCharacterController playerCharacterController =
                GameObject.FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, PlayerHeartBar>(
                playerCharacterController, this);

            m_PlayerHealth = playerCharacterController.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerHeartBar>(m_PlayerHealth, this,
                playerCharacterController.gameObject);

            
            m_PlayerHealth.OnDamaged += UpdateHearts;
            m_PlayerHealth.InitHealth += OnPlayerHealthInit;
        }

        void Update()
        {
        }

        public void ClearHearts()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }

            m_PlayerHearts.Clear();
        }

        public void CreateEmptyHeart()
        {
            var newHeart = Instantiate(HeartPrefab);
            newHeart.transform.SetParent(transform);

            var heartComponent = newHeart.GetComponent<HeartHealth>();
            heartComponent.SetHeartImage(/* isAlive */ false);
            m_PlayerHearts.Add(heartComponent);
        }

        public void DrawHearts()
        {
            ClearHearts();
            var heartsToMake = m_PlayerHealth.MaxHealth;
            for (int i = 0; i < heartsToMake; i++)
            {
                CreateEmptyHeart();
            }

            for (int i = 0; i < m_PlayerHearts.Count; i++)
            {
                var isHeartActive = i + 1 <= m_PlayerHealth.CurrentHealth;
                m_PlayerHearts[i].SetHeartImage(/* isAlive */ isHeartActive);
            }
        }

        private void OnPlayerHealthInit()
        {
            DrawHearts();
        }

        void UpdateHearts(float damage, GameObject damageSource)
        {
            DrawHearts();
        }
    }