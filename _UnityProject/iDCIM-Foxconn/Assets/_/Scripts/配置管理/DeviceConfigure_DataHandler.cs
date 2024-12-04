using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Net.WebAPI;

/// <summary>
/// [配置管理] - 資料處理
/// </summary>
[Serializable]
public class DeviceConfigure_DataHandler : MonoBehaviour
{
    [Header(">>> [資料項] 所有庫存設備資料")]
    [SerializeField] private List<StockDeviceSet> stockList = new List<StockDeviceSet>();

    [Header(">>> [Event] 取得所有庫存設備資料時Invoke")]
    public UnityEvent<List<StockDeviceSet>> onGetAllStockDevices = new UnityEvent<List<StockDeviceSet>>();

    [Header(">>> [WebAPI] 取得所有庫存設備")]
    [SerializeField] private WebAPI_Request request_GetStockDevice;

    /// <summary>
    /// 取得所有庫存設備
    /// </summary>
    public void GetAllStockDevice(Action<List<StockDeviceSet>> onSuccess, Action<long, string> onFailed)
    {
        void onSuccessHandler(long responseCode, string jsonData)
        {
            //用取得所有設備，來抓取場景上的設備來群組化，用來替代庫存設備
            List<Data_ServerRackAsset> dataList = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonData);

            #region [設定Demo庫存資料]
            List<Data_DeviceAsset> filterResult = dataList.SelectMany(data => data.containers).GroupBy(data => data.modelNumber).Select(data => data.ToList()[0]).ToList();
            filterResult.ForEach(data =>
            {
                string key = data.devicePath.Split(":")[1].Trim();
                List<Transform> models = MaterialHandler.FindTargetObjects(key);

                if (models.Count > 0)
                {
                    //只將有模型的列入庫存列表
                    stockList.Add(new StockDeviceSet(data, models[0]));
                }
            });
            #endregion

            onSuccess?.Invoke(stockList);
            onGetAllStockDevices.Invoke(stockList);
        }
        stockList.Clear();

        WebAPI_LoginManager.CheckToken(request_GetStockDevice);
        WebAPI_Caller.SendRequest(request_GetStockDevice, onSuccessHandler, onFailed);
    }

    [ContextMenu("- 取得所有庫存設備")]
    private void GetAllStockDevice() => GetAllStockDevice(null, null);


    [Serializable]
    public class StockDeviceSet
    {
        public string modelNumber;
        [Header(">>> [ICON] - 選取後跟隨鼠標的ICON")]
        public Sprite dragIcon;
        [Header(">>> [模型] - 用於替換的3D物件B")]
        public Transform model;

        public Data_DeviceAsset deviceAsset;
        public StockDeviceSet(Data_DeviceAsset data, Transform model)
        {
            this.deviceAsset = data;
            this.model = model;
            modelNumber = data.modelNumber;
        }
    }
}
