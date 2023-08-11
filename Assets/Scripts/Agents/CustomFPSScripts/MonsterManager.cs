using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public List<MonsterController> Enemies { get; private set; }
    public int NumberOfEnemiesTotal { get; private set; }
    public int NumberOfEnemiesRemaining => Enemies.Count;

    void Awake()
    {
        Enemies = new List<MonsterController>();
    }

    public void RegisterEnemy(MonsterController enemy)
    {
        Enemies.Add(enemy);

        NumberOfEnemiesTotal++;
    }

    public void UnregisterEnemy(MonsterController enemyKilled)
    {
        int enemiesRemainingNotification = NumberOfEnemiesRemaining - 1;

        EnemyKillEvent evt = Events.EnemyKillEvent;
        evt.Enemy = enemyKilled.gameObject;
        evt.RemainingEnemyCount = enemiesRemainingNotification;
        EventManager.Broadcast(evt);

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        Enemies.Remove(enemyKilled);
    }
}
