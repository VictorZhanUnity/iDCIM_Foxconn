using UnityEngine;
using UnityEngine.Events;

public class AccessDoor_InfoPanel : InfoPanel<Data_AccessRecord>
{
    [Space(20)]
    [Header(">>> ÂIÀ»¥þ¿Ã¹õ«ö¶s®ÉInvoke")]
    public UnityEvent<AccessDoor_InfoPanel> onClickZoomButtn = new UnityEvent<AccessDoor_InfoPanel>();


    protected override void OnAwakeHandler()
    {

    }


    protected override void OnShowDataHandler(Data_AccessRecord data)
    {

    }

    protected override void OnCloseHandler(Data_AccessRecord data)
    {
    }

}
