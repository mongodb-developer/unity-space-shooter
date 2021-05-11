using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public float enemyTimer = 2.0f;
    public float timeUntilEnemy = 1.0f;

    void Update() {
        timeUntilEnemy -= Time.deltaTime;
        if(timeUntilEnemy <= 0) {
            GameObject enemy = ObjectPool.SharedInstance.GetPooledEnemy();
            if(enemy != null) {
                enemy.SetActive(true);
            }
            timeUntilEnemy = enemyTimer;
        }
    }
}