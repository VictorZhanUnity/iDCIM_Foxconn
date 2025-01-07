using System;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using VictorDev.Common;

public class ListItem_IAQHistory : MonoBehaviour
{
    [SerializeField] private float value;
    [Space(20)]
    [SerializeField] private TextMeshProUGUI txtTimestamp;
    [SerializeField] private TextMeshProUGUI txtValue;
    [SerializeField] private TextMeshProUGUI txtUnit_Others, txtUnit_ugm3;

    public DateTime timeStamp { get; private set; }

    private string txtUnit { get; set; }

    public string iaqColumnName
    {
        set
        {
            bool isPMUnit = value.Equals("PM2.5") || value.Equals("PM10");

            txtUnit_ugm3.gameObject.SetActive(isPMUnit);
            txtUnit_Others.gameObject.SetActive(!isPMUnit);

            txtUnit = isPMUnit ? txtUnit_ugm3.text : txtUnit_Others.text;

            //�]�w����r
            if (!isPMUnit) txtUnit_Others.SetText(Data_IAQ.UnitName[value].unitName);
        }
    }

    public void ShowData(DateTime timeStamp, float value)
    {
        this.timeStamp = timeStamp;
        this.value = value;

        txtTimestamp.SetText(timeStamp.ToString(DateTimeHandler.FullDateTimeMinuteFormat));
        txtValue.SetText(value.ToString("0.#"));

        name = $"DataItem - {txtValue.text} {txtUnit}";
    }
}
