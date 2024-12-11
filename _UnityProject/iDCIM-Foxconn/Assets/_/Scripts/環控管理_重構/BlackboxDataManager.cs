using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Managers;

/// <summary>
/// 環控資料管理器
/// </summary>
public class BlackboxDataManager : Module
{
    [Header(">>> [資料項] - 目前環控資料")]
    [SerializeField] private List<Data_Blackbox> datas;

    [Header(">>> 接收到資料發送給各對像組件")]
    [SerializeField] private List<BlackboxDataReceiver> receivers;

    [Header(">>> 接收到資料時Invoke")]
    public UnityEvent<List<Data_Blackbox>> onGetBlockboxData = new UnityEvent<List<Data_Blackbox>>();

    [Header(">>> Tag名稱列表")]
    [SerializeField]
    private List<string> tagNames = new List<string>()
    {
        "PUE"
    };

    [Header(">>> 每隔幾秒讀取一次WebAPI")]
    [SerializeField] private int internvalSec = 10;

    [Header(">>> [Event] 更新時間")]
    public UnityEvent<string> onUpdateTimeEvent = new UnityEvent<string>();

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
        Debug.Log(">>> BlackboxDataManager OnInit");
        onInitComplete?.Invoke();
    }

    /// <summary>
    /// 新增Tag至列表上
    /// </summary>
    public void AddTags(List<string> tags) => tagNames.AddRange(tags);

    [ContextMenu("- 取得即時環控資料")]
    public void GetIAQRealtimeData() => WebAPI_GetRealtimeData.GetRealtimeData(new WebAPI_GetRealtimeData.SendDataFormat(tagNames), OnRefreshDataHandler, null);
    /// <summary>
    /// 取得即時資料時進行更新
    /// </summary>
    private void OnRefreshDataHandler(List<Data_Blackbox> result)
    {
        datas = result;

        //發送資料
        receivers.ForEach(receiver => receiver.ReceiveData(datas));
        onGetBlockboxData?.Invoke(datas);

        //更新時間
        DateTime updateDateTime = DateTime.Now;
        onUpdateTimeEvent?.Invoke(updateDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
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
    }

    #endregion
    private void OnDestroy() => StopCoroutine(coroutine);

}
