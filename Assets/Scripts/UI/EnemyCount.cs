using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCount : MonoBehaviour
{
    // Start is called before the first frame update
    private int numEnemies;
    public Text enemyCountText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyCountText.text = "Enemies Remaining: " + numEnemies;
    }

    
}
