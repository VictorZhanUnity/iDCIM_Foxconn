using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VictorDev.Common;
using static AlarmHistoryDataManager;

/// <summary>
/// 告警分類項目顯示器
/// </summary>
public class AlarmTypeDisplayer : MonoBehaviour, IAlarmHistoryDataReceiver
{
    [Header(">>>Tag分類關鍵字")] public List<string> keywords;
    public List<string> keywordsExclude;

    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        filtData = datas.Where(data => keywords.Any(word => data.tagName.Contains(word)) 
                && keywordsExclude.All(word => data.tagName.Contains(word) == false)).ToList();

        int amount = filtData.SelectMany(data => data.alarms).Count();
        DotweenHandler.DoInt(txtAmount, int.Parse(txtAmount.text), amount);
        imgColor.DOColor(amount > 0 ? Color.red : Color.green, 0.5f).SetEase(Ease.OutQuad);
    }

    #region[Initialize]

    private void OnEnable() => btn.onClick.AddListener(() => onItemClicked?.Invoke(this.filtData));
    private void OnDisable() => btn.onClick.RemoveAllListeners();

    #endregion

    #region[Componentes]
    [FormerlySerializedAs("datas")] [Header("[資料項]")] [SerializeField] private List<Data_AlarmHistoryData> filtData;
    public UnityEvent<List<Data_AlarmHistoryData>> onItemClicked = new();
    [SerializeField] private Color colorNormal = Color.green;
    [SerializeField] private Color colorAlert = Color.red;
    private Button btn => _btn ??= transform.GetComponent<Button>();
    private Button _btn;
    private Transform imgMask => _imgMask ??= transform.Find("imgMask");
    private Transform _imgMask;
    private Image imgColor => _imgColor ??= imgMask.Find("imgColor").GetComponent<Image>();
    private Image _imgColor;
    private TextMeshProUGUI txtAmount => _txtAmount ??= imgMask.Find("txtAmount").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtAmount;

    #endregion
}