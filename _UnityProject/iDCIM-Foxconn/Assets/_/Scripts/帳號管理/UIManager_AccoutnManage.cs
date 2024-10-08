using System.Collections.Generic;
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

    public bool isOn
    {
        set
        {
            uiObj.SetActive(value);
            if(value == false) RemoveListItems();
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
    }

    /// <summary>
    ///移除列表原有項目
    /// </summary>
    private void RemoveListItems()
    {
        listItems.ForEach(item =>
        {
            item.onClickEvent.RemoveListener(ShowDetailPanel);
            ObjectPoolManager.PushToPool<ListItem_AccountList>(item);
        });
        listItems.Clear();
    }

    /// <summary>
    /// 顯示帳號詳細資訊面板
    /// </summary>
    public void ShowDetailPanel(ListItem_AccountList listItem)
    {
        if (currentPanel != null)
        {
            if (listItem.userData == currentPanel.userData) return;
            currentPanel.Close();
        }
        currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<AccoutDetailPanel>(accoutDetailPanelPrefab, transform.GetChild(0), true);
        currentPanel.ShowData(listItem.userData);
    }
}

