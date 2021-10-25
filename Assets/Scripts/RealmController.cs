using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using Realms.Sync.Exceptions;
using System.Threading.Tasks;
using System;

public class RealmController : MonoBehaviour {

    public static RealmController Instance;

    public string RealmAppId = "space-shooter-ubpkp";

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;

    void Awake() {
        if(Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
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
                _realmUser = await _realmApp.LogInAsync(Credentials.EmailPassword(email, password));
                _realm = await Realm.GetInstanceAsync(new SyncConfiguration(email, _realmUser));
            } catch (ClientResetException clientResetEx) {
                if(_realm != null) {
                    _realm.Dispose();
                }
                clientResetEx.InitiateClientReset();
            } catch (Exception ex) {
                return ex.Message;
            }
            return "";
        }
        return "All fields are required!";
    }

    public async Task<string> Register(string name, string email, string password) {
        if(name != "" && email != "" && password != "") { 
            try {
                _realmApp = App.Create(new AppConfiguration(RealmAppId) {
                    MetadataPersistenceMode = MetadataPersistenceMode.NotEncrypted
                });
                await _realmApp.EmailPasswordAuth.RegisterUserAsync(email, password);
                await Login(email, password);
                CreatePlayerProfile(_realmUser.Id, name, email);
            } catch (ClientResetException clientResetEx) {
                if(_realm != null) {
                    _realm.Dispose();
                }
                clientResetEx.InitiateClientReset();
            } catch (Exception ex) {
                return ex.Message;
            }
            return "";
        }
        return "All fields must be present!";
    }

    public PlayerProfile CreatePlayerProfile(string id, string name, string email) {
        PlayerProfile _playerProfile = _realm.Find<PlayerProfile>(id);
        if(_playerProfile == null) {
            _realm.Write(() => {
                _playerProfile = _realm.Add(new PlayerProfile(id, name, email));
            });
        }
        return _playerProfile;
    }

    public PlayerProfile GetPlayerProfile() {
        PlayerProfile _playerProfile = _realm.Find<PlayerProfile>(_realmUser.Id);
        if(_playerProfile == null) {
            _realm.Write(() => {
                _playerProfile = _realm.Add(new PlayerProfile(_realmUser.Id, _realmUser.Profile.Email));
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
