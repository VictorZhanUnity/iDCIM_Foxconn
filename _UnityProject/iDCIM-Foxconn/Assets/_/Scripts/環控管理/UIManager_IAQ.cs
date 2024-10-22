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
    [SerializeField] private DeviceModelVisualizerWithLandmark deviceModelVisualizer;
    [SerializeField] private GameObject canvasObj;
    [SerializeField] private IAQ_DataManager iaqDataManager;

    [SerializeField] private List<GridItem_IAQIndex> iaqRealtimeIndexList;

    [Header(">>> 更新目前平均溫度")]
    public UnityEvent<string> onUpdateCurrentAvgRT = new UnityEvent<string>();

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

    private void GetRealtimeIAQIndex()
    {
        StopCoroutine();

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
                yield return new WaitForSeconds(5);
            }
        }
        coroutineGetRealtimeIAQIndex = StartCoroutine(enumerator());
    }

    private void StopCoroutine()
    {
        if (coroutineGetRealtimeIAQIndex != null) StopCoroutine(coroutineGetRealtimeIAQIndex);
    }
}
