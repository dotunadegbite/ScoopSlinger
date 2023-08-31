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
        m_PlayerHealth.InitHealth += DrawHearts;
        m_PlayerHealth.OnHealed += UpdateHearts;
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

        m_PlayerHearts.Clear(); //clears out the heart array
    }

    public void CreateEmptyHeart()
    {
        var newHeart = Instantiate(HeartPrefab); //creates new heart prefab and stores in newheart
        newHeart.transform.SetParent(transform); //sets new heart prefab parent to the HealthHUD prefab

        var heartComponent = newHeart.GetComponent<HeartHealth>(); //heart component takes in heart health script
        heartComponent.SetHeartImage(/* isAlive */ false);
        m_PlayerHearts.Add(heartComponent); //sets to full or empty heart depending on whether that heart is alive or not
    }

    public void DrawHearts()
    {
        ClearHearts(); // clears heart array
        var heartsToMake = m_PlayerHealth.MaxHealth; // makes as many hearts as there is max health
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < m_PlayerHearts.Count; i++)
        {
            var isHeartActive = i + 1 <= m_PlayerHealth.CurrentHealth; // checks whether this determinate spot to reference in the array is less than the current health of the player
            m_PlayerHearts[i].SetHeartImage(/* isAlive */ isHeartActive); // sets the heart image at that point in the array to full or empty depending on the previous statement
        }
    }

    void UpdateHearts(float damage, GameObject damageSource)
    {
        DrawHearts(); //clears hearts and adds them back
    }
}