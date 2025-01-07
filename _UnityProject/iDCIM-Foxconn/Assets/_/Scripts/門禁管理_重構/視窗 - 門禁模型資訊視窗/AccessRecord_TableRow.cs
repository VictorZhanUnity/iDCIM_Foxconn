using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using VictorDev.ColorUtils;
using VictorDev.Common;
using static Data_AccessRecord_Ver2;

public class AccessRecord_TableRow : TableRow<User>
{
    protected TextMeshProUGUI _txtAccount { get; set; }
    protected TextMeshProUGUI txtAccount => _txtAccount ??= transform.Find("txtAccount")?.GetComponent<TextMeshProUGUI>();
    protected TextMeshProUGUI _txtAccessTime { get; set; }
    protected TextMeshProUGUI txtAccessTime => _txtAccessTime ??= transform.Find("txtAccessTime")?.GetComponent<TextMeshProUGUI>();

    protected bool isEmpty { get; set; }
    protected Color originalTextColor => Color.white;
    protected Color emptyColor => ColorHandler.HexToColor(0xFFFFFF, 50 / 255f);

    protected override void OnSetDataHandler(User data)
    {
        isEmpty = data.userName.ToLower().Contains("empty");

        txtAccount.SetText(data.userName);
        txtAccessTime.SetText(data.DateAccessTime.ToString(DateTimeHandler.FullTimeFormat));

        txtAccount.color = isEmpty ? emptyColor : originalTextColor;
        txtAccessTime.color = isEmpty ? emptyColor : originalTextColor;
    }

    protected override void OnToggleValueChanged(bool value)
    {
    }
}

public class AccessRecordDetail_TableRow : AccessRecord_TableRow
{
    private TextMeshProUGUI _txtGroupName { get; set; }
    private TextMeshProUGUI txtGroupName => _txtGroupName ??= transform.GetChild(0).Find("txtGroupName")?.GetComponent<TextMeshProUGUI>();

    protected override void OnSetDataHandler(User data)
    {
        base.OnSetDataHandler(data);

        txtAccessTime.SetText(data.DateAccessTime.ToString(DateTimeHandler.FullDateTimeFormat));

        txtGroupName.SetText(data.groupName);
        txtGroupName.color = isEmpty ? emptyColor : originalTextColor;
    }

    protected override void OnToggleValueChanged(bool value)
    {
    }
}