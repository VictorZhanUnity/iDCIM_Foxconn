using TMPro;
using UnityEngine;

public class Landmark_IAQ : Landmark
{
    [Header(">>> 文字：目前溫度")]
    [SerializeField] private TextMeshProUGUI txtTemperature;

    public float RT
    {
        set
        {
            txtTemperature.SetText(value.ToString("0.#°c"));
        }
    }
}
