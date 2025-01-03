using System;
using System.Collections.Generic;
using System.Globalization;

namespace VictorDev.Common
{
    /// <summary>
    /// DateTime日期格式
    /// </summary>
    public static class DateTimeHandler
    {
        /// <summary>
        /// 2024.11.24 13:33:22
        /// </summary>
        public static string FullDateTimeFormat => $"{FullDateFormat} {FullTimeFormat}";
        /// <summary>
        /// 2024.11.24 13:33
        /// </summary>
        public static string FullDateTimeMinuteFormat => $"{FullDateFormat}\n{HourMinuteFormat}";

        /// <summary>
        /// 2024.11.24T13:33:22
        /// </summary>
        public static string FullDateTimeFormatWithT => $"{FullDateFormat}T{FullTimeFormat}";

        /// <summary>
        /// 2024.11.24
        /// </summary>
        public static string FullDateFormat => "yyyy.MM.dd";

        /// <summary>
        /// 2024.11.24 (週六)
        /// </summary>
        public static string FullDateFormatWithWeekDay(DateTime dateTime) => dateTime.ToString($"{FullDateFormat} (ddd)", new CultureInfo("zh-TW"));

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
        /// <summary>
        /// 指定日期是否在日期區間內
        /// <parp>+ to 會自動換算為to天的23:59:59</parp>
        /// </summary>
        public static bool isDateIntervalDays(DateTime date, DateTime from, DateTime to)
        {
            from = from.Date;
            to = to.AddDays(1).AddTicks(-1);
            return date >= from && date <= to;
        }
        /// <summary>
        /// 指定日期是否在今年內
        /// </summary>
        public static bool isDateInThisYear(DateTime date)
        {
            DateTime from = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime to = from.AddYears(1).AddTicks(-1);
            return isDateIntervalDays(date, from, to);
        }
        /// <summary>
        /// 指定日期是否在當月內
        /// </summary>
        public static bool isDateInThisMonth(DateTime date)
        {
            DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime to = from.AddMonths(1).AddTicks(-1);
            return isDateIntervalDays(date, from, to);
        }
        /// <summary>
        /// 指定日期是否在某天內 {目標時間, 指定哪一天}
        /// </summary>
        public static bool isDateInDay(DateTime date, DateTime day) => isDateIntervalDays(date, day, day);
        /// <summary>
        /// 指定日期是否在今天內
        /// </summary>
        public static bool isDateInToday(DateTime date) => isDateInDay(date, DateTime.Today);

        /// <summary>
        /// 從00:00 ~ 24:00每一小時整點的字串列表
        /// </summary>
        public static List<string> hoursOfDay
        {
            get
            {
                List<string> result = new List<string>();
                // 獲取今天的日期
                DateTime startOfDay = DateTime.Today; //抓到的時間是 00:00
                // 從00:00到23:00的每小時整點時間
                for (int hour = 0; hour <= 23; hour++)
                {
                    DateTime hourlyTime = startOfDay.AddHours(hour);
                    result.Add(hourlyTime.ToString("HH:mm"));
                }
                return result;
            }
        }

        private static CultureInfo cultureInfo_ENG { get; set; }
        private static CultureInfo cultureInfo_ZH { get; set; }

        public static CultureInfo GetCulture(bool isEng = true)
        {
            if (isEng) return cultureInfo_ENG ??= new CultureInfo("en-US");
            else return cultureInfo_ZH ??= new CultureInfo("enzh-CN");
        }
    }
}
