using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RevitUtils;

public class IAQLandmarkDisplayer : IAQIndexDisplayer_RE
{
    [SerializeField] private TextMeshProUGUI txtTagName, txtValue_RT, txtValue_RH;

    #region [Value]
    public string DevicePath => RevitHandler.GetDevicePath(datas[0].model.name);
    public string tagName
    {
        get
        {
            string[] str = datas[0].tagName.Split("/");
            return $"{str[0]}/{str[1]}";
        }
    }
    public float valueRT => SearchByKeyword("RT/Value").value;
    public float valueRH => SearchByKeyword("RH/Value").value;
    private Data_Blackbox SearchByKeyword(string keyword) => datas.FirstOrDefault(data => data.tagName.Contains(keyword));
    #endregion

    public override void ReceiveData(List<Data_Blackbox> datas)
    {
        GroupData(datas);
        txtTagName.SetText(tagName);
        DotweenHandler.ToBlink(txtValue_RT, valueRT.ToString("0.#"));
        DotweenHandler.ToBlink(txtValue_RH, valueRH.ToString("0.#"));
    }
}

public class Data_RTRH : ILandmarkData
{
    public List<Data_Blackbox> datas = new List<Data_Blackbox>();
    public string DevicePath => RevitHandler.GetDevicePath(datas[0].model.name);
    public string tagName
    {
        get
        {
            string[] str = datas[0].tagName.Split("/");
            return $"{str[0]}/{str[1]}";
        }
    }
    public float valueRT => SearchByKeyword("RT").value;
    public float valueRH => SearchByKeyword("RH").value;

    private Data_Blackbox SearchByKeyword(string keyword) => datas.FirstOrDefault(data => data.tagName.Contains(keyword));
}