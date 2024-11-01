
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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

    [Header(">>> 點擊資料項時Invoke")]
    public UnityEvent<ListItem_Device> onClickListItemEvent = new UnityEvent<ListItem_Device>();

    [Header(">>> UI組件")]
    [SerializeField] private ListItem_Device listItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private TMP_Dropdown dropdownBrandList;
    [SerializeField] private TextMeshProUGUI txtAmountOfServerRack, txtAmountOfServer, txtAmountOfSwitch, txtAmountOfRouter;
    [SerializeField] private Toggle toggleServerRack, toggleServer, toggleSwitch, toggleRouter;

    private List<Data_iDCIMAsset> currentAssetList { get; set; }
    private List<Data_iDCIMAsset> filterList { get; set; }
    private List<string> brandList { get; set; }

    private List<ListItem_Device> listItems { get; set; } = new List<ListItem_Device>();

    private void Start()
    {
        InitToggleListener();
        WebAPI_GetAllRackAndDevice();
        dropdownBrandList.onValueChanged.AddListener((index) =>
        {
            if (index == 0) filterList = currentAssetList;
            else
            {
                string keyWorld = dropdownBrandList.captionText.text.Split("(")[0].Trim();
                filterList = currentAssetList.Where(data => data.manufacturer.Contains(keyWorld)).ToList();
                UpdateDeviceList();
            }
        });
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
            brandList.AddRange(currentAssetList.GroupBy(data => data.manufacturer).Select(group => $"{group.Key} (共{group.Count()}台)").ToList());
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
        listItems.ForEach(item => item.onClickItemEvent.RemoveAllListeners());
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

    [ContextMenu("[WebAPI] 所有機櫃與設備清單")]
    private void WebAPI_GetAllRackAndDevice()
    {
        void onSuccess(long responseCode, string jsonString)
        {
            print(WebAPIManager.PrintJSONFormatting(jsonString));
            serverRackDataList = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonString);
            //計算各種類的數量
            switchDataList = new List<Data_DeviceAsset>();
            routerDataList = new List<Data_DeviceAsset>();
            serverDataList = new List<Data_DeviceAsset>();
            List<List<Data_DeviceAsset>> containerList = serverRackDataList.Select(serverRack => serverRack.containers).ToList();
            containerList.ForEach(containers =>
            {
                containers.ForEach(device =>
                {
                    if (device.devicePath.Contains("Switch")) switchDataList.Add(device);
                    else if (device.devicePath.Contains("Router")) routerDataList.Add(device);
                    else if (device.devicePath.Contains("Server")) serverDataList.Add(device);
                });
            });
            UpdateUI();

            toggleServerRack.isOn = true;
        }
        WebAPIManager.GetAllDCRContainer(onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string msg)
    {

    }
}
