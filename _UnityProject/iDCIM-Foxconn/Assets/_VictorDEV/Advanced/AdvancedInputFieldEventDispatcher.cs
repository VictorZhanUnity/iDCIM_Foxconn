using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 在Inspector裡進階處理TMP_InputField.OnValueChange事件
    /// <para>+ 直接掛在TMP_InputField上即可</para>
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class AdvancedInputFieldEventDispatcher : MonoBehaviour
    {
        [Header(">>> 是否檢查Email格式")]
        public bool isCheckEmailFormatter = false;
        [Header(">>> Keying時是否有值")]
        public UnityEvent<bool> onKeyingEvent;
        [Header(">>> 按下Enter送出值")]
        public UnityEvent<string> onSubmitEvent = new UnityEvent<string>();

        [Header(">>> 點擊輸入框時(是否聚焦)")]
        public UnityEvent<bool> onFocus = new UnityEvent<bool>();

        [Header(">>> 點擊輸入框時")]
        public UnityEvent<string> onSelect = new UnityEvent<string>();
        [Header(">>> 離開輸入框時")]
        public UnityEvent<string> onEndEdit = new UnityEvent<string>();

        [SerializeField] private TMP_InputField inputFiled;

        private void Awake()
        {
            inputFiled.onValueChanged.AddListener((txtInput) =>
            {
                bool isHaveValue = string.IsNullOrEmpty(txtInput.Trim()) == false;

                if (isHaveValue && isCheckEmailFormatter)
                {
                    isHaveValue = ValidateEmail(txtInput.Trim());
                }
                onKeyingEvent?.Invoke(isHaveValue);
            });

            onSelect.AddListener((txtInput) => onFocus.Invoke(true));
            onEndEdit.AddListener((txtInput) => onFocus.Invoke(false));

            inputFiled.onSubmit.AddListener(onSubmitEvent.Invoke);
            inputFiled.onValueChanged.Invoke(inputFiled.text);

            inputFiled.onSelect.AddListener(onSelect.Invoke);
            inputFiled.onEndEdit.AddListener(onEndEdit.Invoke);
        }

        /// <summary>
        /// 送出文字
        /// </summary>
        public void Submit() => onSubmitEvent.Invoke(inputFiled.text);

        /// <summary>
        /// 檢查內容是否為Email格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool ValidateEmail(string input)
        {
            // 正則表達式來匹配電子郵件格式
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(input, emailPattern);
        }

        private void OnValidate() => inputFiled ??= GetComponent<TMP_InputField>();
    }
}