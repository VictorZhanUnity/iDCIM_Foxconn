using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [頁面] 帳號管理
/// </summary>
public class UIManager_AccoutnManage : MonoBehaviour
{
    [Header(">>> UI組件")]
    [SerializeField] private GameObject uiObj;
    [SerializeField] private ScrollRect scrollView_AccountList;
    [SerializeField] private ListItem_AccountList listItemPrefab;
    [SerializeField] private AccoutDetailPanel accoutDetailPanelPrefab;

    private AccoutDetailPanel currentPanel { get; set; } = null;
    private List<ListItem_AccountList> listItems { get; set; } = new List<ListItem_AccountList>();
    private List<ListItem_AccountList> searchResult { get; set; } = new List<ListItem_AccountList>();

    public bool isOn
    {
        set
        {
            uiObj.SetActive(value);
            if (value == false) RemoveListItems();
            else WebAPI_GetAccountList();
        }
    }

    /// <summary>
    /// 取得帳號資訊
    /// </summary>
    private void WebAPI_GetAccountList()
    {
        // 資料集
        List<Dictionary<string, string>> dataList = DemoDataCenter.usersRecords;

        RemoveListItems();

        //建立項目
        dataList.ForEach(data =>
        {
            ListItem_AccountList item = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_AccountList>(listItemPrefab, scrollView_AccountList.content);
            item.userData = new Data_User(data);
            item.onClickEvent.AddListener(ShowDetailPanel);
            listItems.Add(item);
        });

        searchResult = listItems;
    }

    /// <summary>
    ///移除列表原有項目
    /// </summary>
    private void RemoveListItems()
    {
        searchResult.ForEach(item =>
        {
            item.onClickEvent.RemoveListener(ShowDetailPanel);
            ObjectPoolManager.PushToPool<ListItem_AccountList>(item);
        });
        listItems.Clear();
        searchResult.Clear();
    }

    /// <summary>
    /// 顯示帳號詳細資訊面板
    /// </summary>
    public void ShowDetailPanel(ListItem_AccountList listItem)
    {
        if (currentPanel != null)
        {
            if (listItem.userData == currentPanel.userData) return;
            CloseDetailPanel();
        }
        currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<AccoutDetailPanel>(accoutDetailPanelPrefab, transform.GetChild(0), true);
        currentPanel.ShowData(listItem.userData);
        currentPanel.onClickCloseBtn.AddListener((data) => currentPanel = null);
    }

    /// <summary>
    /// [Inspector] 依表格欄位依序排列
    /// </summary>
    public void OnSortKeyEvent(string keyName, bool isDesc)
    {
        CloseDetailPanel();

        List<ListItem_AccountList> sortResult = isDesc ? searchResult.OrderByDescending(item => item.userData.GetValue(keyName)).ToList()
            : searchResult.OrderBy(item => item.userData.GetValue(keyName)).ToList();

        // 調整子物件的順序
        for (int i = 0; i < sortResult.Count; i++)
        {
            sortResult[i].transform.SetSiblingIndex(i);
        }

        // 如果你有使用ContentSizeFitter等功能，可能需要重新觸發Layout更新
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView_AccountList.content);
    }

    /// <summary>
    /// [Inspector] 依表格欄位依序排列
    /// </summary>
    public void SearchAccount(string keyAccount, Config_Enum.enumAccountRole role, Config_Enum.enumAccountStatus status)
    {
        searchResult.ForEach(item => item.gameObject.SetActive(false));

        searchResult = listItems.Where(item =>
        (string.IsNullOrEmpty(keyAccount.Trim()) || item.userData.Account.Contains(keyAccount))
        && (role.ToString() == "全部" || item.userData.Role == role)
        && (status.ToString() == "全部" || item.userData.Status == status)
        ).ToList();

        searchResult.ForEach(item => item.gameObject.SetActive(true));

        CloseDetailPanel();
    }

    /// <summary>
    /// 顯示所有帳號
    /// </summary>
    public void ShowAllAcount()
    {
        searchResult.ForEach(item => item.gameObject.SetActive(false));

        searchResult = listItems;

        searchResult.ForEach(item => item.gameObject.SetActive(true));

        CloseDetailPanel();
    }

    private void CloseDetailPanel()
    {
        if (currentPanel != null)
        {
            currentPanel.Close();
            currentPanel.onClickCloseBtn.RemoveAllListeners();
        }
    }
}

