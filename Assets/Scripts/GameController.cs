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
        var realmApp = App.Create(new AppConfiguration(RealmAppId) {
            MetadataPersistenceMode = MetadataPersistenceMode.NotEncrypted
        });
        _realmUser = realmApp.CurrentUser;
        if (_realmUser == null) {
            _realmUser = await realmApp.LogInAsync(Credentials.EmailPassword("nic.raboy@mongodb.com", "password1234"));
            _realm = await Realm.GetInstanceAsync(new SyncConfiguration("game", _realmUser));
        } else {
            _realm = Realm.GetInstance(new SyncConfiguration("game", _realmUser));
        }
        Debug.Log("Logged in with user " + _realmUser.Id);
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
        if(_playerProfile != null) {
            SparkBlasterGraphic.SetActive(_playerProfile.SparkBlasterEnabled);
            CrossBlasterGraphic.SetActive(_playerProfile.CrossBlasterEnabled);
        }
    }

    public bool IsSparkBlasterEnabled() {
        return _playerProfile != null ? _playerProfile.SparkBlasterEnabled : false;
    }

    public bool IsCrossBlasterEnabled() {
        return _playerProfile != null ? _playerProfile.CrossBlasterEnabled : false;
    }

    public void ResetScore() {
        if(_playerProfile != null) {
            if(Score > _playerProfile.HighScore) {
                _realm.Write(() => {
                    _playerProfile.HighScore = Score;
                });
            }
            highScoreText.text = "HIGH SCORE: " + _playerProfile.HighScore.ToString();
        }
        Score = 0;
    }

}