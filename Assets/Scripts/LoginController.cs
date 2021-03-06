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
    public Button RegisterButton;
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Text ErrorText;

    void Awake() {
        ErrorText.gameObject.SetActive(false);
    }

    void Start() {
        UsernameInput.text = "";
        PasswordInput.text = "";
        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
    }

    async void Login() {
        string loginResponse = await RealmController.Instance.Login(UsernameInput.text, PasswordInput.text);
        if(loginResponse == "") {
            SceneManager.LoadScene("MainScene");
        } else {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "ERROR: " + loginResponse;
        }
    }

    void Register() {
        SceneManager.LoadScene("RegisterScene");
    }

    void Update() {
        if(Input.GetKey("escape")) {
            Application.Quit();
        }
    }

}
