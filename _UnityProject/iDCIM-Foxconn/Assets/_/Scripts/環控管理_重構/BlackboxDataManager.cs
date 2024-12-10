using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Advanced;
using VictorDev.Common;

/// <summary>
/// 環控資料管理器
/// </summary>
public class BlackboxDataManager : ModulePage
{
    [Header(">>> [資料項] - 目前環控資料")]
    [SerializeField] private List<Data_Blackbox> datas;

    [Header(">>> 接收到資料發送給各對像組件")]
    [SerializeField] private List<BlackboxDataReceiver> receivers;

    [Header(">>> 其它環控Tag")]
    [SerializeField]
    private List<string> otherTags = new List<string>()
    {
        "PUE"
    };

    [Header(">>> 每隔幾秒讀取一次WebAPI")]
    [SerializeField] private int internvalSec = 10;

    [Header(">>> [Event] 更新時間")]
    public UnityEvent<string> onUpdateTimeEvent = new UnityEvent<string>();

    [Header(">>> Landmark圖標Prefab")]
    [SerializeField] private IAQLandmark landmarkPrefab;

    #region [Initialize]
    private Coroutine coroutine { get; set; }
    public override void OnInit(Action onInitComplete = null)
    {
        IEnumerator GetData_Coroutine()
        {
            while (true)
            {
                GetIAQRealtimeData();
                yield return new WaitForSeconds(internvalSec);
            }
        }
        coroutine = StartCoroutine(GetData_Coroutine());
        onInitComplete?.Invoke();
    }

    [ContextMenu("- 取得即時環控資料")]
    public void GetIAQRealtimeData()
    {
        //設定感測器Tag
        List<string> tagNames = modelList.Select(model => model.name.Split(",")[0]).ToList();
        tagNames = tagNames.SelectMany(tag => tag.Contains("04") ? new[] { $"{tag}/Smoke/Status" }
        : new[] { $"{tag}/RT/Value", $"{tag}/RT/Status", $"{tag}/RH/Value", $"{tag}/RH/Status" }).ToList();
        //加入其它環控項目Tag
        tagNames.AddRange(otherTags);
        WebAPI_GetRealtimeData.GetRealtimeData(new WebAPI_GetRealtimeData.SendDataFormat(tagNames), OnRefreshDataHandler, null);
    }
    /// <summary>
    /// 取得即時資料時進行更新
    /// </summary>
    private void OnRefreshDataHandler(List<Data_Blackbox> result)
    {
        List<string> modelNameList = modelList.Select(model => model.name).ToList();
        result.ForEach(data =>
        {
            string[] str = data.tagName.Split("/");
            if (str.Length > 1)
            {
                string tagName = $"{str[0]}/{str[1]}";
                data.model = modelList.FirstOrDefault(model => model.name.Contains(tagName));
            }
        });
        datas = result;

        receivers.ForEach(receiver => receiver.ReceiveData(datas));
        DateTime updateDateTime = DateTime.Now;
        onUpdateTimeEvent?.Invoke(updateDateTime.ToString(DateTimeHandler.FullDateTimeFormat));

        SetupLandmark();
    }

    /// <summary>
    /// 設定圖標
    /// </summary>
    private void SetupLandmark()
    {
        Dictionary<string, List<Data_Blackbox>> filter = datas.Where(data => data.tagName.Contains("T/H") && data.tagName.Contains("Smoke") == false).GroupBy(data =>
        {
            string[] str = data.tagName.Split("/");
            return $"{str[0]}/{str[1]}";
        }).ToDictionary(data => data.Key, data => data.ToList());

        List<Data_RTRH> result = new List<Data_RTRH>();
        filter.ToList().ForEach(keyPair =>
        {
            Data_RTRH data = new Data_RTRH();
            data.datas.AddRange(keyPair.Value);
            result.Add(data);
        });

        
     /*   List<IAQLandmark> landmarks = new List<IAQLandmark>();
        result.ForEach(data =>
        {
            IAQLandmark item = ObjectPoolManager.GetInstanceFromQueuePool(landmarkPrefab, content.transform);
            item.columnName = 
            landmarks.Add(item);
        });
*/

       // LandmarkManager_Ver3.CreateLandMarks(landmarkPrefab, result, modelList);
    }

    #endregion

    protected override void InitEventListener()
    {
    }

    protected override void RemoveEventListener()
    {
    }

    protected override void OnShowHandler()
    {
    }

    protected override void OnCloseHandler()
    {
    }
    private void OnDestroy() => StopCoroutine(coroutine);
}
