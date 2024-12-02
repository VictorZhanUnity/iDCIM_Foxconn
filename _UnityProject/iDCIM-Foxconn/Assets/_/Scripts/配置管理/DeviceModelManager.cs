using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
/// <summary>
/// [資產管理] - 機房模型設備管理
/// </summary>
public class DeviceModelManager : SingletonMonoBehaviour<DeviceModelManager>
{
    [Header(">>> [資料項] 所有機櫃設備資料")]
    [SerializeField] private List<Data_ServerRackAsset> rackDataList;
    public static List<Data_ServerRackAsset> RackDataList => Instance.rackDataList;

    [Header(">>> [模型] 各個設備模型")]
    [SerializeField] List<Transform> rackModels;
    [SerializeField] List<Transform> switchModels;
    [SerializeField] List<Transform> routerModels;
    [SerializeField] List<Transform> serverModels;

    [Space(20)]
    [Header(">>> [Event] 取得所有機櫃設備資料時Invoke")]
    public UnityEvent<List<Data_ServerRackAsset>> onGetAllRackDevices = new UnityEvent<List<Data_ServerRackAsset>>();

    [Header(">>> [WebAPI] 取得所有機櫃設備資料")]
    [SerializeField] private WebAPI_Request request_GetAllRackDevices;

    [Header(">>> 機房模型")]
    [SerializeField] private Transform serverRoomModel;

    private void Awake() => GetAllRackDevices();

    [ContextMenu("- [WebAPI] 取得所有機櫃設備資料")]
    public void GetAllRackDevices()
    {
        void onSuccessHandler(long responseCode, string jsonData)
        {
            rackDataList = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonData);
            rackDataList.ForEach(data => data.model = rackModels.FirstOrDefault(model => model.name.Contains(data.deviceName)));

            rackDataList.ForEach(data => data.CaculateUsage());

            InitializedRackDevices();
        }
        void onFailed(long responseCode, string msg)
        { }

        WebAPI_LoginManager.CheckToken(request_GetAllRackDevices);
        WebAPI_Caller.SendRequest(request_GetAllRackDevices, onSuccessHandler, onFailed);
    }

    [ContextMenu("- 將模型進行分類")]
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
        ///


    }
}
