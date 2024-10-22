using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 視窗 - 單一IAQ設備之各項指數資訊
/// </summary>
public class IAQDevicePanel : MonoBehaviour
{
    [Header(">>> IAQ資料項")]
    [SerializeField] private Data_IAQ iaqData;

    [Header(">>> 點擊單一IAQ指數項目時Invoke")]
    public UnityEvent<IAQIndexDisplayer> onClickIAQIndex = new UnityEvent<IAQIndexDisplayer>();

    [Header(">>> UI組件")]
    [SerializeField] private List<IAQIndexDisplayer> indexDisplayer;

    public Data_IAQ data
    {
        get => iaqData;
        set
        {
            iaqData = value;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        indexDisplayer.ForEach(item => item.data = iaqData);
    }

    private void Start()
    {
        indexDisplayer.ForEach(displayer
           => displayer.onClickIAQIndex.AddListener(onClickIAQIndex.Invoke));
    }

    private void ListenGridItemsEvent()
    {
    }
}
