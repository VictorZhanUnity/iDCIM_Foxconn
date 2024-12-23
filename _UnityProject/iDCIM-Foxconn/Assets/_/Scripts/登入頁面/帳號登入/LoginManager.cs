using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Managers;
using static WebAPI_LoginManager;

/// <summary>
/// 帳密登入管理器
/// </summary>
public class LoginManager : Module
{
    [Header(">>> [Event] 登入成功後Invoke")]
    public UnityEvent onLoginSuccessed = new UnityEvent();

    [Header(">>> [Event] 進行登出時Invoke")]
    public UnityEvent onLogout = new UnityEvent();

    [Header(">>> [Event] 登入失敗後Invoke")]
    public UnityEvent onLoginFailed = new UnityEvent();

    #region [>>> Components]
    [Header(">>> 是否自動登入")]
    [SerializeField] private bool isAutoLogin = false;
    [Header(">>> [Comp] 輸入框 帳號與密碼")]
    [SerializeField] private TMP_InputField inputAccount, inputPassword;
    [Header(">>> [Comp] 登入頁面CanvasGroup")]
    [SerializeField] private CanvasGroup canvasGroupLogin;
    /// <summary>
    ///  預設帳號與密碼
    /// </summary>
    private string account => "TCIT";
    private string password => "TCIT";
    private Action onInitComplete { get; set; }
    #endregion

    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        if (isAutoLogin) Login();
    }

    public void Login()
    {
        void OnFailedHandler(long responseCode, string msg)
        {
            onLoginFailed?.Invoke();
        }
        WebAPI_LoginManager.SignIn(inputAccount.text.Trim(), inputPassword.text.Trim(), OnSuccessHandler, OnFailedHandler);
    }

    private void OnSuccessHandler(long responseCode, Data_LoginInfo info)
    {
        Debug.Log(">>> LoginManager OnInit!!");

        IEnumerator onSuccess()
        {
            yield return new WaitForSeconds(0.5f);
            ToTween(false);
            onInitComplete?.Invoke();
            onLoginSuccessed?.Invoke();
        }
        StartCoroutine(onSuccess());
    }

    private void ToTween(bool isShow)
    {
        canvasGroupLogin.DOFade(isShow ? 1 : 0, isShow ? 0.5f : 0.5f).SetEase(isShow ? Ease.InQuad : Ease.OutQuad)
            .OnUpdate(() =>
            {
                canvasGroupLogin.interactable = canvasGroupLogin.blocksRaycasts = canvasGroupLogin.alpha == 1;
            })
            .OnComplete(() =>
            {
                if (isShow) inputAccount.Select();
            });
    }

    [ContextMenu("- Logout")]
    public void Logout()
    {
        inputAccount.text = inputPassword.text = "";
        ToTween(true);
        onLogout?.Invoke();
    }

    private void Awake()
    {
        canvasGroupLogin.alpha = 0;
#if UNITY_EDITOR
        inputAccount.text = account.Trim();
        inputPassword.text = password.Trim();
#endif
    }
}