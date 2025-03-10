using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
/// <summary>
/// [資產管理] - 機房模型設備管理
/// </summary>
public class DeviceModelManager_OLD : SingletonMonoBehaviour<DeviceModelManager_OLD>
{
    [Header(">>> [資料項] 所有機櫃設備資料")]
    [SerializeField] private List<Data_ServerRackAsset> rackDataList;
    public static List<Data_ServerRackAsset> RackDataList => Instance.rackDataList;

    [Header(">>> [模型] 場景上各個設備模型")]
    [SerializeField] public List<Transform> rackModels;
    [SerializeField] List<Transform> switchModels;
    [SerializeField] List<Transform> routerModels;
    [SerializeField] List<Transform> serverModels;

    [Space(20)]
    [Header(">>> [Event] 取得所有機櫃設備資料時Invoke")]
    public static UnityEvent<List<Data_ServerRackAsset>> onGetAllRackDevices = new UnityEvent<List<Data_ServerRackAsset>>();

    [Header(">>> [WebAPI] 取得所有機櫃設備資料")]
    [SerializeField] private WebAPI_Request request_GetAllRackDevices;

    [Header(">>> 機房模型")]
    [SerializeField] private Transform serverRoomModel;

    [Header(">>> [Prefab] 機櫃RU空格物件")]
    public RackSpacer rackSpacerPrefab;

    private void Awake() => GetAllRackDevices();

    [ContextMenu("- [WebAPI] 取得所有機櫃設備資料")]
    public void GetAllRackDevices()
    {
        void onSuccessHandler(long responseCode, string jsonData)
        {
            #region [Demo設定]
            rackDataList = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonData);

            // 比對場景上的模型，設定DeviceData裡的模型物件
            rackDataList.ForEach(data => data.model = rackModels.FirstOrDefault(model => model.name.Contains(data.deviceName)));
            rackDataList.SelectMany(rack => rack.containers).ToList().ForEach(data =>
            {
                data.model = switchModels.FirstOrDefault(model => model.name.Contains(data.deviceName));
                if (data.model == null) data.model = switchModels.FirstOrDefault(model => model.name.Contains(data.deviceName));
                if (data.model == null) data.model = routerModels.FirstOrDefault(model => model.name.Contains(data.deviceName));
                if (data.model == null) data.model = serverModels.FirstOrDefault(model => model.name.Contains(data.deviceName));
            });

            //重新整理，擷取機櫃裡設備資料，Model不為null的資料項 (就是場景上無此設備模型)
            rackDataList = rackDataList.Select(rack => new Data_ServerRackAsset().RefreshData(rack)).ToList();
            #endregion

            InitializedRackDevices();
        }
        void onFailed(long responseCode, string msg)
        { }

        WebAPI_LoginManager.CheckToken(request_GetAllRackDevices);
        WebAPI_Caller.SendRequest(request_GetAllRackDevices, onSuccessHandler, onFailed);
    }

    [ContextMenu("- 將場景上的模型進行分類")]
    private void ClassfiyModelDatas()
    {
        rackModels = ObjectHandler.FindObjectsByKeywords(serverRoomModel, new List<string>() { "RACK", "ATEN" });
        switchModels = ObjectHandler.FindObjectsByKeyword(serverRoomModel, "Switch");
        routerModels = ObjectHandler.FindObjectsByKeyword(serverRoomModel, "Router");
        serverModels = ObjectHandler.FindObjectsByKeyword(serverRoomModel, "Server");
    }

    [ContextMenu("- 將設備模型放至機櫃下")]
    private void DeviceBoundCheck()
    {
        List<BoxCollider> racks = rackModels.Select(model => model.GetComponent<BoxCollider>()).ToList();
        List<BoxCollider> switches = switchModels.Select(model => model.GetComponent<BoxCollider>()).ToList();
        List<BoxCollider> routers = routerModels.Select(model => model.GetComponent<BoxCollider>()).ToList();
        List<BoxCollider> servers = serverModels.Select(model => model.GetComponent<BoxCollider>()).ToList();

        void Check(List<BoxCollider> devices)
        {
            devices.ForEach(model =>
            {
                foreach (BoxCollider rack in racks)
                {
                    if (ObjectHandler.IsModelBPartiallyInsideModelA(rack, model))
                    {
                        model.transform.parent = rack.transform;
                        break;
                    }
                }
            });
        }
        Check(switches);
        Check(routers);
        Check(servers);
        rackModels.ForEach(model => ObjectHandler.SortingChildByPosY(model));
    }

    /// <summary>
    /// 動態建立機櫃設備模型
    /// </summary>
    private void InitializedRackDevices()
    {
        //待處理
    }
    public static void HideAllRackAvailableRuSpacer() => Instance.rackDataList.ForEach(model => model.HideAvailableRuSpacer());
}
