using System;
using System.Collections.Generic;

/// <summary>
/// [資料項] - 門禁記錄
/// </summary>
[Serializable]
public class DataAccessRecord
{
    public int currentPageIndex = 0;
    public int totalPage = 0;
    public PageData pageData = new PageData();
    /// 模型DevicePath，門禁只有一個，所以寫死
    public string DevicePath => "10";

    [Serializable]
    public class PageData
    {
        public List<ChartData> chartData = new List<ChartData>();
        public List<User> users = new List<User>();
    }

    [Serializable]
    public class ChartData
    {
        public string from = "";
        public string to = "";
        public int total = 0;
        public DateTime DateFrom => DateTime.Parse(from).ToLocalTime();
        public DateTime DateTo => DateTime.Parse(to).ToLocalTime();
    }
    [Serializable]
    public class User
    {
        public string groupName ="";
        public string userName = "";
        public string accessTime = "";
        public DateTime DateAccessTime => DateTime.Parse(accessTime).ToLocalTime();
    }
}
