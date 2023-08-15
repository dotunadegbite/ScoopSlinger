using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

    public class CrosshairManager : MonoBehaviour
    {
        public Image CrosshairImage;
        public Sprite NullCrosshairSprite;
        public float CrosshairUpdateshrpness = 5f;

        // PlayerWeaponsManager m_WeaponsManager;
        PlayerScoopManager m_ScoopManager;
        IceCreamScoopController m_ScoopController;

        bool m_WasPointingAtEnemy;
        RectTransform m_CrosshairRectTransform;
        CrosshairData m_CrosshairDataDefault;
        CrosshairData m_CrosshairDataTarget;
        CrosshairData m_CurrentCrosshair;

        void Start()
        {
            m_ScoopManager = GameObject.FindObjectOfType<PlayerScoopManager>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerScoopManager, CrosshairManager>(m_ScoopManager, this);

            m_ScoopController = m_ScoopManager.GetWeapon();
            // InitializeCrossheir();
        }

        void Update()
        {
            /* UpdateCrosshairPointingAtEnemy(false);
            m_WasPointingAtEnemy = m_ScoopManager.IsPointingAtEnemy; */
        }

        void UpdateCrosshairPointingAtEnemy(bool force)
        {
            if (m_CrosshairDataDefault.CrosshairSprite == null)
                return;

            if ((force || !m_WasPointingAtEnemy) && m_ScoopManager.IsPointingAtEnemy)
            {
                m_CurrentCrosshair = m_CrosshairDataTarget;
                CrosshairImage.sprite = m_CurrentCrosshair.CrosshairSprite;
                m_CrosshairRectTransform.sizeDelta = m_CurrentCrosshair.CrosshairSize * Vector2.one;
            }
            else if ((force || m_WasPointingAtEnemy) && !m_ScoopManager.IsPointingAtEnemy)
            {
                m_CurrentCrosshair = m_CrosshairDataDefault;
                CrosshairImage.sprite = m_CurrentCrosshair.CrosshairSprite;
                m_CrosshairRectTransform.sizeDelta = m_CurrentCrosshair.CrosshairSize * Vector2.one;
            }

            CrosshairImage.color = Color.Lerp(CrosshairImage.color, m_CurrentCrosshair.CrosshairColor,
                Time.deltaTime * CrosshairUpdateshrpness);

            m_CrosshairRectTransform.sizeDelta = Mathf.Lerp(m_CrosshairRectTransform.sizeDelta.x,
                m_CurrentCrosshair.CrosshairSize,
                Time.deltaTime * CrosshairUpdateshrpness) * Vector2.one;
        }

        void InitializeCrossheir()
        {
            CrosshairImage.enabled = true;
            m_CrosshairDataDefault = m_ScoopController.CrosshairDataDefault;
            m_CrosshairDataTarget = m_ScoopController.CrosshairDataTargetInSight;
            m_CrosshairRectTransform = CrosshairImage.GetComponent<RectTransform>();
            DebugUtility.HandleErrorIfNullGetComponent<RectTransform, CrosshairManager>(m_CrosshairRectTransform,
                this, CrosshairImage.gameObject);
        }
    }