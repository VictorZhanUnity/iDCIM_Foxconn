using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeviceInfoPanel : MonoBehaviour
{
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ListItem_COBie listItemPrefab;
    [SerializeField] private TextMeshProUGUI txtTitle, txtDeviceName, txtSystem, txtBrand, txtType;

    private void Start()
    {
        BuildCOBieList();
        doTweenFadeController.OnFadeOutEvent.AddListener(() => ObjectPoolManager.PushToPool<DeviceInfoPanel>(this));
    }

    private void BuildCOBieList()
    {
        //移除項目
        ObjectPoolManager.PushToPool<ListItem_COBie>(scrollRect.content);

        demoCOBieData.ToList().ForEach(data =>
        {
            ListItem_COBie item = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_COBie>(listItemPrefab, scrollRect.content);
            item.ShowData(data.Key, data.Value);
        });
        scrollRect.verticalNormalizedPosition = 1;
    }

    public void Show(DeviceRUItem item)
    {
        txtTitle.SetText(item.data.deviceName);
        txtDeviceName.SetText("Schneider Rack");
        txtSystem.SetText(item.data.system);
        txtBrand.SetText("Schneider");
        txtType.SetText(item.data.system);

        doTweenFadeController.FadeIn(true);
    }

    public void Close() => doTweenFadeController.FadeOut();

    /// <summary>
    /// 顯示資料
    /// </summary>
    public void ShowData(ListItem_Device target)
    {
        throw new NotImplementedException();
    }

    private Dictionary<string, string> demoCOBieData = new Dictionary<string, string>()
    {
        {"設備描述", "--- Empty ----"}, {"設備編碼", "--- Empty ----"}, {"FM資產識別字", "--- Empty ----"}, {"安裝日期", "--- Empty ----"}, {"產品序號", "--- Empty ----"},
        {"保固開始時間", "--- Empty ----"}, {"系統類別", "--- Empty ----"}, {"系統名稱", "--- Empty ----"},

        {"設備品類名稱", "--- Empty ----"}, {"製造廠商", "--- Empty ----"}, {"產品型號", "--- Empty ----"}, {"OminiClass編碼", "--- Empty ----"}, {"無障礙功能", "--- Empty ----"},
        {"形狀", "--- Empty ----"}, {"尺吋", "--- Empty ----"}, {"顏色", "--- Empty ----"}, {"完成面", "--- Empty ----"}, {"設備分級", "--- Empty ----"},

        {"專案棟別名稱", "--- Empty ----"}, {"專案名稱", "--- Empty ----"}, {"項目地點", "--- Empty ----"}, {"樓層名稱", "--- Empty ----"}, {"空間名稱", "--- Empty ----"},
        {"空間代號", "--- Empty ----"},

        {"聯絡單位公司", "--- Empty ----"}, {"聯絡單位部門", "--- Empty ----"}, {"聯絡人EMail", "--- Empty ----"}, {"聯絡人姓氏", "--- Empty ----"}, {"聯絡人名字", "--- Empty ----"},
        {"聯絡人電話", "--- Empty ----"},  {"聯絡人地址", "--- Empty ----"},
    };
}
