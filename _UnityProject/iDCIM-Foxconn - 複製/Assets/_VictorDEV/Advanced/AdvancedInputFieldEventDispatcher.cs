using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Advanced
{
    /// <summary>
    /// �bInspector�̶i���B�zTMP_InputField.OnValueChange�ƥ�
    /// <para>+ �������bTMP_InputField�W�Y�i</para>
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class AdvancedInputFieldEventDispatcher : MonoBehaviour
    {
        [Header(">>> �O�_�ˬdEmail�榡")]
        public bool isCheckEmailFormatter = false;
        [Header(">>> Keying�ɬO�_����")]
        public UnityEvent<bool> onKeyingEvent;
        [Header(">>> ���UEnter�e�X��")]
        public UnityEvent<string> onSubmitEvent = new UnityEvent<string>();

        [Header(">>> �I����J�خ�(�O�_�E�J)")]
        public UnityEvent<bool> onFocus = new UnityEvent<bool>();

        [Header(">>> �I����J�خ�")]
        public UnityEvent<string> onSelect = new UnityEvent<string>();
        [Header(">>> ���}��J�خ�")]
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
        /// �e�X��r
        /// </summary>
        public void Submit() => onSubmitEvent.Invoke(inputFiled.text);

        /// <summary>
        /// �ˬd���e�O�_��Email�榡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool ValidateEmail(string input)
        {
            // ���h��F���Ӥǰt�q�l�l��榡
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(input, emailPattern);
        }

        private void OnValidate() => inputFiled ??= GetComponent<TMP_InputField>();
    }
}