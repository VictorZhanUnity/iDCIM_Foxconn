using System;
using System.Collections.Generic;
using _VictorDEV.DateTimeUtils;
using VictorDev.Common;
using Random = UnityEngine.Random;

namespace VictorDev._FakeData
{
    /// <summary>
    /// [DateTime時間]
    /// </summary>
    public abstract class _FakeData_DateTime
    {
        /// <summary>
        /// 随機生成時間
        /// <para>+ 月份調整(正:現在之後 / 負：現在之前)</para>
        /// </summary>
        public static DateTime GenerateRandomDateTime(int adjustMonth = -6)
        {
            DateTime today = DateTime.Now;
            // 計算六個月前的日期
            DateTime monthsAgo = today.AddMonths(adjustMonth);

            // 生成隨機的日期時間
            // 獲取隨機的時間戳，範圍是六個月前到今天的秒數
            float randomTimeSpan = Random.Range(0f, (float)(today - monthsAgo).TotalSeconds);

            // 計算隨機日期時間
            DateTime randomDateTime = monthsAgo.AddSeconds(randomTimeSpan);
            return randomDateTime;
        }

        /// <summary>
        /// 取從往回推時間內的任一時間點 {StartDateTime、EndDateTime、EditDateTime}
        /// </summary>
        public static Dictionary<string, string> GetRandomDateTimeSet(int monthsAgo = 6)
        {
            DateTime now = DateTime.Now;
            // 計算6個月前的時間
            DateTime sixMonthsAgo = now.AddMonths(-monthsAgo);

            // 初始化資料列表
            Dictionary<string, string> resultTime = new Dictionary<string, string>();

            // 隨機生成起始時間點
            DateTime startDateTime = RandomDateTime(sixMonthsAgo, now);

            // 隨機生成結束時間點，必須晚於起始時間點
            DateTime endDateTime;
            do
            {
                endDateTime = RandomDateTime(startDateTime, now);
            } while (endDateTime <= startDateTime);

            // 隨機生成編輯時間點，必須晚於結束時間點
            DateTime editDateTime;
            do
            {
                editDateTime = RandomDateTime(endDateTime, now);
            } while (editDateTime <= endDateTime);

            // 將生成的時間點加入列表
            resultTime.Add("StartDateTime", startDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
            resultTime.Add("EndDateTime", endDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
            resultTime.Add("EditDateTime", editDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
            return resultTime;
        }

        // 生成在指定範圍內的隨機時間點
        private static DateTime RandomDateTime(DateTime start, DateTime end)
        {
            TimeSpan timeSpan = end - start;
            TimeSpan randomTimeSpan = new TimeSpan((long)(Random.value * timeSpan.Ticks));
            return start + randomTimeSpan;
        }
    }
}
