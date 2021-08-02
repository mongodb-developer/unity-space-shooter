using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using MongoDB.Bson;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public float enemyTimer = 2.0f;
    public float timeUntilEnemy = 1.0f;
    public float minTimeUntilEnemy = 0.25f;
    public float maxTimeUntilEnemy = 2.0f;

    public GameObject SparkBlasterGraphic;
    public GameObject CrossBlasterGraphic;

    public Text highScoreText;
    public Text scoreText;

    private PlayerProfile _playerProfile;

    void OnEnable() {
        _playerProfile = RealmController.Instance.GetPlayerProfile();
        highScoreText.text = "HIGH SCORE: " + _playerProfile.HighScore.ToString();
        scoreText.text = "SCORE: " + _playerProfile.Score.ToString();
    }

    void Update() {
        highScoreText.text = "HIGH SCORE: " + _playerProfile.HighScore.ToString();
        scoreText.text = "SCORE: " + _playerProfile.Score.ToString();
        timeUntilEnemy -= Time.deltaTime;
        if(timeUntilEnemy <= 0) {
            GameObject enemy = ObjectPool.SharedInstance.GetPooledEnemy();
            if(enemy != null) {
                enemy.SetActive(true);
            }
            timeUntilEnemy = Random.Range(minTimeUntilEnemy, maxTimeUntilEnemy);
        }
        if(_playerProfile != null) {
            SparkBlasterGraphic.SetActive(_playerProfile.SparkBlasterEnabled);
            CrossBlasterGraphic.SetActive(_playerProfile.CrossBlasterEnabled);
        }
        if(Input.GetKey("escape")) {
            Application.Quit();
        }
    }

}