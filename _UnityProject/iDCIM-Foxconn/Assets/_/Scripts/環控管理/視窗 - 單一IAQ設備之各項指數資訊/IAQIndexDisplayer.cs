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

    [Header(">>> Color等級顏色")]
    [SerializeField] private Color coldColor = Color.blue;    // 當值為 0 時的顏色
    [SerializeField] private Color midColor = Color.green;    // 當值為中間（約 50）時的顏色
    [SerializeField] private Color warmColor = Color.red;     // 當值為 100 時的顏色

    [Header(">>> UI組件")]
    [SerializeField] private Button btn;
    [SerializeField] protected Image imgICON;
    [SerializeField] protected TextMeshProUGUI txtValue;

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

            if (columnName == "RT")
            {
                Color targetColor = ChangeTextColor(iaqIndexValue, txtValue);
                imgICON.DOColor(targetColor, 1f);
            }
        }
    }
    public Sprite imgICON_Sprite => imgICON.sprite;

    private void Start()
    {
        btn?.onClick.AddListener(() => onClickIAQIndex.Invoke(this));
    }

    private Color ChangeTextColor(float value, TextMeshProUGUI target)
    {
        // 根據 value 的範圍來決定顏色
        Color targetColor;

        if (value <= 50)
        {
            // 0 到 50 之間，從冷色到中間色
            float t = Mathf.InverseLerp(0, 50, value);
            targetColor = Color.Lerp(coldColor, midColor, t);
        }
        else
        {
            // 50 到 100 之間，從中間色到暖色
            float t = Mathf.InverseLerp(50, 100, value);
            targetColor = Color.Lerp(midColor, warmColor, t);
        }
        // 使用 DoTween 來插值改變顏色
        target.DOColor(targetColor, 2f);
        return targetColor;
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
