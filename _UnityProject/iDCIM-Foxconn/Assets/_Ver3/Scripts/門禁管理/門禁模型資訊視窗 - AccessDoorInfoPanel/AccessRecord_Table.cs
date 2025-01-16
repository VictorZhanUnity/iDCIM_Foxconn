using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using User = DataAccessRecord.User;

/// <summary>
/// [門禁管理] - 門禁記錄表格
/// </summary>
public class AccessRecord_Table : TableScrollRect<User, AccessRecord_TableRow>
{
    protected override void onClickHeaderSortHandler(bool isDesc, string label)
    {
        List<TableRow<User>> filterList = headers[0].label.Contains(label)
            ? rowList.OrderBy(row => row.data.userName).ToList()
            : rowList.OrderBy(row => row.data.DateAccessTime).ToList();

        filterList.ForEach(row =>
        {
            if (isDesc) row.transform.SetAsFirstSibling();
            else row.transform.SetAsLastSibling();
        });
    }

    protected override void onClickItemEventHandler(User data)
    {
        Debug.Log($"onClickItemEventHandler: {data}");
    }
    protected override void OnShowDataHandler(List<User> data)
    {
     /*   if (data.Count == 0)
        {
            AccessRecord_TableRow rowItem = Instantiate(rowPrefab, scrollRect.content);
            rowItem.SetData(new User());
        }*/
    }
}