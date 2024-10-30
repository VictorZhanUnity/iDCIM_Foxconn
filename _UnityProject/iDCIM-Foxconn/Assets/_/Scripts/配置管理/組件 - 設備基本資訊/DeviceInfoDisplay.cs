using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// [資產管理] 顯示資產設備基本資訊
/// </summary>
public class DeviceInfoDisplay : MonoBehaviour
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_iDCIMAsset data;

    [Header(">>> UI組件")]
    [SerializeField] private ListItem_COBie listItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TextMeshProUGUI txtDeviceName, txtSystem, txtManufacturer, txtModelNumber;

    private List<TextMeshProUGUI> txtList { get; set; }

    public void ShowData(Data_iDCIMAsset data)
    {
        this.data = data;
        UpdateUI();
    }

    private void UpdateUI()
    {
        txtDeviceName.SetText(data.deviceName);
        txtSystem.SetText(data.system);
        txtManufacturer.SetText(data.manufacturer);
        txtModelNumber.SetText(data.modelNumber);

        txtList ??= new List<TextMeshProUGUI>() { txtDeviceName, txtSystem, txtManufacturer, txtModelNumber };
        DotweenHandler.ToBlink(txtList, null, 0.1f, 0.3f, true);

        if (scrollRect != null)
        {
            //清除COBie清單
            ObjectPoolManager.PushToPool<ListItem_COBie>(scrollRect.content);

            // 建立COBie清單
            data.information.COBieMap.ToList().ForEach(keyPair =>
            {
                ListItem_COBie item = ObjectPoolManager.GetInstanceFromQueuePool(listItemPrefab, scrollRect.content);
                item.ShowData(keyPair.Key, keyPair.Value);
            });
            scrollRect.verticalNormalizedPosition = 1;
        }
    }
}
