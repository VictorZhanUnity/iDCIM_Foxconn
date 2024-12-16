using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.Common;

public class IAQLandmarkDisplayer : BlackDataDisplayer
{
    [SerializeField] private TextMeshProUGUI txtTagName, txtValue_RT, txtValue_RH;

    #region [Value]
    public string DeviceName
    {
        get
        {
            string[] str = datas[0].tagName.Split("/");
            return $"{str[0]}/{str[1]}";
        }
    }
    public float valueRT => SearchByKeyword("RT/Value").value ?? 2;
    public float valueRH => SearchByKeyword("RH/Value").value ?? 2;
    private Data_Blackbox SearchByKeyword(string keyword) => datas.FirstOrDefault(data => data.tagName.Contains(keyword));
    #endregion

    public override void ReceiveData(List<Data_Blackbox> blackBoxData)
    {
        GroupData(blackBoxData);
        if (datas.Count == 0) return;
        txtTagName.SetText(DeviceName);
        DotweenHandler.ToBlink(txtValue_RT, valueRT.ToString("0.#"));
        DotweenHandler.ToBlink(txtValue_RH, valueRH.ToString("0.#"));
    }

    private float originalPosY { get; set; }
    private void Awake() => originalPosY = transform.localPosition.y;

    private void OnEnable()
    {
        transform.DOLocalMoveY(originalPosY, 0.1f).From(100)
            .SetEase(Ease.OutBack).SetDelay(Random.Range(0f, 0.3f));
    }
}
