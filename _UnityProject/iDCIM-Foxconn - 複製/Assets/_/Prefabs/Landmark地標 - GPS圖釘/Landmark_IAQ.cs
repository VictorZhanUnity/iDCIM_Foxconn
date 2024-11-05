using TMPro;
using UnityEngine;
using VictorDev.Common;

public class Landmark_IAQ : Landmark
{
    [Header(">>> 文字：目前溫度")]
    [SerializeField] private TextMeshProUGUI txtTemperature;

    public float value
    {
        set
        {
            txtTemperature.SetText(value.ToString("0.#"));
            DotweenHandler.ToBlink(txtTemperature);
        }
    }
}
