using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CCTVManager : iDCIM_ModuleManager
{
    [Header(">>> [資料項] CCTV連線資訊")]
    [SerializeField] List<Data_RTSP> data_RTSPs;

    protected override void OnShowHandler()
    {
        GetAllCCTVInfo();
    }

    protected override void OnCloseHandler()
    {
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
            data_RTSPs = JsonConvert.DeserializeObject<List<Data_RTSP>>(jsonString_PageData);
        }

        WebAPI_SearchDeviceAsset.SearchDeviceAsset("+CCTV", onSuccess, onFailed);
    }



    [Serializable]
    public class Data_RTSP
    {
        public string name;
        public string devicePath;
        /// <summary>
        /// 編號
        /// </summary>
        public string idNumber => name.Split('-')[1];

        public DeviceInformation deviceInformation;

        [Serializable]
        public class DeviceInformation
        {
            public string rtsp_connection_string;
            public string description;
        }
    }

}
