using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.DoTweenUtils;

public class Panel_AddUser : MonoBehaviour
{
    public UnityEvent<Data_User> onClickCreateUser { get; set; } = new UnityEvent<Data_User>();

    [Header(">>> UI組件")]
    [SerializeField] private Button btnAddUser;
    [SerializeField] private Image imgPortrait;
    [SerializeField] private DoTweenFadeController fadeController;
    [SerializeField] private TMP_Dropdown dropdownRole;
    [SerializeField] private TMP_InputField inputAccount, inputPassword, inputConfirmPassword, inputEmail;

    private void Start()
    {
        fadeController.OnHideEvent.AddListener(() =>
        {
            inputAccount.text = inputPassword.text = inputConfirmPassword.text = inputEmail.text = string.Empty;
            dropdownRole.value = 0;
        });
        btnAddUser.onClick.AddListener(CreateUser);
    }

    public void Show()
    {
        fadeController.ToShow(true);
    }

    public void Close() => fadeController.ToHide();

    private void CreateUser()
    {
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"Account", inputAccount.text.Trim()},
            {"Password", inputPassword.text.Trim()},
            {"UserName", inputAccount.text.Trim()},
            {"EMail", inputEmail.text.Trim()},
            {"Role", dropdownRole.captionText.text.Trim()},
            {"CreateDateTime",  DateTime.Now.ToString(DateTimeHandler.FullDateTimeFormat)},
            {"EditDateTime", DateTime.Now.ToString(DateTimeHandler.FullDateTimeFormat)}, 
            {"LastLoginDateTime", ""}, 
            
            //Temp
            {"Status", "啟用"},
            {"NetType", "Local"},
        };
        Data_User userData = new Data_User(data);
        onClickCreateUser.Invoke(userData);
        Close();
    }
}
