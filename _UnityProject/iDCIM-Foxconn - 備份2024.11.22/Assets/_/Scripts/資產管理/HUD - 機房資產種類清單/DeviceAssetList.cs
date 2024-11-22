
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Parser;
using VictorDev.RevitUtils;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// [資產管理] - 機房設備種類清單
/// </summary>
public class DeviceAssetList : MonoBehaviour
{
    [Header(">>> [資料項] 機櫃Rack")]
    [SerializeField] private List<Data_ServerRackAsset> serverRackDataList;
    [Header(">>> [資料項] Server")]
    [SerializeField] private List<Data_DeviceAsset> serverDataList;
    [Header(">>> [資料項] Switch")]
    [SerializeField] private List<Data_DeviceAsset> switchDataList;
    [Header(">>> [資料項] Router")]
    [SerializeField] private List<Data_DeviceAsset> routerDataList;

    [Header(">>> 獲取所有機櫃與其設備資料時Invoke")]
    public UnityEvent<List<Data_ServerRackAsset>> onGetAllDeviceDataComplete = new UnityEvent<List<Data_ServerRackAsset>>();

    [Header(">>> 點擊資料項時Invoke")]
    public UnityEvent<ListItem_Device> onClickListItemEvent = new UnityEvent<ListItem_Device>();

    [Header(">>> UI組件")]
    [SerializeField] private ListItem_Device listItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private TMP_Dropdown dropdownBrandList;
    [SerializeField] private TextMeshProUGUI txtAmountOfServerRack, txtAmountOfServer, txtAmountOfSwitch, txtAmountOfRouter;
    [SerializeField] private Toggle toggleServerRack, toggleServer, toggleSwitch, toggleRouter;

    /// <summary>
    /// 目前選擇的設備種類清單
    /// </summary>
    private List<Data_iDCIMAsset> currentAssetList { get; set; }
    /// <summary>
    /// (設備種類、廠牌)過濾後所擷取的List
    /// </summary>
    private List<Data_iDCIMAsset> filterList { get; set; }
    private List<string> brandList { get; set; }

    private List<ListItem_Device> listItems { get; set; } = new List<ListItem_Device>();

    private Data_iDCIMAsset currentSelectData { get; set; }

    private void Start()
    {
        InitToggleListener();
        dropdownBrandList.onValueChanged.AddListener((index) =>
        {
            if (index == 0) filterList = currentAssetList;
            else
            {
                string keyWorld = dropdownBrandList.captionText.text.Split("(")[0].Trim();
                filterList = currentAssetList.Where(data => data.information.type_manufacturer.Contains(keyWorld)).ToList();
                UpdateDeviceList();
            }
        });

        toggleServerRack.isOn = true;
    }

    private void InitToggleListener()
    {
        void refreshList<T>(List<T> dataList) where T : Data_iDCIMAsset
        {
            currentAssetList = dataList.ToList<Data_iDCIMAsset>();
            filterList = currentAssetList;

            //設置廠牌DropdownList
            dropdownBrandList.options.Clear();
            brandList = new List<string>() { $"全部 (共{dataList.Count}台)" };
            brandList.AddRange(currentAssetList.GroupBy(data => data.information.type_manufacturer).Select(group => $"{group.Key} (共{group.Count()}台)").ToList());
            dropdownBrandList.AddOptions(brandList);
            dropdownBrandList.value = 0;

            UpdateDeviceList();
        }

        toggleServerRack.onValueChanged.AddListener((isOn) => { if (isOn) refreshList(serverRackDataList); });
        toggleServer.onValueChanged.AddListener((isOn) => { if (isOn) refreshList(serverDataList); });
        toggleSwitch.onValueChanged.AddListener((isOn) => { if (isOn) refreshList(switchDataList); });
        toggleRouter.onValueChanged.AddListener((isOn) => { if (isOn) refreshList(routerDataList); });
    }

