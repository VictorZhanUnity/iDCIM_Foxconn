using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Advanced
{
    public class TextInputEventHandler : MonoBehaviour
    {
        [Header(">>> [Event] - 輸入框是否有值")]
        public UnityEvent<bool> onValueChanged = new UnityEvent<bool>();
        private TMP_InputField _inputField { get; set; }
        private TMP_InputField inputField => _inputField ??= GetComponent<TMP_InputField>();
        private void onValueChangedHandler(string txt) => onValueChanged?.Invoke(string.IsNullOrEmpty(txt) == false);

        public void ShowPassword(bool isShow)
        {
            inputField.contentType = isShow ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
            inputField.ForceLabelUpdate(); // 強制刷新文字顯示;
        }

        private void OnEnable() => inputField.onValueChanged.AddListener(onValueChangedHandler);
        private void OnDisable() => inputField.onValueChanged.RemoveListener(onValueChangedHandler);
    }
}
