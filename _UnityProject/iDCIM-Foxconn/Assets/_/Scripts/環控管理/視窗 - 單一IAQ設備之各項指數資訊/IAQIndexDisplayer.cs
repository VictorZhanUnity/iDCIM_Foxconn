using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

/// <summary>
/// IAQ單項指數顯示器
/// </summary>
public class IAQIndexDisplayer : MonoBehaviour
{
    [Header(">>> 點擊時Invoke")]
    [SerializeField] private Data_IAQ iaqData;

    [Header(">>> 點擊時Invoke")]
    public UnityEvent<IAQIndexDisplayer> onClickIAQIndex = new UnityEvent<IAQIndexDisplayer>();

    public string columnName;
    [SerializeField] private Button btn;
    [SerializeField] protected Image imgICON;
    [SerializeField] private TextMeshProUGUI txtValue;

    public List<string> key
    {
        get
        {
            List<string> result = new List<string>();
            string[] id = iaqData.ModelID.Split(",");
            Array.ForEach(id, modelID => result.Add($"{modelID}/{columnName}"));
            return result;
        }
    }

    public virtual Data_IAQ data
    {
        get => iaqData;
        set
        {
            iaqData = value;
            float iaqIndexValue = float.Parse(iaqData.GetValue(columnName));
            DotweenHandler.ToBlink(txtValue, null, 0.1f, 0.1f);
            DOTween.To(() => float.Parse(txtValue.text), x =>
            {
                // 更新文字
                if (columnName != "RT" && columnName != "RH") txtValue.text = x.ToString("F0");
                else txtValue.text = x.ToString("0.#");
            }, iaqIndexValue, 0.2f).SetEase(Ease.OutQuart);
        }
    }
    public Sprite imgICON_Sprite => imgICON.sprite;

    private void Start()
    {
        btn?.onClick.AddListener(() => onClickIAQIndex.Invoke(this));
    }

    private void OnEnable()
    {
        DOTween.To(() => 0, x =>
        {
            // 更新文字
            if (columnName != "RT" && columnName != "RH") txtValue.text = x.ToString("F0");
            else txtValue.text = x.ToString("0.#");
        }, float.Parse(txtValue.text), 0.15f).SetEase(Ease.OutQuart);
    }
}
