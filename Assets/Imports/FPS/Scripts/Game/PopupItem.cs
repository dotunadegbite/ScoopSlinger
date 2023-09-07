using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PopupInfo
{
    public Sprite Image {get; set;}
    public string Title { get; set; }
    public string Text { get; set; }
}

public class PopupItem : MonoBehaviour
{
    public Sprite PopupImage;
    public string PopupTitle;
    public string PopupText;

    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var info = new PopupInfo
            {
                Image = PopupImage,
                Title = PopupTitle,
                Text = PopupText
            };

            var playerHandler = other.gameObject.GetComponent<PlayerPopupHandler>();
            if (playerHandler)
            {
                playerHandler.TriggerPopup(info);
            }

            Destroy(gameObject);
        }
    }
}
