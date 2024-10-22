using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Advanced;

/// <summary>
/// ���� - ��@IAQ�]�Ƥ��U�����Ƹ�T
/// </summary>
public class IAQDevicePanel : MonoBehaviour
{
    [Header(">>> IAQ��ƶ�")]
    [SerializeField] private Data_IAQ iaqData;

    [Header(">>> �I����@IAQ���ƶ��خ�Invoke")]
    public UnityEvent<IAQIndexDisplayer> onClickIAQIndex = new UnityEvent<IAQIndexDisplayer>();

    [Header(">>> UI�ե�")]
    [SerializeField] private List<IAQIndexDisplayer> indexDisplayer;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private AdvancedCanvasGroupFader fader;

    public void ShowData(Data_IAQ data)
    {
        iaqData = data;
        txtTitle.SetText($"[{data.ModelID}]");
        indexDisplayer.ForEach(item => item.data = iaqData);
        fader.isOn = true;
    }

    private void Start()
    {
        indexDisplayer.ForEach(displayer
           => displayer.onClickIAQIndex.AddListener(onClickIAQIndex.Invoke));
    }
}
