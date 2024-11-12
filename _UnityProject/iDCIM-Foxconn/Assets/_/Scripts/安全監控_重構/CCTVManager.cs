using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Advanced;
using VictorDev.Common;

public class CCTVManager : iDCIM_ModuleManager
{
    [Header(">>> [WebAPI資料項] CCTV連線資訊")]
    [SerializeField] List<Data_RTSP> webapiDatas;

    [Header(">>> Landmark圖標Prefab")]
    [SerializeField] CCTV_LandMark landmarkPrefab;

    private void Start()
    {
        LandmarkManager_RE.onToggleOnEvent.AddListener(OnLandmarkToggleOnHandler);
        RaycastHitManager.onSelectObjectEvent.AddListener((targetModel) => ShowData(webapiDatas.FirstOrDefault(data => targetModel.name.Contains(data.DevicePath))));
        GetAllCCTVInfo();
    }
    private void OnLandmarkToggleOnHandler(bool isOn, ILandmarkHandler result)
    {
        if (isOn != false && result is CCTV_LandMark landmark) ShowData(landmark.data);
    }

    private void ShowData(Data_RTSP data)
    {
        Debug.Log($"CCTVManager - ShowData: {data.devicePath}");
    }

    protected override void OnShowHandler()
    {
        GetAllCCTVInfo();
    }

    protected override void OnCloseHandler()
    {
        LandmarkManager_RE.RemoveLandmarks<CCTV_LandMark, Data_RTSP>();
    }

    [ContextMenu("- [WebAPI] 取得所有CCTV設備資訊")]
    private void GetAllCCTVInfo()
    {
        void onFailed(long responseCode, string msg)
        {
            throw new NotImplementedException();
        }

        void onSuccess(long responseCode, string jsonString_PageData)
        {
            webapiDatas = JsonConvert.DeserializeObject<List<Data_RTSP>>(jsonString_PageData);
            if (isOn) ShowLandmarks();
        }

        WebAPI_SearchDeviceAsset.SearchDeviceAsset("+CCTV", onSuccess, onFailed);
    }

    private void ShowLandmarks()
        => LandmarkManager_RE.AddLandMarks(landmarkPrefab, webapiDatas, modelList);

    [Serializable]
    public class Data_RTSP : ILandmarkData
    {
        public string name;
        public string devicePath;
        /// <summary>
        /// 編號
        /// </summary>
        public string idNumber => name.Split('-')[1];

        public string DevicePath => devicePath;

        public DeviceInformation deviceInformation;

        [Serializable]
        public class DeviceInformation
        {
            public string rtsp_connection_string;
            public string description;
        }
    }

}
