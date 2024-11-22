using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Advanced;
using static Config_Enum;

/// <summary>
/// 資料項 - 帳戶
/// </summary>
[Serializable]
public class Data_User : Data_NoSQL
{
    /// <summary>
    /// 帳號
    /// </summary>
    public string Account => GetValue("Account");
    /// <summary>
    /// 密碼
    /// </summary>
    public string Password => GetValue("Password");
    /// <summary>
    /// 名稱
    /// </summary>
    public string UserName => GetValue("UserName");
    /// <summary>
    /// 角色
    /// </summary>
    public enumAccountRole Role => EnumHandler.Parse<enumAccountRole>(GetValue("Role"));
    /// <summary>
    /// 上層主管
    /// </summary>
    public string SupervisorAccount => GetValue("SupervisorAccount");
    /// <summary>
    /// 類型(local)
    /// </summary>
    public enumNetType NetType => EnumHandler.Parse<enumNetType>(GetValue("NetType"));
    /// <summary>
    /// 信箱
    /// </summary>
    public string EMail => GetValue("EMail");
    /// <summary>
    /// 語系
    /// </summary>
    public enumLanguage Language => EnumHandler.Parse<enumLanguage>(GetValue("Language"));
    /// <summary>
    /// 大頭照
    /// </summary>
    public Sprite UserPhoto { get;  set; }
    /// <summary>
    /// 型態(啟用/停用)
    /// </summary>
    public enumAccountStatus Status => EnumHandler.Parse<enumAccountStatus>(GetValue("Status"));
    /// <summary>
    /// 建立日期
    /// </summary>
    public DateTime CreateDateTime => DateTime.Parse(GetValue("CreateDateTime"));
    /// <summary>
    /// 修改日期
    /// </summary>
    public DateTime EditDateTime => DateTime.Parse(GetValue("EditDateTime"));
    /// <summary>
    /// 上次登入時間
    /// </summary>
    public DateTime LastLoginDateTime => DateTime.Parse(GetValue("LastLoginDateTime"));
    /// <summary>
    /// 停用日期
    /// </summary>
    public string SuspendDateTime => GetValue("SuspendDateTime");
    public Data_User(Dictionary<string, string> sourceData) : base(sourceData)
    {
        UserPhoto = DemoDataCenter.RandomPhoto;
    }
}
