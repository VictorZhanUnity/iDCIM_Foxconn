using TMPro;
using VictorDev.Common;

namespace VictorDev.Advanced
{
    public class TextBlinker : TextMeshProUGUI
    {
        public void SetText(string text) => DotweenHandler.ToBlink(this, text);
    }
}
