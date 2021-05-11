using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;

public class GameController : MonoBehaviour {

    public string RealmAppId;

    public float enemyTimer = 2.0f;
    public float timeUntilEnemy = 1.0f;
    public float minTimeUntilEnemy = 0.25f;
    public float maxTimeUntilEnemy = 2.0f;

    private Realm _realm;
    private User _realmUser;
    private PlayerProfile _playerProfile;

    async void OnEnable() {
        // App realmApp = App.Create(RealmAppId);
        // _realmUser = await realmApp.LogInAsync(Credentials.Anonymous());
        _realmUser = await (App.Create(RealmAppId)).LogInAsync(Credentials.Anonymous());
        Debug.Log("Logged in with user " + _realmUser.Id);
        _realm = await Realm.GetInstanceAsync(new SyncConfiguration("game", _realmUser));
        _playerProfile = _realm.Find<PlayerProfile>(_realmUser.Id);
        if(_playerProfile == null) {
            _realm.Write(() => {
                _playerProfile = _realm.Add(new PlayerProfile(_realmUser.Id));
            });
        }
    }

    void OnDisable() {
        if(_realm != null) {
            _realm.Dispose();
        }
    }

    void Update() {
        timeUntilEnemy -= Time.deltaTime;
        if(timeUntilEnemy <= 0) {
            GameObject enemy = ObjectPool.SharedInstance.GetPooledEnemy();
            if(enemy != null) {
                enemy.SetActive(true);
            }
            timeUntilEnemy = Random.Range(minTimeUntilEnemy, maxTimeUntilEnemy);
        }
    }
}