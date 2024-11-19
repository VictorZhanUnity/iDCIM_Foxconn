using System;
using System.Collections.Generic;

/// <summary>
/// [資料項] - 門禁記錄
/// </summary>
[Serializable]
public class Data_AccessRecord : ILandmarkData
{
    public int currentPageIndex;
    public int totalPage;
    public PageData pageData;

    string ILandmarkData.DevicePath => "Door-10";

    [Serializable]
    public class PageData
    {
        public List<ChartData> ChartData;
        public List<User> Users;
    }


    [Serializable]
    public class ChartData
    {
        public string From;
        public string To;
        public int Total;
    }
    [Serializable]
    public class User
    {
        public string GroupName;
        public string UserName;
        public string AccessTime;
    }

 
}
