using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.DoTweenUtils;
using static Data_AccessRecord_Ver2;

public class AccessDoor_InfoPanel_Ver2 : AccessRecordDataReceiver
{
    [Header(">>> [Prefab] 表格項目")]
    [SerializeField] private AccessRecord_TableRow itemPrefab;

    [Header(">>> 表格")]
    [SerializeField] private AccessRecord_Table recordTable;

    /// <summary>
    /// 今日門禁記錄
    /// </summary>
    private List<User> todayRecordList { get; set; } = new List<User>();

    public override void ReceiveData(List<Data_AccessRecord_Ver2> datas)
    {
        todayRecordList.Clear();
        todayRecordList = datas.SelectMany(recored => recored.pageData.users).Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).ToList();
        recordTable.ShowData(todayRecordList);
    }

    #region [Call by Inspector]
    public void Show(Transform target) => doTweenFadeController.ToShow();
    public void Close(Transform target) => doTweenFadeController.ToHide();
    #endregion

    #region[Components]
    private DoTweenFadeController _doTweenFadeController { get; set; }
    private DoTweenFadeController doTweenFadeController => _doTweenFadeController ??= GetComponent<DoTweenFadeController>();
    #endregion
}
