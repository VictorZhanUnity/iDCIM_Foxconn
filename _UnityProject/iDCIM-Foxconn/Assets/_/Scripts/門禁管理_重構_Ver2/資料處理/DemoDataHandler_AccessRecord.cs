using System;
using _VictorDEV.DateTimeUtils;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;
using static DataAccessRecord;

public class DemoDataHandlerAccessRecord : MonoBehaviour
{
    [Header(">>> 欲產生的資料筆數")]
    [SerializeField] private int amoutOfRecord = 5;

    public DataAccessRecord _accessRecord;
    
    public void GenerateDemoData(int year, Action<string> onSuccess, Action<string> onFailed = null)
    {
        Debug.Log("告警記錄 - 產生假資料...");

        _accessRecord = new DataAccessRecord();
        for (int i = 0; i < amoutOfRecord; i++)
        {
            _accessRecord.pageData.users.Add(new User()
            {
                groupName = $"群組 - {Random.Range(1,11)}",
                userName = $"使用者 - {Random.Range(1,11)}",
                accessTime = DateTimeHandler.GetRandomDateTimeInYear(year, year==DateTime.Today.Year).ToString(),
            });
        }
        onSuccess.Invoke(JsonConvert.SerializeObject(_accessRecord));
    }
}
