using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListItem_AccountList : MonoBehaviour
{
    [Header(">>> 資料項")]
    [SerializeField] private Data_User data;
    [Header(">>> UI組件")]
    [SerializeField] private Image imgUserPhoto, imgStatus;
    [SerializeField] private List<TextMeshProUGUI> txtCompList;
    [SerializeField] private Button btnRow;

    [SerializeField] private Color colorStatus_Activate, colorStatus_Forbbiden, txtForbbiden;

    public UnityEvent<ListItem_AccountList> onClickEvent = new UnityEvent<ListItem_AccountList>();

    public Data_User userData
    {
        get => data;
        set
        {
            data = value;
            data.SetValueByName(ref txtCompList);
            imgUserPhoto.sprite = data.UserPhoto;
            imgUserPhoto.color = new Color(1, 1, 1, (data.Status == Config_Enum.enumAccountStatus.啟用 ? 1f : 0.5f) );

            imgStatus.color = data.Status == Config_Enum.enumAccountStatus.啟用 ? colorStatus_Activate : colorStatus_Forbbiden;
            txtCompList.ForEach(txt =>
            {
                txt.color = data.Status == Config_Enum.enumAccountStatus.啟用 ? Color.white : txtForbbiden;
            });
        }
    }
    private void Start()
    {
        btnRow.onClick.AddListener(() => onClickEvent.Invoke(this));
    }

    private void Remove()
    {
        ObjectPoolManager.PushToPool<ListItem_AccountList>(this);
    }
}