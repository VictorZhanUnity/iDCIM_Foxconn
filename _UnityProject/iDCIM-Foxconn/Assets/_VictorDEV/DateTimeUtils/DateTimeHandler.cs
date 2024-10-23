using System;

namespace VictorDev.Common
{
    /// <summary>
    /// DateTime日期格式
    /// </summary>
    public static class DateTimeHandler
    {
        /// <summary>
        /// 2024-11-24 13:33:22
        /// </summary>
        public static string FullDateTimeFormat => $"{FullDateFormat} {FullTimeFormat}";
        /// <summary>
        /// 2024-11-24 13:33
        /// </summary>
        public static string FullDateTimeMinuteFormat => $"{FullDateFormat} {HourMinuteFormat}";

        /// <summary>
        /// 2024-11-24T13:33:22
        /// </summary>
        public static string FullDateTimeFormatWithT => $"{FullDateFormat}T{FullTimeFormat}";

        /// <summary>
        /// 2024-11-24
        /// </summary>
        public static string FullDateFormat => "yyyy-MM-dd";
        /// <summary>
        /// 13:33:22
        /// </summary>
        public static string FullTimeFormat => "HH:mm:ss";
        /// <summary>
        /// 13:33
        /// </summary>
        public static string HourMinuteFormat => "HH:mm";

        // <summary>
        /// 檢查目前是否為整點 (分鐘和秒數都為0)
        /// </summary>
        public static bool isNowOnTheHour
        {
            get
            {
                // 取得當前時間
                DateTime currentTime = DateTime.Now;
                return (currentTime.Minute == 0 && currentTime.Second == 0);
            }
        }
    }
}
