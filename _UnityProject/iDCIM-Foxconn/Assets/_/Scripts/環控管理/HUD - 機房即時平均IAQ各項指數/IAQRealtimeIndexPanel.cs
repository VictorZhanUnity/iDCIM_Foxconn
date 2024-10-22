using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Async.CoroutineUtils;
using VictorDev.Common;
using VictorDev.IAQ;
using VictorDev.RevitUtils;

/// <summary>
/// HUD - 機房即時平均IAQ各項指數
/// </summary>
public class IAQRealtimeIndexPanel : MonoBehaviour
{
    [Header(">>> [資料項] IAQ平均數據")]
    [SerializeField] private Data_IAQ iaqDataAvg;

    [Header(">>> 間隔幾秒訪問WebAPI")]
    [Range(0, 60)]
    [SerializeField] private int intervalSendRequest = 5;

    [Header(">>> 所有IAQ指數即時資訊更新時Invoke")]
    public UnityEvent<Dictionary<string, Data_IAQ>> onUpdateIAQInfo = new UnityEvent<Dictionary<string, Data_IAQ>>();

    [Header(">>> 點擊單一IAQ指數項目時Invoke")]
    public UnityEvent<IAQIndexDisplayer> onClickIAQIndex = new UnityEvent<IAQIndexDisplayer>();

    [Header(">>> UI組件")]
    [SerializeField] private List<IAQIndexDisplayer> iaqRealtimeIndexAvgList;
    [SerializeField] private UIManager_IAQ uiManager_IAQ;
    [SerializeField] private IAQ_DataManager iaqDataManager;
    [SerializeField] private TextMeshProUGUI txtLastTimestamp;

    private Coroutine coroutineGetRealtimeIAQIndex { get; set; }

    /// <summary>
    /// 每一台IAQ指數資訊 {ModelID, IAQ指數}
    /// </summary>
    public Dictionary<string, Data_IAQ> eachIAQData { get; private set; }

    private void Start()
    {
        WebAPI_GetRealtimeIAQIndex();
        iaqRealtimeIndexAvgList.ForEach(displayer
            => displayer.onClickIAQIndex.AddListener(onClickIAQIndex.Invoke));
    }

    /// <summary>
    /// [WebAPI] 擷取即時IAQ資料，計算平均值
    /// <para>+ 會持續間隔時間進行擷取，不中斷</para>
    /// </summary>
    public void WebAPI_GetRealtimeIAQIndex()
    {
        CoroutineHandler.ToStopCoroutine(coroutineGetRealtimeIAQIndex);

        IEnumerator enumerator()
        {
          /*  while (true)
            {*/
                List<string> modelID = uiManager_IAQ.deviceModelVisualizer.ModelList.Select(model => RevitHandler.GetDeviceID(model.name)).ToList();
                iaqDataManager.GetRealtimeIAQIndex(modelID, (responseCode, eachIAQData, iaqDataAvg) =>
                {
                    if (responseCode != 200) return;
                    this.iaqDataAvg = iaqDataAvg;
                    iaqRealtimeIndexAvgList.ForEach(item => item.data = iaqDataAvg);
                    this.eachIAQData = eachIAQData;

                    onUpdateIAQInfo.Invoke(eachIAQData);
                    //最後更新時間
                    DotweenHandler.ToBlink(txtLastTimestamp, DateTime.Now.ToString(DateTimeFormatter.FullDateTimeFormat));
                }, OnFailed);
                yield return new WaitForSeconds(intervalSendRequest);
            //}
        }
        coroutineGetRealtimeIAQIndex = CoroutineHandler.ToStartCoroutine(enumerator());
    }

    private void OnFailed(long responseCode, string msg)
    {
        Debug.LogError($"XXX OnFailed: {responseCode} / {msg}");
    }
}
