using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// [�t�m�޲z] - RU�Ů�
/// </summary>
public class RackSpacer : MonoBehaviour
{
    public Data_ServerRackAsset dataRack;

    public int RuIndex
    {
        get => int.Parse(txtRuIndex.text);
        set
        {
            txtRuIndex.SetText(value.ToString());
            name = $"U {value}";
        }
    }

    private void OnMouseEnter()
    {
        container.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        container.gameObject.SetActive(container.childCount > 1);
    }

    /// <summary>
    /// �����W�[�]��
    /// </summary>
    public void CancellTempDevice()
    {
        if (tempDeviceModel != null)
        {
            tempDeviceModel.transform.parent.gameObject.SetActive(false);
            Destroy(tempDeviceModel.gameObject);
            tempDeviceModel = null;
        }
    }
    /// <summary>
    /// �T�{�W�[�]�ơA�N�]�Ƽҫ����ܾ��d���󩳤U�A�A�R���ۨ�RackSpacer
    /// </summary>  
    public void ConfirmUploadDevice()
    {
        tempDeviceModel.transform.parent = parentRack;
        Destroy(this.gameObject);
        dataRack.availableRackSpacerList.Remove(this);
    }

    /// <summary>
    /// �إ߼Ȯɪ��W�[�]�Ƽҫ�
    /// </summary>
    public void CreateTempDevice(Transform deviceModel)
    {
        CancellTempDevice();
        tempDeviceModel = Instantiate(deviceModel, container);
        tempDeviceModel.localPosition = Vector3.zero;
        tempDeviceModel.localRotation = Quaternion.Euler(0, 90, 0);
        tempDeviceModel.gameObject.SetActive(true);
        tempDeviceModel.DOLocalMove(Vector3.zero, 0.3f).From(Vector3.left * 0.3f).SetEase(Ease.OutQuad).SetAutoKill(true);
    }

    #region [Component]
    private Transform _parentRack { get; set; }
    public Transform parentRack => _parentRack ??= transform.parent;
    private Transform _container { get; set; }
    private Transform container => _container ??= transform.Find("Container");
    private TextMeshPro _txtRuIndex { get; set; }
    private TextMeshPro txtRuIndex => _txtRuIndex ??= container.GetChild(0).GetComponent<TextMeshPro>();
    private Transform tempDeviceModel { get; set; }
    #endregion
}
