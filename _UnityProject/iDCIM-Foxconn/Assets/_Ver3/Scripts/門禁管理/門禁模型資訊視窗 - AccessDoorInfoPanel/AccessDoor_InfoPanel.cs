using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using static Data_AccessRecord;

public class AccessDoor_InfoPanel : InfoPanel<Data_AccessRecord>
{
    [Header(">>> [Event] - 點擊詳細按鈕時Invoke")]
    public UnityEvent<Data_AccessRecord> onClickDetialButton = new UnityEvent<Data_AccessRecord>();

    [Header(">>> [組件] - 排序化表格")]
    [SerializeField] private AccessRecord_Table recordTable;

    [Header(">>> [組件]")]
    [SerializeField] private Button btnDetail;
    [SerializeField] private TextMeshProUGUI txtIdNumber, txtDeviceID;

    protected override void OnAwakeHandler()
    {
        btnDetail.onClick.AddListener(() => onClickDetialButton.Invoke(data));
    }

    protected override void OnShowDataHandler(Data_AccessRecord data)
    {
        txtIdNumber.SetText(data.DevicePathBySplit);
        txtDeviceID.SetText(data.DeviceID);

        List<User> filterData = data.pageData.users.Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).OrderBy(data => data.DateAccessTime).ToList();
        filterData = filterData.Count > 0 ? filterData : new List<User>() { new User() };
      //  recordTable.ShowData(filterData);
    }

    protected override void OnCloseHandler(Data_AccessRecord data)
    {
        btnDetail.onClick.RemoveAllListeners();
    }
}