    private void UpdateDeviceList()
    {
        //清除ListItem
        listItems.ForEach(item =>
        {
            item.onClickItemEvent.RemoveAllListeners();
            item.SetToggleWithoutNotify(false);
        });
        listItems.Clear();
        ObjectPoolManager.PushToPool<ListItem_Device>(scrollRect.content);
        scrollRect.verticalNormalizedPosition = 1;

        filterList.ForEach(data =>
        {
            ListItem_Device item = ObjectPoolManager.GetInstanceFromQueuePool(listItemPrefab, scrollRect.content);
            item.toggleGroup = toggleGroup;
            item.ShowData(data);
            item.onClickItemEvent.AddListener(onClickListItemEvent.Invoke);
            listItems.Add(item);
        });
    }

    private void UpdateUI()
    {
        void setAmount(TextMeshProUGUI txt, int amount) => txt.SetText($"共{amount}台");
        setAmount(txtAmountOfServerRack, serverRackDataList.Count);
        setAmount(txtAmountOfServer, serverDataList.Count);
        setAmount(txtAmountOfSwitch, switchDataList.Count);
        setAmount(txtAmountOfRouter, routerDataList.Count);
    }

    [ContextMenu("清除Inspector資料清單")]
    private void ClearAllDataList()
    {
        serverRackDataList.Clear();
        switchDataList.Clear();
        routerDataList.Clear();
        serverDataList.Clear();
    }

    public void WebAPI_onSuccess(long responseCode, string jsonString)
    {
        print(JsonUtils.PrintJSONFormatting(jsonString));
        serverRackDataList = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonString);

        onGetAllDeviceDataComplete?.Invoke(serverRackDataList);

        //計算各種類的數量
        switchDataList = new List<Data_DeviceAsset>();
        routerDataList = new List<Data_DeviceAsset>();
        serverDataList = new List<Data_DeviceAsset>();
        List<List<Data_DeviceAsset>> containerList = serverRackDataList.Select(serverRack => serverRack.containers).ToList();
        containerList.ForEach(containers =>
        {
            containers.ForEach(device =>
            {
                //分類List
                if (device.devicePath.Contains("Switch")) switchDataList.Add(device);
                else if (device.devicePath.Contains("Router")) routerDataList.Add(device);
                else if (device.devicePath.Contains("Server")) serverDataList.Add(device);
            });
        });

        //排序
        int ParseNumberFromName(string name)
        {
            // 找到 "+" 的位置
            int index = name.IndexOf('+');
            if (index != -1 && index < name.Length - 1)
            {
                // 嘗試解析 "+" 後的數字
                if (int.TryParse(name.Substring(index + 1), out int result))
                {
                    return result;
                }
            }
            // 如果找不到 "+" 或無法解析，預設回傳 0
            return 0;
        }
        serverRackDataList = serverRackDataList.OrderBy(rackData => ParseNumberFromName(rackData.deviceName)).ToList();
        switchDataList = switchDataList.OrderBy(deviceData => ParseNumberFromName(deviceData.deviceName)).ToList();
        routerDataList = routerDataList.OrderBy(deviceData => ParseNumberFromName(deviceData.deviceName)).ToList();
        serverDataList = serverDataList.OrderBy(deviceData => ParseNumberFromName(deviceData.deviceName)).ToList();

        UpdateUI();
    }

    /// <summary>
    /// 依模型名稱尋找相對應的ListItem
    /// </summary>
    public Data_iDCIMAsset SearchDeviceAssetByModel(Transform model)
    {
        currentSelectData = null;
        string modelDeviceName =RevitHandler.GetDeviceNameFromModel(model.name);

        if (model.name.ToLower().Contains("rack") || model.name.ToLower().Contains("aten-pce"))
        {
          currentSelectData = serverRackDataList.FirstOrDefault(rackData => rackData.deviceName == modelDeviceName);
           // currentSelectData = serverRackDataList.FirstOrDefault(rackData => rackData.deviceNamemodel.name.Contains(rackData.deviceName));
            //toggleServerRack.isOn = true;
        }
        else
        {
            currentSelectData = serverRackDataList.SelectMany(rackData => rackData.containers) //SelectMany展平所有內部List
              .FirstOrDefault(deviceData => deviceData.deviceName == modelDeviceName);
        }

        //檢查目前的列表上有無此資料
        ListItem_Device targetItem = listItems.FirstOrDefault(item => item.data == currentSelectData);
        //if (targetItem != null) targetItem.SetToggleWithoutNotify(true);
        return currentSelectData;
    }
}
