using UnityEngine;

namespace VictorDev.RTSP
{
    /// <summary>
    /// [ScriptableObject] Logo設定
    /// </summary>
    [CreateAssetMenu(fileName = "SoData_Logo", menuName = "- VictorDev/Scriptable Objects/SoData_Logo")]
    public class SoData_Logo : ScriptableObject
    {
        [Header(">>> 公司名與英文名")]
        public string componey;
        public string componey_ENG;

        [Header(">>> LOGO Sprite")]
        public Sprite logo;
    }
}
