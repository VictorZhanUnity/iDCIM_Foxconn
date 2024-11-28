using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// [配置管理] - RU空格
/// </summary>
public class RackSpacer : MonoBehaviour
{
    #region [Component]
    private Transform _parentRack { get; set; }
    public Transform parentRack => _parentRack ??= transform.parent;
    private TextMeshPro _txtRuIndex { get; set; }
    private TextMeshPro txtRuIndex => _txtRuIndex ??= transform.GetChild(0).GetComponent<TextMeshPro>();
    private Transform _container { get; set; }
    private Transform container => _container ??= transform.Find("Container");
    private Transform tempDeviceModel { get; set; }
    #endregion

    public int RuIndex
    {
        get => int.Parse(txtRuIndex.text);
        set
        {
            txtRuIndex.SetText(value.ToString());
            name = $"U {value}";
        }
    }

    /// <summary>
    /// 取消上架設備
    /// </summary>
    public void CancellTempDevice()
    {
        if (tempDeviceModel != null)
        {
            Destroy(tempDeviceModel.gameObject);
            tempDeviceModel = null;
        }
    }
    /// <summary>
    /// 確認上架設備，將設備模型移至機櫃物件底下，再刪除自身RackSpacer
    /// </summary>  
    public void ConfirmUploadDevice()
    {
        tempDeviceModel.transform.parent = parentRack;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 建立暫時的上架設備模型
    /// </summary>
    public void CreateTempDevice(Transform deviceModel)
    {
        CancellTempDevice();
        tempDeviceModel = Instantiate(deviceModel, container);
        tempDeviceModel.localPosition = Vector3.zero;
        tempDeviceModel.rotation = Quaternion.Euler(Vector3.zero);
        tempDeviceModel.DOLocalMove(Vector3.zero, 0.3f).From(Vector3.left * 0.3f).SetEase(Ease.OutQuad).SetAutoKill(true);
    }
}
