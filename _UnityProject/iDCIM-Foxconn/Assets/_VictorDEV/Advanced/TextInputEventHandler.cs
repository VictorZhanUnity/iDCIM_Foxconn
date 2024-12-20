using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Advanced
{
    public class TextInputEventHandler : MonoBehaviour
    {
        [Header(">>> [Event] - ��J�جO�_����")]
        public UnityEvent<bool> onValueChanged = new UnityEvent<bool>();
        private TMP_InputField _inputField { get; set; }
        private TMP_InputField inputField => _inputField ??= GetComponent<TMP_InputField>();
        private void onValueChangedHandler(string txt) => onValueChanged?.Invoke(string.IsNullOrEmpty(txt) == false);

        public void ShowPassword(bool isShow)
        {
            inputField.contentType = isShow ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
            inputField.ForceLabelUpdate(); // �j���s��r���;
        }

        private void OnEnable() => inputField.onValueChanged.AddListener(onValueChangedHandler);
        private void OnDisable() => inputField.onValueChanged.RemoveListener(onValueChangedHandler);
    }
}
