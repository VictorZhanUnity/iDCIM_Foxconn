using System;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using static WebAPI_LoginManager;

public class LoginManager : Module
{
    [SerializeField] private string account = "TCIT", password = "TCIT";

    public override void OnInit(Action onInitComplete = null)
    {
        void onSuccess(long responseCode, Data_LoginInfo info)
        {
            Debug.Log(">>> LoginManager OnInit!!");
            onInitComplete?.Invoke();
        }
        WebAPI_LoginManager.SignIn(account, password, onSuccess, WebAPI_Caller.WebAPI_OnFailed);
    }
}