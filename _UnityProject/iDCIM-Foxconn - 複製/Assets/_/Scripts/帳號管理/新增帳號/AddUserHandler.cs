using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AddUserHandler : MonoBehaviour
{
    [Header(">>> 點選新增帳號時Invoke")]
    public UnityEvent<Data_User> onClickCreateUser = new UnityEvent<Data_User>();

    [Header(">>> UI組件")]
    [SerializeField] private Image bkg;
    [SerializeField] private Panel_AddUser addUserPanel;
    [SerializeField] private Panel_Confirm confirmPanel;
    [SerializeField] private Sprite defaultPortarit;

    public bool isOn
    {
        set
        {
            bkg.enabled = value;

            if (value) addUserPanel.Show();
            else addUserPanel.Close();
            confirmPanel.Close();
        }
    }

    private void Start()
    {
        isOn = false;

        addUserPanel.onClickCreateUser.AddListener((data) =>
        {
            data.UserPhoto = defaultPortarit;
            confirmPanel.ShowData(data);
            onClickCreateUser.Invoke(data);
        });
    }
}
