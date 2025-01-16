using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using static DataAccessRecord;

public class AccessDoorLandmarkDisplayer : AccessRecordDataReceiver
{
    [Header(">>> [��ƶ�] ������T�O��")]
    [SerializeField] private List<User> todayList;

    public List<User> TodayList => todayList;

    public override void ReceiveData(List<DataAccessRecord> datas)
    {
        todayList = datas.SelectMany(data => data.pageData.users)
            .Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).ToList();

        txtIdNumber.SetText(datas[0].DevicePath);
        DotweenHandler.ToBlink(txtAmountofEntryRecord, todayList.Count.ToString());
    }

    private void OnEnable() => DotweenHandler.ToBlink(txtAmountofEntryRecord);

    #region [Components]
    [Header(">>> [�ե�] ��r")]
    [SerializeField] private TextMeshProUGUI txtIdNumber;
    [SerializeField] private TextMeshProUGUI txtAmountofEntryRecord;
    #endregion
}
