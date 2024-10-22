using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 自動Lerp數值從0到目標值
    /// <para>+ 直接掛載到Slider或TextMeshPro上就好</para>
    /// </summary>
    public class AutoLerpValueController : MonoBehaviour
    {
        [Header(">>> Lerp耗時(秒)")]
        [SerializeField] private float duration = 1.5f;

        [Header(">>> UI對像組件(擇一)")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textMeshPro;

        private float targetValue { get; set; }
        private IEnumerator enumerator;

        private void Awake()
        {
            /*slider.onValueChanged.AddListener((targetValue) =>
            {
                enumerator = CoroutineHandler.LerpValue(0, targetValue, (returnValue) => slider.value = returnValue, duration + Random.Range(0, duration));
            });*/
        }

        private void OnEnable()
        {
            if (slider != null)
            {
                targetValue = slider.value;
                enumerator = CoroutineHandler_Old.LerpValue(0, targetValue, (returnValue) => slider.value = returnValue, duration + Random.Range(0, duration));
            }
            if (textMeshPro != null)
            {
                targetValue = float.Parse(textMeshPro.text);
                enumerator = CoroutineHandler_Old.LerpValue(0, targetValue, (returnValue) => textMeshPro.SetText(returnValue.ToString()), duration + Random.Range(0, duration));
            }
        }
        private void OnDisable()
        {
            if (slider != null) slider.value = targetValue;
            if (textMeshPro != null) textMeshPro.SetText(targetValue.ToString());
            CoroutineHandler_Old.CancellCoroutine(enumerator);
        }

        private void OnValidate()
        {
            slider ??= GetComponent<Slider>();
            textMeshPro ??= GetComponent<TextMeshProUGUI>();
        }
    }
}