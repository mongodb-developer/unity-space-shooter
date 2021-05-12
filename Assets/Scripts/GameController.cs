using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using MongoDB.Bson;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    static public int Score;

    public string RealmAppId;

    public float enemyTimer = 2.0f;
    public float timeUntilEnemy = 1.0f;
    public float minTimeUntilEnemy = 0.25f;
    public float maxTimeUntilEnemy = 2.0f;

    public GameObject SparkBlasterGraphic;
    public GameObject CrossBlasterGraphic;

    public Text highScoreText;
    public Text scoreText;

    private Realm _realm;
    private User _realmUser;
    private PlayerProfile _playerProfile;

    async void OnEnable() {
        //_realmUser = await (App.Create(RealmAppId)).LogInAsync(Credentials.Anonymous());
        _realmUser = await (App.Create(RealmAppId)).LogInAsync(Credentials.EmailPassword("nic.raboy@mongodb.com", "password1234"));
        Debug.Log("Logged in with user " + _realmUser.Id);
        _realm = await Realm.GetInstanceAsync(new SyncConfiguration("game", _realmUser));
        //_playerProfile = _realm.Find<PlayerProfile>(ObjectId.Parse(_realmUser.Id));
        _playerProfile = _realm.Find<PlayerProfile>(_realmUser.Id);
        if(_playerProfile == null) {
            _realm.Write(() => {
                _playerProfile = _realm.Add(new PlayerProfile(_realmUser.Id));
            });
        }
        highScoreText.text = "HIGH SCORE: " + _playerProfile.HighScore.ToString();
        scoreText.text = "SCORE: " + Score.ToString();
    }

    void OnDisable() {
        if(_realm != null) {
            _realm.Dispose();
        }
    }

    void Update() {
        scoreText.text = "SCORE: " + Score.ToString();
        timeUntilEnemy -= Time.deltaTime;
        if(timeUntilEnemy <= 0) {
            GameObject enemy = ObjectPool.SharedInstance.GetPooledEnemy();
            if(enemy != null) {
                enemy.SetActive(true);
            }
            timeUntilEnemy = Random.Range(minTimeUntilEnemy, maxTimeUntilEnemy);
        }
        SparkBlasterGraphic.SetActive(_playerProfile.SparkBlasterEnabled);
        CrossBlasterGraphic.SetActive(_playerProfile.CrossBlasterEnabled);
    }

    public bool IsSparkBlasterEnabled() {
        return _playerProfile.SparkBlasterEnabled;
    }

    public bool IsCrossBlasterEnabled() {
        return _playerProfile.CrossBlasterEnabled;
    }

    public void ResetScore() {
        if(Score > _playerProfile.HighScore) {
            _realm.Write(() => {
                _playerProfile.HighScore = Score;
            });
        }
        highScoreText.text = "HIGH SCORE: " + _playerProfile.HighScore.ToString();
        Score = 0;
    }

}