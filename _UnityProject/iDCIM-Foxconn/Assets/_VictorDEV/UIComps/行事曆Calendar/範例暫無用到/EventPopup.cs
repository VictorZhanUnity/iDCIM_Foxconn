using System;
using UnityEngine.UI;
using UnityEngine;

public class EventPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public Text dayText;
    public InputField eventInputField;
    public CalendarEventManager eventManager;

    private DateTime selectedDate;

    public void ShowPopup(int day, DateTime currentMonth)
    {
        selectedDate = new DateTime(currentMonth.Year, currentMonth.Month, day);
        dayText.text = selectedDate.ToString("MMMM dd, yyyy");
        popupPanel.SetActive(true);
    }

    public void AddEvent()
    {
        string eventName = eventInputField.text;
        if (!string.IsNullOrEmpty(eventName))
        {
            eventManager.AddEvent(selectedDate, eventName);
        }
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
