using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 輸入框表格處理
    /// </summary>
    public class TextFormHandler : TabSwitchHandler
    {
        [Header(">>> [Event] 按下Submit時Invoke")]
        public UnityEvent onSubmitEvent = new UnityEvent();

        [Header(">>> [Event] 當有任一文字組件在Keying時Invoke")]
        public UnityEvent onValueChanged = new UnityEvent();

        [Header(">>> [Comp] Submit按鈕")]
        [SerializeField] private Button btnSubmit;

        /// <summary>
        /// 當輸入框值改變時
        /// </summary>
        private void OnValueChangedHandler(string txt)
        {
            bool allHaveValue = selectableComps.OfType<TMP_InputField>().All(inputTxt => string.IsNullOrEmpty(inputTxt.text.Trim()) == false);
            btnSubmit.interactable = allHaveValue;
            onValueChanged?.Invoke();
        }

        private void OnSubmitHandler(string txt)
        {
            if (btnSubmit.interactable)
            {
                btnSubmit.onClick.Invoke();
                onSubmitEvent?.Invoke();
            }
        }

        #region [Event Listener]
        protected override void OnAddListener(TMP_InputField inputField)
        {
            base.OnAddListener(inputField);
            inputField.onValueChanged.AddListener(OnValueChangedHandler);
            inputField.onSubmit.AddListener(OnSubmitHandler);
        }
        protected override void OnRemoveListener(TMP_InputField inputField)
        {
            base.OnRemoveListener(inputField);
            inputField.onValueChanged.RemoveListener(OnValueChangedHandler);
            inputField.onSubmit.RemoveListener(OnSubmitHandler);
        }
        #endregion
    }
}
