using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VictorDev.RTSP
{
    /// <summary>
    /// [資料項] RTSP - 串流 帳號、密碼與IP設定
    /// </summary>
    [CreateAssetMenu(fileName = "SoData_RTSP_Account", menuName = "- VictorDev/Scriptable Objects/SoData_RTSP_Account")]
    public class SoData_RTSP_Account : ScriptableObject
    {
        public static string PROTOCAL = "rtsp://";

        [SerializeField] private string account, password;

        [Header(">>> [選填] - 若無則由串流資訊設定各自的IP與Port")]
        [SerializeField] private string ip;
        [SerializeField] private string port;

        [Header(">>> [選填] - 串流頻道清單，可供外部統一管理使用")]
        [SerializeField] private List<SoData_RTSP_Channel> eachChannel;

        public List<SoData_RTSP_Channel> EachChannel => eachChannel;
        /// <summary>
        /// 依照關鍵字取得指定的Channel
        /// </summary>
        public string GetChannelURL(string url_Keyword)
        {
            SoData_RTSP_Channel target = eachChannel.FirstOrDefault(channel => channel.RTSP_URL.Contains(url_Keyword));
            if (target == null) return "";

            if (target.RTSP_URL.Contains(URL)) return target.RTSP_URL;
            else return $"{PROTOCAL}{URL}/{target.RTSP_URL.Replace(PROTOCAL, "")}";
        }

        public string URL
        {
            get
            {
                string result = "";
                if (string.IsNullOrEmpty(account) == false) result = $"{account}";
                if (string.IsNullOrEmpty(password) == false) result = $"{result}:{password}";
                if (string.IsNullOrEmpty(ip) == false) result = $"{result}@{ip}";
                if (string.IsNullOrEmpty(port) == false) result = $"{result}:{port}";
                return result;
            }
        }

        private void OnValidate()
        {
            account = account.Trim();
            password = password.Trim();
            ip = ip.Trim();
            port = port.Trim();
        }
    }
}
