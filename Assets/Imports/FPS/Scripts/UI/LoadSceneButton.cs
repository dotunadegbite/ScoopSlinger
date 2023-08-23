using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Unity.FPS.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        public string SceneName = "";

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject
                && Input.GetButtonDown(GameConstants.k_ButtonNameSubmit))
            {
                Debug.Log("transition button pressed");
                LoadTargetScene();
            }
        }

        public void LoadTargetScene()
        {
            Debug.Log("transition in load scene button triggered");
            SceneManager.LoadScene(SceneName);
        }
    }
}