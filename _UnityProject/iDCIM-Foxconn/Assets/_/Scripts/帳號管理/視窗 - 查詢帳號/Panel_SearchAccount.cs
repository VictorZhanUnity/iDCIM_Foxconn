
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Panel_SearchAccount : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputKeyAccount;
    [SerializeField] private TMP_Dropdown dropDown_Role, dropDown_Status;
    [SerializeField] private Button btnShowAll, btnSearch;

    public UnityEvent onShowAllAccount = new UnityEvent();
    public UnityEvent<string, Config_Enum.enumAccountRole, Config_Enum.enumAccountStatus> onSearchAccount = new UnityEvent<string, Config_Enum.enumAccountRole, Config_Enum.enumAccountStatus>();

    private void Start()
    {
        btnShowAll.onClick.AddListener(()=>
        {
            inputKeyAccount.text = "";
            dropDown_Role.value = dropDown_Status.value = 0;
            onShowAllAccount.Invoke();
        });
        btnSearch.onClick.AddListener(InvokeSearchAccount);
    }

    public void InvokeSearchAccount()
    {
        onSearchAccount.Invoke(inputKeyAccount.text
            , (Config_Enum.enumAccountRole)(dropDown_Role.value)
            , (Config_Enum.enumAccountStatus)(dropDown_Status.value));
    }
}
