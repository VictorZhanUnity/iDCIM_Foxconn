using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 在Inspector裡進階處理Toggle.OnValueChange事件
    /// <para>+ 直接掛在GameObject上即可</para>
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class AdvancedToggleEventDispatcher : MonoBehaviour
    {
        [Header(">>> Awake時自動Invoke")]
        public bool isInvokeInAwake = true;

        [Header(">>> Toggle值反向Invoke")]
        public UnityEvent<bool> OnValueToReverse;

        [Header(">>> 當Toggle值為True時")]
        public UnityEvent<bool> OnValueToTrue;
        [Header(">>> 當Toggle值為False時")]
        public UnityEvent<bool> OnValueToFalse;
        [SerializeField] private Toggle toggle;

        private void Awake()
        {
            toggle.onValueChanged.AddListener(
                (isOn) =>
                {

                    if (isOn) OnValueToTrue?.Invoke(isOn);
                    else OnValueToFalse?.Invoke(isOn);

                    OnValueToReverse?.Invoke(!isOn);
                });
            if (isInvokeInAwake) toggle.onValueChanged.Invoke(toggle.isOn);
        }

        private void OnValidate() => toggle ??= GetComponent<Toggle>();
    }
}
