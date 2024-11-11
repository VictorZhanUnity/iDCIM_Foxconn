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

    [ContextMenu("- [WebAPI] 取得所有CCTV設備資訊")]
    private void GetAllCCTVInfo() => webAPI.GetAllCCTVInfo();
}
