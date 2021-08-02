using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using Realms.Sync.Exceptions;
using System.Threading.Tasks;

public class RealmController : MonoBehaviour {

    public static RealmController Instance;

    public string RealmAppId = "space-shooter-ubpkp";

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    void OnDisable() {
        if(_realm != null) {
            _realm.Dispose();
        }
    }

    public async Task<string> Login(string email, string password) {
        if(email != "" && password != "") {
            _realmApp = App.Create(new AppConfiguration(RealmAppId) {
                MetadataPersistenceMode = MetadataPersistenceMode.NotEncrypted
            });
            try {
                if(_realmUser == null) {
                    _realmUser = await _realmApp.LogInAsync(Credentials.EmailPassword(email, password));
                    _realm = await Realm.GetInstanceAsync(new SyncConfiguration(email, _realmUser));
                } else {
                    _realm = Realm.GetInstance(new SyncConfiguration(email, _realmUser));
                }
            } catch (ClientResetException clientResetEx) {
                if(_realm != null) {
                    _realm.Dispose();
                }
                clientResetEx.InitiateClientReset();
            }
            return _realmUser.Id;
        }
        return "";
    }

    public PlayerProfile GetPlayerProfile() {
        PlayerProfile _playerProfile = _realm.Find<PlayerProfile>(_realmUser.Id);
        if(_playerProfile == null) {
            _realm.Write(() => {
                _playerProfile = _realm.Add(new PlayerProfile(_realmUser.Id));
            });
        }
        return _playerProfile;
    }

    public void IncreaseScore() {
        PlayerProfile _playerProfile = GetPlayerProfile();
        if(_playerProfile != null) {
            _realm.Write(() => {
                _playerProfile.Score++;
            });
        }
    }

    public void ResetScore() {
        PlayerProfile _playerProfile = GetPlayerProfile();
        if(_playerProfile != null) {
            _realm.Write(() => {
                if(_playerProfile.Score > _playerProfile.HighScore) {
                    _playerProfile.HighScore = _playerProfile.Score;
                }
                _playerProfile.Score = 0;
            });
        }
    }

    public bool IsSparkBlasterEnabled() {
        PlayerProfile _playerProfile = GetPlayerProfile();
        return _playerProfile != null ? _playerProfile.SparkBlasterEnabled : false;
    }

    public bool IsCrossBlasterEnabled() {
        PlayerProfile _playerProfile = GetPlayerProfile();
        return _playerProfile != null ? _playerProfile.CrossBlasterEnabled : false;
    }

}
