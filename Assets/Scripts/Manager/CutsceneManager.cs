using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public Image CutsceneImage;
    public GameObject CutscenePanel;
    public Sprite[] CutsceneImages;

    public Button NextSceneButton;
    public Button SkipSceneButton;

    int m_CurrentIndex = 0;
    AsyncOperation m_LoadLevelOperation;
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
        StartCoroutine(LoadLevelAsync());
    }

    public void HideCutscene()
    {
        m_LoadLevelOperation.allowSceneActivation = true;
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
            var skipText = SkipSceneButton.GetComponentInChildren<TMP_Text>();
            if (skipText != null)
            {
                skipText.text = "Start";
            }

            NextSceneButton.interactable = false;
        }
    }

    private IEnumerator LoadLevelAsync()
    {
        yield return null;

        m_LoadLevelOperation = null;
        //Begin to load the Scene you specify
        m_LoadLevelOperation = SceneManager.LoadSceneAsync("LevelRefactor");
        //Don't let the Scene activate until you allow it to
        m_LoadLevelOperation.allowSceneActivation = false;

        CutscenePanel.SetActive(true);

        //When the load is still in progress, output the Text and progress bar
        while (!m_LoadLevelOperation.isDone)
        {
            // Check if the load has finished
            if (m_LoadLevelOperation.progress >= 0.9f)
            {
                // StartCoroutine(EnableSkipCutsceneButton());
                yield return new WaitForSeconds(2);
                SkipSceneButton.interactable = true;
            }

            yield return null;
        }

        ResetCutscene();
        CutscenePanel.SetActive(false);
    }

    private IEnumerator EnableSkipCutsceneButton()
    {
        Debug.Log("coroutine started");
        yield return new WaitForSeconds(2);
        SkipSceneButton.interactable = true;
    }


    void ResetCutscene()
    {
        m_CurrentIndex = 0;
        CutsceneImage.sprite = CutsceneImages[m_CurrentIndex];
        NextSceneButton.interactable = true;
    }
}
