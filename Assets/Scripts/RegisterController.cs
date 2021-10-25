using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterController : MonoBehaviour {

    public Button RegisterButton;
    public Button CancelButton;
    public InputField NameInput;
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Text ErrorText;

    void Awake() {
        ErrorText.gameObject.SetActive(false);
    }

    void Start() {
        NameInput.text = "";
        UsernameInput.text = "";
        PasswordInput.text = "";
        RegisterButton.onClick.AddListener(Register);
        CancelButton.onClick.AddListener(Login);
    }

    async void Register() {
        string registerResponse = await RealmController.Instance.Register(NameInput.text, UsernameInput.text, PasswordInput.text);
        if(registerResponse == "") {
            SceneManager.LoadScene("MainScene");
        } else {
            ErrorText.gameObject.SetActive(true);
            ErrorText.text = "ERROR: " + registerResponse;
        }
    }

    void Login() {
        SceneManager.LoadScene("LoginScene");
    }

    void Update() {
        if(Input.GetKey("escape")) {
            Application.Quit();
        }
    }

}