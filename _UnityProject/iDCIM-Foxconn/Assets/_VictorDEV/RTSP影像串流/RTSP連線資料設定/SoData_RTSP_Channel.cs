using UnityEngine;

namespace VictorDev.RTSP
{
    /// <summary>
    /// [資料項] RTSP - 串流頻道資訊設定
    /// </summary>
    [CreateAssetMenu(fileName = "SoData_RTSP_Channel", menuName = "- VictorDev/Scriptable Objects/SoData_RTSP_Channel")]
    public class SoData_RTSP_Channel : ScriptableObject
    {
        [Header(">>> RTSP位址，可包含rtsp://")]
        [SerializeField] private string URL;
        [Header(">>> [選填] - 帳號、密碼與IP設定，若無則直接使用上方URL設定")]
        [SerializeField] private SoData_RTSP_Account soData_Account;

        public string RTSP_URL
        {
            get
            {
                string result = SoData_RTSP_Account.PROTOCAL;
                if (soData_Account != null)
                {
                    if (soData_Account != null && string.IsNullOrEmpty(soData_Account.URL) == false)
                    {
                        result = $"{result}{soData_Account.URL}";
                        result = $"{result}{(soData_Account.URL.Contains("@") ? "/" : "@")}"; //檢查是否有設定IP與Port
                    }
                    else result = $"{result}/";
                }
                result = $"{result}{URL.Replace(SoData_RTSP_Account.PROTOCAL, "")}";
                return result;
            }
        }

        private void OnValidate()
        {
            URL = URL.Trim();
        }
    }
}
