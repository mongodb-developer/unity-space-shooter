using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Realms;
using Realms.Sync;
using Realms.Sync.Exceptions;
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
            _realmUser = await realmApp.LogInAsync(Credentials.EmailPassword(UsernameInput.text, PasswordInput.text));
            try {
                _realm = await Realm.GetInstanceAsync(new SyncConfiguration("game", _realmUser));
            } catch (ClientResetException clientResetEx) {
                if(_realm != null) {
                    _realm.Dispose();
                }
                clientResetEx.InitiateClientReset();
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
