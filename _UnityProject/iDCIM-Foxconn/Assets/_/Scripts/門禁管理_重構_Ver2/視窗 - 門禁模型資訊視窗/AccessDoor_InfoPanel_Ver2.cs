
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using UnityEngine;
using VictorDev.Common;
using static Data_AccessRecord_Ver2;

public class AccessDoor_InfoPanel_Ver2 : AccessRecordDataReceiver
{
    [Header(">>> [Prefab] ���涵��")]
    [SerializeField] private AccessRecord_TableRow itemPrefab;

    [Header(">>> ����")]
    [SerializeField] private AccessRecord_Table recordTable;

    /// <summary>
    /// ������T�O��
    /// </summary>
    private List<User> todayRecordList { get; set; } = new List<User>();

    public override void ReceiveData(List<Data_AccessRecord_Ver2> datas)
    {
        todayRecordList.Clear();
        todayRecordList = datas.SelectMany(recored => recored.pageData.users).Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).OrderBy(data => data.accessTime).ToList();
        recordTable.ShowData(todayRecordList);
    }

    #region [Call by Inspector]
    public void Show(Transform target) => gameObject.SetActive(true);
    public void Close(Transform target) => gameObject.SetActive(false);
    #endregion
}
