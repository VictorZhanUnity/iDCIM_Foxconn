using Newtonsoft.Json;
using System;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
using static VictorDev.Parser.JsonUtils;

/// <summary>
/// [EMS] 2.5 �d�߳]�ƲM��
/// </summary>
public class WebAPI_SearchDeviceAsset : SingletonMonoBehaviour<WebAPI_SearchDeviceAsset>
{
    [Header(">>> �j�M����r")]
    [SerializeField] private string keyword;

    [Header(">>> [��ƶ�] - �j�M���G")]
    [SerializeField] private SearchResultFormat searchResult;

    [Header(">>> [EMS���2.5] �d�߳]������r")]
    [SerializeField] private WebAPI_Request request_SearchDevice;

    /// <summary>
    /// [EMS] 2.5 �d�߳]�ƲM��
    /// </summary>
    /// <param name="keyword">����r</param>
    /// <param name="page">�d�߭��Ư��ޡA�w�]�� 0</param>
    /// <param name="pageItemCount">�d�ߥ������ơA�w�]��10</param>
    public static void SearchDeviceAsset(string keyword, Action<long, string> onSuccess, Action<long, string> onFailed,
        int page = 0, int pageItemCount = 10)
    {
        Instance.keyword = keyword;

        SearchFormat sendData = new SearchFormat()
        {
            page = page,
            pageItemCount = pageItemCount,
            filter = new { search = keyword, }
        };

        Instance.request_SearchDevice.SetRawJsonData(JsonConvert.SerializeObject(sendData));

        void onSuccessHandler(long responseCode, string jsonString)
        {
            Instance.searchResult = JsonConvert.DeserializeObject<SearchResultFormat>(jsonString);
            onSuccess?.Invoke(responseCode, Instance.searchResult.data.pageData);
        }

        if (WebAPI_LoginManager.CheckToken(Instance.request_SearchDevice) == false) return;
        WebAPI_Caller.SendRequest(Instance.request_SearchDevice, onSuccessHandler, onFailed);
    }

    [Serializable]
    public class SearchFormat
    {
        public int page;
        public int pageItemCount;
        public object filter;
    }

    [Serializable]
    public class SearchResultFormat
    {
        public bool isSuccess;

        public string state;
        public DataPages data;

        [Serializable]
        public class DataPages
        {
            public int currentPageIndex;
            public int totalPage;
            [JsonConverter(typeof(CustomDeserializeConverter))]
            [TextArea(1, 20)]
            public string pageData;
        }
    }

    [ContextMenu("[WebAPI] - �j�M�]��by����r]")]
    private void Test_Search()
    {
        SearchDeviceAsset(keyword, WebAPI_Caller.WebAPI_OnSuccess, WebAPI_Caller.WebAPI_OnFailed);
    }
}

