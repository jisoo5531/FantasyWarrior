using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DatabaseUIManager : MonoBehaviour
{
    [Header("Login")]
    public TMP_InputField emailInput;
    public TMP_InputField pwInput;
    public Button loginButton;

    private UserData userData;

    private void Awake()
    {
        loginButton.onClick.AddListener(LoginButtonClick);
    }

    public void LoginButtonClick()
    {
        DatabaseManager.Instance.Login(emailInput.text, pwInput.text, OnLoginSuccess, OnLoginFail);
        SoundManager.Instance.PlaySound("GameStart");
    }
    private void OnLoginSuccess(UserData data)
    {
        Debug.Log("로그인 성공");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        this.userData = data;        
    }
    private void OnLoginFail()
    {
        Debug.Log("로그인 실패");
    }
}
