using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.Advanced
{
    /// 輸入框表格處理
    public class TextFormHandler : TabSwitchHandler
    {
        [Header(">>> [Event] 按下Submit時Invoke")]
        public UnityEvent onSubmitEvent = new UnityEvent();

        [Header(">>> [Event] 當有任一文字組件在Keying時Invoke {是否全部文字框含有值}")]
        public UnityEvent<bool> onValueChanged = new UnityEvent<bool>();

        [Header(">>> [Comp] Submit按鈕")]
        [SerializeField] private Button btnSubmit;

        /// 當輸入框值改變時，是否全部文字框含有值，決定Submit按鈕是否顯示
        private void OnValueChangedHandler(string txt)
        {
            bool allHaveValue = selectableComps.OfType<TMP_InputField>().All(inputTxt => string.IsNullOrEmpty(inputTxt.text.Trim()) == false);
            btnSubmit.interactable = allHaveValue;
            onValueChanged?.Invoke(allHaveValue);
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
