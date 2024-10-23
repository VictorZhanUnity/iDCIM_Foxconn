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
    [Header(">>> [��ƶ�] IAQ�]�ƫ���")]
    [SerializeField] private string modelID = "";

    [Header(">>> [��ƶ�] IAQ���")]
    [SerializeField] private Data_IAQ iaqData;

    [Header(">>> �I����@IAQ���ƶ��خ�Invoke")]
    public UnityEvent<IAQIndexDisplayer> onClickIAQIndex = new UnityEvent<IAQIndexDisplayer>();

    [Header(">>> UI�ե�")]
    [SerializeField] private List<IAQIndexDisplayer> indexDisplayer;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private AdvancedCanvasGroupFader fader;

    private Dictionary<string, Data_IAQ> iaqDataSet { get; set; }

    public string ModelID
    {
        set
        {
            modelID = value;
            if (iaqDataSet != null) ShowData(iaqDataSet);
        }
    }

    public void ShowData(Dictionary<string, Data_IAQ> iaqDataSet)
    {
        this.iaqDataSet = iaqDataSet;
        if (string.IsNullOrEmpty(modelID)) return;
        ShowData(iaqDataSet[modelID]);
    }

    private void ShowData(Data_IAQ data)
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
