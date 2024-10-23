using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Advanced;

/// <summary>
/// 視窗 - 單一IAQ設備之各項指數資訊
/// </summary>
public class IAQDevicePanel : MonoBehaviour
{
    [Header(">>> [資料項] IAQ設備型號")]
    [SerializeField] private string modelID = "";

    [Header(">>> [資料項] IAQ資料")]
    [SerializeField] private Data_IAQ iaqData;

    [Header(">>> 點擊單一IAQ指數項目時Invoke")]
    public UnityEvent<IAQIndexDisplayer> onClickIAQIndex = new UnityEvent<IAQIndexDisplayer>();

    [Header(">>> UI組件")]
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
