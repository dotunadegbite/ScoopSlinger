using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartHealth : MonoBehaviour
{
    public Sprite fullHeart, emptyHeart;
    Image heartImage;
    // Start is called before the first frame update
    void Awake()
    {
        heartImage = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHeartImage (bool isAlive)
    {
        var heartSprite = isAlive ? fullHeart : emptyHeart;
        heartImage.sprite = heartSprite;
    }
}
