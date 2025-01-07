using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using static AlarmHistoryDataManager;

/// <summary>
/// 告警分類項目顯示器
/// </summary>
public class AlarmTypeDisplayer : MonoBehaviour, IAlarmHistoryDataReceiver
{
    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        this.datas = datas;
        DotweenHandler.DoInt(txtAmount, int.Parse(txtAmount.text), datas.Count);
        imgColor.DOColor(datas.Count > 0 ? Color.red : Color.green, 0.5f).SetEase(Ease.OutQuad);
    }

    #region[Componentes]
    [Header("[資料項]")]
    private List<Data_AlarmHistoryData> datas;

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
