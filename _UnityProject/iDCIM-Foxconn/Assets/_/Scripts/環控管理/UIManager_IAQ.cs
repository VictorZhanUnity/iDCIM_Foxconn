using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.IAQ;
using VictorDev.RevitUtils;

public class UIManager_IAQ : MonoBehaviour
{
    [Header(">>> 間隔幾秒訪問WebAPI")]
    [Range(0, 60)]
    [SerializeField] private int intervalSendRequest = 5;

    [Header(">>> 更新目前平均溫度")]
    public UnityEvent<string> onUpdateCurrentAvgRT = new UnityEvent<string>();

    [Header(">>> UI組件")]
    [SerializeField] private List<GridItem_IAQIndex> iaqRealtimeIndexList;

    [Header(">>> UI組件")]
    [SerializeField] private DeviceModelVisualizerWithLandmark deviceModelVisualizer;
    [SerializeField] private GameObject canvasObj;
    [SerializeField] private IAQ_DataManager iaqDataManager;

   

    private Coroutine coroutineGetRealtimeIAQIndex;

    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;
            canvasObj.SetActive(value);
        }
    }

    private void Start()
    {
        GetRealtimeIAQIndex();
    }

    /// <summary>
    /// [WebAPI] 擷取即時IAQ資料，計算平均值
    /// </summary>
    public void GetRealtimeIAQIndex()
    {
        if (coroutineGetRealtimeIAQIndex != null) StopCoroutine(coroutineGetRealtimeIAQIndex);

        IEnumerator enumerator()
        {
            while (true)
            {
                List<string> modelID = deviceModelVisualizer.ModelList.Select(model => RevitHandler.GetDeviceID(model.name)).ToList();
                iaqDataManager.GetRealtimeIAQIndex(modelID, (responseCode, iaqData) =>
                {
                    if (responseCode != 200) return;
                    iaqRealtimeIndexList.ForEach(item => item.data = iaqData);
                    onUpdateCurrentAvgRT.Invoke($"{iaqData.RT.ToString("0.#")}°c");
                }, null);
                yield return new WaitForSeconds(intervalSendRequest);
            }
        }
        coroutineGetRealtimeIAQIndex = StartCoroutine(enumerator());
    }

    private void StopCoroutine()
    {
       
    }
}
