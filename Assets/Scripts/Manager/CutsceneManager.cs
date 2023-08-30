using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Image CutsceneImage;
    public GameObject CutscenePanel;
    public Sprite[] CutsceneImages;

    public Button NextSceneButton;

    int m_CurrentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeCutscene()
    {
        ResetCutscene();
        CutscenePanel.SetActive(true);
    }

    public void HideCutscene()
    {
        ResetCutscene();
        CutscenePanel.SetActive(false);
    }

    public void UpdateCutscene()
    {
        m_CurrentIndex++;
        if (m_CurrentIndex >= CutsceneImages.Length)
        {
            m_CurrentIndex = 0;
        }

        CutsceneImage.sprite = CutsceneImages[m_CurrentIndex];

        if (m_CurrentIndex >= CutsceneImages.Length - 1)
        {
            NextSceneButton.interactable = false;
        }
    }


    void ResetCutscene()
    {
        m_CurrentIndex = 0;
        CutsceneImage.sprite = CutsceneImages[m_CurrentIndex];
        NextSceneButton.interactable = true;
    }
}
