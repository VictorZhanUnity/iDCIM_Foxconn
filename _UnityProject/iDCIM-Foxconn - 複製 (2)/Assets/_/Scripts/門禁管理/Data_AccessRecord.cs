using System;
using System.Collections.Generic;

/// <summary>
/// [資料格式] - 門禁
/// </summary>
[Serializable]
public class Data_AccessRecord : Data_User
{
    /// <summary>
    /// 時間戳記
    /// </summary>
    public DateTime AccessTimeStamp => DateTime.Parse(GetValue("AccessTimeStamp"));

    public Data_AccessRecord(Dictionary<string, string> sourceData) : base(sourceData) { }
}
