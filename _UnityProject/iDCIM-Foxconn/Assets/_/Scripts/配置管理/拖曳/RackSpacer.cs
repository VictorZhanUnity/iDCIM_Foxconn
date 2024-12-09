using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// [�t�m�޲z] - RU�Ů�
/// </summary>
public class RackSpacer : MonoBehaviour
{
    public Data_ServerRackAsset dataRack { get; set; }

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
    private bool _isForceToShow { get; set; } = false;
    public bool isForceToShow
    {
        set
        {
            _isForceToShow = value;
            if (_isForceToShow) OnMouseEnter();
            else OnMouseExit();
        }
    }

    private void OnMouseExit()
    {
        container.gameObject.SetActive(_isForceToShow);
    }

    /// <summary>
    /// �O�_�񪺤U�]�Ƥj�p
    /// </summary>
    public bool isAbleToUpload(Data_DeviceAsset deviceAsset)
    {
        List<int> occupyRuIndex = Enumerable.Range(RuIndex, deviceAsset.information.heightU).ToList();
        List<int> availableRuIndex = dataRack.availableRackSpacerList.Select(rackSpacer => rackSpacer.RuIndex).ToList();
        return occupyRuIndex.All(occupy => availableRuIndex.Contains(occupy));
    }

    private Data_DeviceAsset currentDeviceData { get; set; }

    #region [Component]
    private Transform _parentRack { get; set; }
    public Transform parentRack => _parentRack ??= transform.parent;
    private Transform _container { get; set; }
    public Transform container => _container ??= transform.Find("Container");
    private TextMeshPro _txtRuIndex { get; set; }
    private TextMeshPro txtRuIndex => _txtRuIndex ??= container.GetChild(0).GetComponent<TextMeshPro>();
    private Transform tempDeviceModel { get; set; }
    #endregion
}
