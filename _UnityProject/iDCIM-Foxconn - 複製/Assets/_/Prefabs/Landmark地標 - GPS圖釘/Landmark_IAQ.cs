using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

public class Landmark_IAQ : Landmark
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_IAQ _data;

    [Header(">>> UI組件")]
    [SerializeField] private TextMeshProUGUI txtValue, txtModelID;
    [SerializeField] private Image iconRT, iconSmoke, iconSmokeWarning;

    /// <summary>
    /// 
    /// </summary>
    public void ShowData(Data_IAQ data)
    {
        _data = data;

        txtModelID.SetText(data.ModelID);

        if (string.IsNullOrEmpty(data.GetValue("RT")))
        {
            bool isHaveSmoke = bool.Parse(data.GetValue("Smoke"));
            txtValue.SetText(isHaveSmoke ? "煙霧警報" : "正常運作");
            txtValue.DOColor(isHaveSmoke ? Color.red : Color.green, 1f);

            iconRT.gameObject.SetActive(false);
            iconSmoke.gameObject.SetActive(isHaveSmoke == false);
            iconSmokeWarning.gameObject.SetActive(isHaveSmoke);
        }
        else
        {
            float rtValue = float.Parse(data.GetValue("RT"));
            txtValue.SetText(rtValue.ToString("0.#") + "°c");
            DotweenHandler.ToBlink(txtValue);

            iconRT.gameObject.SetActive(true);
            iconSmoke.gameObject.SetActive(false);
            iconSmokeWarning.gameObject.SetActive(false);
        }
    }
}
