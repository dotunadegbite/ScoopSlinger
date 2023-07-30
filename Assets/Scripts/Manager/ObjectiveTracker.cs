using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    LoadSceneManager loadScene;
    [SerializeField] List<GameObject> _enemyToDestroy;
    private int numEnemies;
    // Start is called before the first frame update
    void Start()
    {
        loadScene = GameObject.FindGameObjectWithTag("WinManager").GetComponent<LoadSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Quick way to track win condition.
        // Reference pulling from "WinCondition" object in the main scene
        if (GameObject.FindWithTag("Enemy") == null)
        {
            loadScene.MoveToScene("WinScene");
        }
    }
}
