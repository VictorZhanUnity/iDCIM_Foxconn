using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using static DataAccessRecord;
using Debug = VictorDev.Common.Debug;

public class AccessDoorLandmarkDisplayer : AccessRecordDataReceiver
{
    [Header("[資料項]")]
    [SerializeField] private List<User> todayList;

    public List<User> TodayList => todayList;

    public override void ReceiveData(DataAccessRecord data)
    {
        todayList = data.pageData.users.Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).ToList();

        txtIdNumber.SetText(data.DevicePath);
        DotweenHandler.DoInt(txtAmountofEntryRecord, 0, todayList.Count, 0.2f);
    }

    private void OnEnable() => DotweenHandler.ToBlink(txtAmountofEntryRecord);

    #region [Components]
    [SerializeField] private TextMeshProUGUI txtIdNumber;
    [SerializeField] private TextMeshProUGUI txtAmountofEntryRecord;
    #endregion
}
