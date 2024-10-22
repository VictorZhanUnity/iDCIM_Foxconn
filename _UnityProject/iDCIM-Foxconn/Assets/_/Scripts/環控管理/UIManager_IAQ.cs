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
    [Header(">>> ���j�X��X��WebAPI")]
    [Range(0, 60)]
    [SerializeField] private int intervalSendRequest = 5;

    [Header(">>> ��s�ثe�����ū�")]
    public UnityEvent<string> onUpdateCurrentAvgRT = new UnityEvent<string>();

    [Header(">>> UI�ե�")]
    [SerializeField] private List<GridItem_IAQIndex> iaqRealtimeIndexList;

    [Header(">>> UI�ե�")]
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
    /// [WebAPI] �^���Y��IAQ��ơA�p�⥭����
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
                    onUpdateCurrentAvgRT.Invoke($"{iaqData.RT.ToString("0.#")}�Xc");
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
