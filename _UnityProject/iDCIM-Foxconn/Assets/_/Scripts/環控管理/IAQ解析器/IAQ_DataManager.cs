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


        [ContextMenu("- 測試: 取得IAQ即時各項指數")]
        public void GetRealtimeIAQIndex() => GetRealtimeIAQIndex(new List<string>() { "2132N0FF0238", "2132N0FF0239" }, null, null);

        /// <summary>
        /// 取得IAQ即時各項指數
        /// <para>+ 群組化各台IAQ指數資訊，與平均值</para>
        /// </summary>
        public void GetRealtimeIAQIndex(List<string> modelID, Action<long, Dictionary<string, Data_IAQ>, Data_IAQ> onSuccess, Action<long, string> onFailed)
        {
            //設定IAQ指數
            List<string> topicList = new List<string>();
            modelID.ForEach(id => topicList.AddRange(ToAllIndexTopic(id)));

            WebAPIManager.GetIAQRealTimeIndex(topicList, (responseCode, jsonData) =>
            {
                //解析JSON資料
                modelRealtimeData = ParseData(jsonData);
                // 使用 LINQ 來加總與計算平均值，並存入新的字典
                Dictionary<string, string> averagedDict = modelRealtimeData
                    .SelectMany(d => d.Value) // 展開內部字典
                    .GroupBy(kvp => kvp.Key)  // 依據 key 分組
                    .ToDictionary(
                        g => g.Key, // 使用 key 分組
                        g => g.Average(kvp => float.Parse(kvp.Value)).ToString("0.#") // 計算該 key 的所有值的平均
                    );

                // 儲存IAQ資訊平均值為Data_IAQ
                Data_IAQ iaqAvg = new Data_IAQ(averagedDict);

                // 儲存每一台設備的IAQ資訊為Data_IAQ
                Dictionary<string, Data_IAQ> eachIAQData = new Dictionary<string, Data_IAQ>();
                modelRealtimeData.ToList().ForEach(keyPair =>
                {
                    Data_IAQ iaqData = new Data_IAQ(keyPair.Value);
                    iaqData.ModelID = keyPair.Key;
                    eachIAQData[keyPair.Key] = iaqData;
                });

                onSuccess(responseCode, eachIAQData, iaqAvg);
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
}