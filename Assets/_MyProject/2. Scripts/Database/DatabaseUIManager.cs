using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class DatabaseUIManager : MonoBehaviour
{
    [Header("Login")]
    public GameObject loginPanel;
    public TMP_InputField emailInput;
    public TMP_InputField pwInput;
    public Button loginButton;
    public Button goSignUpButton;

    [Header("SignUp")]
    public GameObject signUpPanel;
    public TMP_InputField userNameInput;
    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPwInput;
    public Button signUpButton;
    public Button goLoginButton;

    private UserData userData;

    private void Awake()
    {        
        loginButton.onClick.AddListener(LoginButtonClick);
        signUpButton.onClick.AddListener(SignUpButtonClick);
        goSignUpButton.onClick.AddListener(GoSignUpButtonClick);
        goLoginButton.onClick.AddListener(GoLoginButtonClick);
    }

    public void LoginButtonClick()
    {
        DatabaseManager.Instance.Login(emailInput.text, pwInput.text, OnLoginSuccess, OnLoginFail);
    }
    private void OnLoginSuccess(UserData data)
    {
        Debug.Log("로그인 성공");

        this.userData = data;
        NetworkManager.singleton.StartClient();
        
    }
    private void OnLoginFail()
    {
        Debug.Log("로그인 실패");
    }    
    private void GoSignUpButtonClick()
    {
        Debug.Log("클릭!");
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }
    private void GoLoginButtonClick()
    {
        Debug.Log("클릭?");
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }
    private void SignUpButtonClick()
    {
        DatabaseManager.Instance.SignUP(userNameInput.text, signUpEmailInput.text, signUpPwInput.text);
        GoLoginButtonClick();
    }    
}
