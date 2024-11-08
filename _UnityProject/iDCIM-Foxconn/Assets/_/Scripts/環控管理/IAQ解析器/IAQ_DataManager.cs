using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Parser;

namespace VictorDev.IAQ
{
    /// <summary>
    /// IAQ資料解析處理
    /// </summary>
    public class IAQ_DataManager : MonoBehaviour
    {
        /// <summary>
        /// 當IAQ資料有更新時
        /// </summary>
        [Header(">>> 當IAQ資料有更新時")]
        public UnityEvent<List<Data_IAQ>> OnReceiveData = new UnityEvent<List<Data_IAQ>>();

        [Header(">>> IAQ歷史資料集 (主題名稱，資料集)")]
        [SerializeField] private List<Data_IAQ> iaqTopicList;

        private readonly List<string> indexType = new List<string>()
        {
            "RT", "RH",  "IAQ",
            "CO2", "CO", "PM2.5",  "PM10",
            "VOCs", "Formaldehyde","Ozone",    "Lit",
            "RF","RTO",
        };
        /// <summary>
        /// 各IAQ設備即時資料
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> modelRealtimeData { get; private set; }
        private Dictionary<string, Data_IAQ> eachIAQData { get; set; }

        [ContextMenu("- 測試: 取得IAQ即時各項指數")]
        public void GetRealtimeIAQIndex() => GetRealtimeIAQIndex(new List<string>() {
        "T/H-01, FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+4",
        "T/H-02, FIT+TPE+DC+03F+1+WE+GarrisonJP 溫度感應偵測器: GarrisonJP 溫度感應偵測器+1",
        "T/H-03, FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+5",
        "T/H-04, FIT+TPE+DC+03F+1+WE+GarrisonJP 溫度感應偵測器: GarrisonJP 溫度感應偵測器+2",
        "T/H-05, FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+6",
        "T/H-06, FIT+TPE+DC+03F+1+WE+GarrisonJP 溫度感應偵測器: GarrisonJP 溫度感應偵測器+3"

        }, null, null);

        /// <summary>
        /// 取得IAQ即時各項指數
        /// <para>+ 群組化各台IAQ指數資訊，與平均值</para>
        /// </summary>
        public void GetRealtimeIAQIndex(List<string> modelID, Action<long, Dictionary<string, Data_IAQ>, Data_IAQ> onSuccess, Action<long, string> onFailed)
        {
            List<string> topicList = new List<string>();

            //設定溫濕度、煙霧為Topic
            List<string> SetupTopic(string modelID)
            {
                List<string> result = new List<string>();

                string code = modelID.Split(",")[0]; //取,前的 T/H-01流水號

                if (modelID.Contains("GarrisonJP")) result.Add($"{code}/Smoke");
                else
                {
                    result.Add($"{code}/RT");
                    result.Add($"{code}/RH");
                }
                return result;
            }
            modelID.ForEach(id => topicList.AddRange(SetupTopic(id)));

            WebAPIManager.GetIAQRealTimeIndex(topicList, (responseCode, jsonData) =>
            {
                List<TagData> tagDatas = JsonConvert.DeserializeObject<List<TagData>>(jsonData);

                List<IGrouping<string, TagData>> deviceGroupList = tagDatas.GroupBy(data => data.tagName.Substring(0, data.tagName.LastIndexOf("/"))).ToList();

                //儲存即時資料 {設備名稱, {IAQ標籤, 值}}
                modelRealtimeData = deviceGroupList.ToDictionary(g => g.Key, g => g.ToDictionary(data => data.tagName.Split("/")[2], data => data.value.ToString()));

                var tagGroupList = tagDatas.GroupBy(data => data.tagName.Split("/")[2]);
                float avgRT = tagGroupList.FirstOrDefault(group => group.Key.Equals("RT")).Average(td => (float)td.value);
                float avgRH = tagGroupList.FirstOrDefault(group => group.Key.Equals("RH")).Average(td => (float)td.value);
                bool isHaveSmoke = tagGroupList.FirstOrDefault(group => group.Key.Equals("Smoke")).All(td => (bool)td.value);

                //儲存各項的平均值
                Dictionary<string, string> averagedDict = new Dictionary<string, string>()
                {
                   {"RT", avgRT.ToString()},
                   {"RH", avgRH.ToString()},
                   {"Smoke", isHaveSmoke.ToString()},
                };
                // 儲存IAQ資訊平均值為Data_IAQ
                Data_IAQ iaqAvg = new Data_IAQ(averagedDict);
                iaqAvg.ModelID = string.Join(",", modelRealtimeData.Keys.ToList());

                // 儲存每一台設備的IAQ資訊為Data_IAQ
                eachIAQData = new Dictionary<string, Data_IAQ>();
                modelRealtimeData.ToList().ForEach(keyPair =>
                {
                    Data_IAQ iaqData = new Data_IAQ(keyPair.Value);
                    iaqData.ModelID = keyPair.Key;
                    eachIAQData[keyPair.Key] = iaqData;
                });

                onSuccess?.Invoke(responseCode, eachIAQData, iaqAvg);
            }, onFailed);
        }
        private List<string> ToAllIndexTopic(string modelID) => indexType.Select(index => ToIndexTopic(modelID, index)).ToList();
        private string ToIndexTopic(string modelID, string indexName) => $"{modelID}/{indexName}";

        /// <summary>
        /// 解析IAQ {modelID, 列表{IAQ指數,值}}
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> ParseData(string jsonString)
        {
            List<Dictionary<string, string>> jsonDataList = JsonUtils.ParseJsonArray(jsonString);

            return jsonDataList
            .GroupBy(dictData => dictData["tagName"].Split('/')[0])
            .ToDictionary(group => group.Key, group => group.GroupBy(dictData => dictData["tagName"].Split('/')[1])
                .ToDictionary(group => group.Key, group => group.Sum(dictData => float.Parse(dictData["value"])).ToString())
            );
        }
    }


    [Serializable]
    public class TagData
    {
        public string tagName;
        public JValue value;
        public object alarm;
    }
}