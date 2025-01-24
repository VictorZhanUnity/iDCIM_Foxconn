using System;
using System.Collections.Generic;
using System.Globalization;

namespace _VictorDEV.DateTimeUtils
{
    /// DateTime日期格式
    public static class DateTimeHandler
    {
        /// 2024-11-24 13:33:22
        public static string FullDateTimeFormat => $"{FullDateFormat} {FullTimeFormat}";

        /// 2024-11-24 13:33
        public static string FullDateTimeMinuteFormat => $"{FullDateFormat}\n{HourMinuteFormat}";

        /// 2024-11-24T13:33:22
        public static string FullDateTimeFormatWithT => $"{FullDateFormat}T{FullTimeFormat}";

        /// 2024-11-24
        public static string FullDateFormat => "yyyy-MM-dd";

        /// 2024-11-24 (週六)
        public static string FullDateFormatWithWeekDay(DateTime dateTime) =>
            dateTime.ToString($"{FullDateFormat} (ddd)", new CultureInfo("zh-TW"));

        /// 13:33:22
        public static string FullTimeFormat => "HH:mm:ss";

        /// 13:33
        public static string HourMinuteFormat => "HH:mm";

        /// 檢查目前是否為整點 (分鐘和秒數都為0)
        public static bool isNowOnTheHour
        {
            get
            {
                // 取得當前時間
                DateTime currentTime = DateTime.Now;
                return (currentTime.Minute == 0 && currentTime.Second == 0);
            }
        }

        /// 指定日期是否在日期區間內
        /// <parp>+ to 會自動換算為to天的23:59:59</parp>
        public static bool isDateIntervalDays(DateTime date, DateTime from, DateTime to)
        {
            from = from.Date;
            to = to.AddDays(1).AddTicks(-1);
            return date >= from && date <= to;
        }

        /// 指定日期是否在今年內
        public static bool isDateInThisYear(DateTime date) => isDateInYear(date, DateTime.Now.Year);  
        
        /// 指定日期是否在指定年份內
        public static bool isDateInYear(DateTime date, int year)
        {
            DateTime from = new DateTime(year, 1, 1);
            DateTime to = from.AddYears(1).AddTicks(-1);
            return isDateIntervalDays(date, from, to);
        }
        

        /// 指定日期是否在當月內
        public static bool IsDateInThisMonth(DateTime date) =>IsDateInMonth(date, DateTime.Now.Month);
        
        /// 指定日期是否在指定月份內 {選填：指定年份}
        public static bool IsDateInMonth(DateTime date,int month, int year=-1)
        {
            DateTime from = new DateTime(year == -1? DateTime.Now.Year: year, month, 1);
            DateTime to = from.AddMonths(1).AddTicks(-1);
            return isDateIntervalDays(date, from, to);
        }

       
        /// 指定日期是否在某天內 {目標時間, 指定哪一天}
        
        public static bool isDateInDay(DateTime date, DateTime day) => isDateIntervalDays(date, day, day);

       
        /// 指定日期是否在今天內
        
        public static bool isDateInToday(DateTime date) => isDateInDay(date, DateTime.Today);

       
        /// 從00:00 ~ 24:00每一小時整點的字串列表
        
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

        /// 依年份取得隨機某一天時間點 {是否限制不超過目前時間}
        public static DateTime GetRandomDateTimeInYear(int year, bool limitToCurrentTime = false)
        {
            Random random = new Random();
            DateTime start = new DateTime(year, 1, 1); // 隨機生成的開始時間
            DateTime end = start.AddYears(1).AddTicks(-1); // 隨機生成的結束時間
            if (limitToCurrentTime)
            {
                end = DateTime.UtcNow; // 限制到目前時間
            }

            // 生成隨機時間
            TimeSpan timeSpan = end - start;
            TimeSpan newSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
            DateTime randomDateTime = start + newSpan;
            return randomDateTime;
        }

        /// 從今天日期隨機某一天時間點 {是否限制不超過目前時間}
        public static DateTime GetRandomDateTimeInToday(bool limitToCurrentTime = true)
        {
            // 獲取當前時間 或 今日午夜12點之前
            DateTime todayEnd = limitToCurrentTime ? DateTime.Now : DateTime.Now.Date.AddDays(1).AddTicks(-1);
            DateTime todayStart = todayEnd.Date; // 今天的開始時間（00:00:00）
            // 計算今天的總秒數
            TimeSpan timeSpan = todayEnd - todayStart;
            int totalSeconds = (int)timeSpan.TotalSeconds;
            Random random = new Random();
            int randomSeconds = random.Next(0, totalSeconds + 1); // 生成隨機秒數
            return todayStart.AddSeconds(randomSeconds);
        }

        /// [格式] - 全球標準時間 2024-12-29T23:49:38.241Z
        public static string Format_GlobalTime => "yyyy-MM-ddTHH:mm:ss.fffZ";

       
        /// 字串轉換成LocalTime
        
        public static DateTime StrToLocalTime(string dateTimeString) => DateTime.Parse(dateTimeString).ToLocalTime();

        public static string[] MonthName_ZH => Enum.GetNames(typeof(EnumMonthName_ZH));
        public enum EnumMonthName_ZH
        {
            一月 = 1,
            二月,
            三月,
            四月,
            五月,
            六月,
            七月,
            八月,
            九月,
            十月,
            十一月,
            十二月
        }
    }
}