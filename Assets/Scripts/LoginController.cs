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

    void Start() {
        UsernameInput.text = "nic.raboy@mongodb.com";
        PasswordInput.text = "password1234";
        LoginButton.onClick.AddListener(Login);
    }

    async void Login() {
        if(await RealmController.Instance.Login(UsernameInput.text, PasswordInput.text) != "") {
            SceneManager.LoadScene("MainScene");
        }
    }

    void Update() {
        if(Input.GetKey("escape")) {
            Application.Quit();
        }
    }

}
