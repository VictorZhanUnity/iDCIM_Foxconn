using UnityEngine;

public class CCTVManager : iDCIM_ModuleManager
{
    [Header(">>> WebAPI")]
    [SerializeField] CCTV_WebAPI webAPI;

    protected override void OnShowHandler()
    {
        webAPI.GetAllCCTVInfo();
    }

    protected override void OnCloseHandler()
    {
    }

    [ContextMenu("- [WebAPI] ���o�Ҧ�CCTV�]�Ƹ�T")]
    private void GetAllCCTVInfo() => webAPI.GetAllCCTVInfo();
}
