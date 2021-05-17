using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Realms;
using Realms.Sync;
using MongoDB.Bson;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour {

    public Button LoginButton;
    public InputField UsernameInput;
    public InputField PasswordInput;

    public string RealmAppId;
    private Realm _realm;
    private User _realmUser;
    private PlayerProfile _playerProfile;

    void Start() {
        LoginButton.onClick.AddListener(Login);
    }

    async void Login() {
        if(UsernameInput.text != "" && PasswordInput.text != "") {
            var realmApp = App.Create(new AppConfiguration(RealmAppId) {
                MetadataPersistenceMode = MetadataPersistenceMode.NotEncrypted
            });
            if (_realmUser == null) {
                _realmUser = await realmApp.LogInAsync(Credentials.EmailPassword(UsernameInput.text, PasswordInput.text));
                _realm = await Realm.GetInstanceAsync(new SyncConfiguration("game", _realmUser));
            } else {
                _realm = Realm.GetInstance(new SyncConfiguration("game", _realmUser));
            }
            _playerProfile = _realm.Find<PlayerProfile>(_realmUser.Id);
            if(_playerProfile == null) {
                _realm.Write(() => {
                    _playerProfile = _realm.Add(new PlayerProfile(_realmUser.Id));
                });
            }
            SceneManager.LoadScene("MainScene");
        }
    }

}
