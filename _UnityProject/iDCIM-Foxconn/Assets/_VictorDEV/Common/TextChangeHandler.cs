using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Advanced
{
    public class TextChangeHandler : MonoBehaviour
    {
        public UnityEvent<string> onTextChanged = new UnityEvent<string>();
        public UnityEvent<float> onTextChanged_Float = new UnityEvent<float>();
        public UnityEvent<float> onTextChanged_Percent01 = new UnityEvent<float>();

        private TextMeshProUGUI _txt { get; set; }
        private TextMeshProUGUI txt => _txt ??= GetComponent<TextMeshProUGUI>();

        private string previousTxt { get; set; }

        private void LateUpdate()
        {
            if (txt.text.Equals(previousTxt) == false)
            {
                previousTxt = txt.text;
                onTextChanged?.Invoke(previousTxt);
                if (float.TryParse(previousTxt, out float value))
                {
                    onTextChanged_Float?.Invoke(value);
                    onTextChanged_Percent01?.Invoke(value / 100);
                }
            }
        }
    }
}
