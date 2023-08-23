using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private string SceneName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToScene()
    {
        Debug.Log("transition 1");
        SceneManager.LoadScene(SceneName);
    }

    //Added for a quick way to transition scenes on win
    public void MoveToScene(string SceneName)
    {
        Debug.Log("transition 1");
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
