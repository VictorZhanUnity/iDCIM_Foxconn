using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// [資料項] - 門禁記錄
/// </summary>
[Serializable]
public class Data_AccessRecord : ILandmarkData
{
    public int currentPageIndex;
    public int totalPage;
    public PageData pageData;

    /// <summary>
    /// 模型DevicePath，門禁只有一個，所以寫死
    /// </summary>
    public string DevicePath => "Door-10";
    public string DevicePathBySplit => DevicePath.Split('-')[1];
    public string DeviceID => "FIT+TPE+DC+03F++AR+雙開-矩形- (1): 160 x 220 cm+1";

    #region [資料時間過濾 {今年、當月、今日}]

    [Header(">>> 過瀘條件後之時間資料")]
    [SerializeField] private PageData _listOfThisYear;
    [SerializeField] private PageData _listOfThisMonth;
    [SerializeField] private PageData _listOfToday;
    public PageData listOfThisYear
    {
        get
        {
            if (_listOfThisYear == null) FilterByDate();
            return _listOfThisYear;
        }
    }
    public PageData listOfThisMonth
    {
        get
        {
            if (_listOfThisMonth == null) FilterByDate();
            return _listOfThisMonth;
        }
    }
    public PageData listOfToday
    {
        get
        {
            if (_listOfToday == null) FilterByDate();
            return _listOfToday;
        }
    }
    private void FilterByDate()
    {
        _listOfThisYear = new PageData();
        _listOfThisMonth = new PageData();
        _listOfToday = new PageData();

        pageData.chartData.ForEach(data =>
        {
            if (DateTimeHandler.isDateInThisYear(data.DateFrom)) _listOfThisYear.chartData.Add(data);
            if (DateTimeHandler.isDateInThisMonth(data.DateFrom)) _listOfThisMonth.chartData.Add(data);
            if (DateTimeHandler.isDateInToday(data.DateFrom)) _listOfToday.chartData.Add(data);
        });

        pageData.users.ForEach(data =>
        {
            if (DateTimeHandler.isDateInThisYear(data.DateAccessTime)) _listOfThisYear.users.Add(data);
            if (DateTimeHandler.isDateInThisMonth(data.DateAccessTime)) _listOfThisMonth.users.Add(data);
            if (DateTimeHandler.isDateInToday(data.DateAccessTime)) _listOfToday.users.Add(data);
        });
    }

    /// <summary>
    /// 取得今年內某一時間區段的資料
    /// </summary>
    public PageData FilterFromDayInterval(DateTime from, DateTime to)
    {
        return new PageData()
        {
            chartData = pageData.chartData.Where(date => DateTimeHandler.isDateIntervalDays(date.DateFrom, from, to)).ToList(),
            users = pageData.users.Where(date => DateTimeHandler.isDateIntervalDays(date.DateAccessTime, from, to)).ToList(),
        };
    }
    /// <summary>
    /// 取得今年內某一天的資料
    /// </summary>

    public PageData FilterFromDay(DateTime day) => FilterFromDayInterval(day, day);

    #endregion

    [Serializable]
    public class PageData
    {
        public List<ChartData> chartData = new List<Data_AccessRecord.ChartData>();
        public List<User> users = new List<User>();
    }

    [Serializable]
    public class ChartData
    {
        public string From;
        public string To;
        public int Total;

        public DateTime DateFrom => DateTime.Parse(From).ToLocalTime();
        public DateTime DateTo => DateTime.Parse(To).ToLocalTime();
    }
    [Serializable]
    public class User
    {
        public string GroupName = "--- Empty ---";
        public string UserName = "--- Empty ---";
        public string AccessTime = "--- Empty ---";

        public DateTime DateAccessTime => DateTime.Parse(AccessTime).ToLocalTime();

        public string GetDateAccessTime(string dateFormat)
        {
            if (AccessTime.Contains("Empty")) return AccessTime;
            else return DateAccessTime.ToString(dateFormat);
        }

        public override string ToString()
       => $"Group: {GroupName} / User: {UserName} / AccessTime: {AccessTime}";
    }
}
