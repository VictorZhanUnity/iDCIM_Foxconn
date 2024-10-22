using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
