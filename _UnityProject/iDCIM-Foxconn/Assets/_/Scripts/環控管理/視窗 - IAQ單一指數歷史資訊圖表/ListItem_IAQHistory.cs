using System;
using TMPro;
using UnityEngine;

public class ListItem_IAQHistory : MonoBehaviour
{
    [SerializeField] private float value;
    [Space(20)]
    [SerializeField] private TextMeshProUGUI txtTimestamp;
    [SerializeField] private TextMeshProUGUI txtValue;
    [SerializeField] private TextMeshProUGUI txtUnit, txtUnit_ugm3;

    public DateTime timeStamp { get; private set; }

    public string iaqColumnName
    {
        set
        {
            bool isPMUnit = value.Equals("PM2.5") || value.Equals("PM10");

            txtUnit_ugm3.gameObject.SetActive(isPMUnit);
            txtUnit.gameObject.SetActive(!isPMUnit);

            if (!isPMUnit) txtUnit.SetText(Data_IAQ.ColumnUnit[value]);
        }
    }

    public void SetData(DateTime timeStamp, float value)
    {
        this.timeStamp = timeStamp;
        this.value = value;

        txtTimestamp.SetText(timeStamp.ToString(DateTimeFormatter.FullDateTimeMinuteFormat));
        txtValue.SetText(timeStamp.ToString());
    }
}
