using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.DoTweenUtils;
using static VictorDev.RevitUtils.RevitHandler;


/// <summary>
/// [資產管理] 設備資訊清單(設備 or 機櫃)
/// </summary>
public class DeviceInfoPanel : MonoBehaviour
{
    [Header(">>> UI組件")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ListItem_COBie listItemPrefab;
    [SerializeField] private List<GameObject> deviceImgTags;
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private TextMeshProUGUI txtDeviceName, txtDeviceID, txtManufacturer, txtModelNumber, txtDescription, txtRackLocation;

    /// <summary>
    /// 顯示資料(From設備清單)
    /// </summary>
    public void ShowData(ListItem_Device target)
    {
        UpdateUI(target.data);
    }
    /// <summary>
    /// 顯示資料(From機櫃面板RU設備清單)
    /// </summary>
    public void ShowData(DeviceRUItem target) => UpdateUI(target.data);
    /// <summary>
    /// 顯示資料(From機櫃面板)
    /// </summary>
    public void ShowData(Data_iDCIMAsset dataRack) => UpdateUI(dataRack);

    /// <summary>
    /// 更新資料顯示畫面
    /// </summary>
    private void UpdateUI(Data_iDCIMAsset data)
    {
        txtDeviceID.SetText(data.devicePath);
        txtDeviceName.SetText(data.deviceName);

        deviceImgTags.ForEach(tag=> tag.SetActive(tag.name.Contains(data.system)));

        txtManufacturer.SetText(data.information.type_manufacturer);
        txtModelNumber.SetText(data.information.type_modelNumber);

        txtDescription.gameObject.SetActive(false);
        txtRackLocation.gameObject.SetActive(false);
        if (data is Data_ServerRackAsset dataRack)
        {
            txtDescription?.SetText(dataRack.description);
            txtDescription.gameObject.SetActive(true);
        }
        else if (data is Data_DeviceAsset dataDevice)
        {
            txtRackLocation?.SetText($"第{dataDevice.rackLocation.ToString()}U");
            txtRackLocation.gameObject.SetActive(true);
        }

        BuildCOBieList(data);

        doTweenFadeController.ToShow(true);
    }

    public void Close() => doTweenFadeController.ToHide();

    /// <summary>
    /// 建立COBie資料清單
    /// </summary>
    private void BuildCOBieList(Data_iDCIMAsset data)
    {
        //移除項目
        ObjectPoolManager.PushToPool<ListItem_COBie>(scrollRect.content);

        data.information.COBieMapData.ToList().ForEach(data =>
        {
            ListItem_COBie item = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_COBie>(listItemPrefab, scrollRect.content);
            item.ShowData(data.Key, data.Value);
        });
        scrollRect.verticalNormalizedPosition = 1;
    }
}
