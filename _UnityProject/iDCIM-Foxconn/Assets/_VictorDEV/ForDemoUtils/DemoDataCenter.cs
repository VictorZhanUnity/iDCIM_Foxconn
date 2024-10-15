using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev._FakeData;
using VictorDev.Advanced;
using VictorDev.Common;
using static Config_Enum;
using Random = UnityEngine.Random;

public class DemoDataCenter : SingletonMonoBehaviour<DemoDataCenter>
{
    [Header(">>> 門禁管理 - 記錄筆數")]
    [SerializeField] private int numOfAccessRecord = 10;
    public static List<Dictionary<string, string>> accessRecords { get; set; } = new List<Dictionary<string, string>>();
    public List<DictionaryVisualizerListItem<string, string>> accessRecordViz;

    [Header(">>> 帳號管理 - 記錄筆數")]
    [SerializeField] private int numOfUsers = 20;
    public static List<Dictionary<string, string>> usersRecords { get; set; } = new List<Dictionary<string, string>>();
    public List<DictionaryVisualizerListItem<string, string>> usersViz;

    [Header(">>> 帳號大頭照")]
    [SerializeField] private List<Sprite> userPhotoList = new List<Sprite>();

    /// <summary>
    /// 用戶大頭照
    /// </summary>
    public static Sprite RandomPhoto => Instance.userPhotoList[Random.Range(0, Instance.userPhotoList.Count)];


    [Header(">>> 資產管理 - 示範機櫃模型")]
    [SerializeField] private List<Demo_Rack> demoRackList = new List<Demo_Rack>();

    /// <summary>
    /// 資產管理 - 示範機櫃模型
    /// </summary>
    public static List<Demo_Rack> DemoRackList => Instance.demoRackList;

    private void Start()
    {
        Generate_AccessRecord(numOfAccessRecord);
        Generate_UserRecord(numOfUsers);
    }


    [ContextMenu("- 隨機生成門禁記錄")]
    private void Generate_AccessRecord() => Generate_AccessRecord(numOfAccessRecord);
    [ContextMenu("- 隨機生成帳號資料")]
    private void Generate_UserRecord() => Generate_UserRecord(numOfUsers);

    /// <summary>
    /// 隨機生成門禁記錄
    /// </summary>
    private void Generate_AccessRecord(int count)
    {
        accessRecords = Generate_UserRecord(count);
        accessRecords.ForEach(data =>
        {
            data["AccessTimeStamp"] = _FakeData_DateTime.GenerateRandomDateTime().ToString(DateTimeFormatter.FullDateTimeFormat);
        });
        accessRecordViz = DictionaryVisualizerListItem<string, string>.Parse(accessRecords);
    }

    /// <summary>
    /// 隨機生成帳號資料
    /// </summary>
    private List<Dictionary<string, string>> Generate_UserRecord(int count)
    {
        usersRecords = _FakeData_Users.GetRandomeUsers(count);
        usersRecords.ForEach(data =>
        {
            data["Role"] = EnumHandler.GetRandomFromEnum<enumAccountRole>().ToString();
            data["NetType"] = enumNetType.local.ToString();
            data["Language"] = enumLanguage.繁體中文.ToString();
            bool isActivate = Random.Range(0, 11) > 2;
            data["Status"] = isActivate ? enumAccountStatus.啟用.ToString() : enumAccountStatus.停用.ToString();

            Dictionary<string, string> dateTime = _FakeData_DateTime.GetRandomDateTimeSet();
            data["CreateDateTime"] = dateTime["StartDateTime"];
            data["EditDateTime"] = dateTime["EndDateTime"];//兩者時間調換
            data["LastLoginDateTime"] = dateTime["EditDateTime"];
            data["SuspendDateTime"] = isActivate ? "---" : dateTime["EditDateTime"];
        });

        usersViz = DictionaryVisualizerListItem<string, string>.Parse(usersRecords);
        return usersRecords;
    }
}

[Serializable]
public class Demo_Rack
{
    public Transform rack;
    [SerializeField] private List<Transform> devices;
    public List<Transform> Devices=> devices.OrderBy(x => x.position.y).ToList();
}
