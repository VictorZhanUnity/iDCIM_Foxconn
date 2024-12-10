using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RevitUtils;

public class IAQLandmark : LandmarkHandler<Data_RTRH>
{
    [SerializeField] private TextMeshProUGUI txtIdNumber, txtValue_RT, txtValue_RH;

    protected override void OnShowDataHandler(Data_RTRH data)
    {
        txtIdNumber.SetText(data.tagName);
        DotweenHandler.ToBlink(txtValue_RT, data.valueRT.ToString("0.#"));
        DotweenHandler.ToBlink(txtValue_RH, data.valueRH.ToString("0.#"));
    }

    protected override void OnToggleOnHandler()
    {
    }
    protected override void OnToggleOffHandler()
    {
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
