using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using UnityEngine;
using static DataAccessRecord;

public class AccessDoorInfoPanel : AccessRecordDataReceiver
{
    [Header("[Prefab] - 列表項目")]
    [SerializeField] private AccessRecord_TableRow itemPrefab;

    public override void ReceiveData(DataAccessRecord data)
    {
        _todayRecordList.Clear();
        if (data.pageData.users.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _todayRecordList = data.pageData.users.Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).OrderBy(data => data.accessTime).ToList();
        RecordTable.ShowData(_todayRecordList);
    }

    #region [Components]s
    /// [資料項] - 今日進出人員
    private List<User> _todayRecordList = new List<User>();
    ///  表格
    private AccessRecord_Table RecordTable => _recordTable ??= transform.GetChild(0).Find("Container").Find("Table排序化表格").GetComponent<AccessRecord_Table>();
    private AccessRecord_Table _recordTable;
    #endregion
    #region [Call by Inspector]
    public void Show(Transform target) => gameObject.SetActive(true);
    public void Close(Transform target) => gameObject.SetActive(false);
    #endregion
}
