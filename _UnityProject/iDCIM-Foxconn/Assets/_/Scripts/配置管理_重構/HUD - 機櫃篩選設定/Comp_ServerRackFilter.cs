﻿using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.ColorUtils;
using VictorDev.Common;
using Random = UnityEngine.Random;

/// <summary>
/// [組件] 機櫃過濾
/// </summary>
public class Comp_ServerRackFilter : DeviceAssetDataReceiver
{
    [Header(">>> Dotween設定")]
    [SerializeField] private float minScale = 0.001f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease easeOut = Ease.OutBack;
    [SerializeField] private Ease easeIn = Ease.InQuad;

    public Color colorGood, colorNormal, colorBad;

    public List<Data_ServerRackAsset> dataRacks;

    /// <summary>
    /// 機櫃原始顏色
    /// </summary>
    private Color originalRackColor = ColorHandler.HexToColor(0x181818, 1);

    /// <summary>
    /// 目前所選的庫存設備項目
    /// </summary>
    private ListItem_Device_RE currentItem { get; set; }

    public override void ReceiveData(List<Data_ServerRackAsset> datas) => dataRacks = datas;

    [ContextMenu("- 顯示合適的機櫃")]
    private void ShowFilterResult()
    {
        dataRacks.ForEach(dataRack =>
        {
            InfoWithCOBie selectDeviceInfo = currentItem.data.information;

            bool isSuitable = (selectDeviceInfo.watt <= dataRack.reaminOfWatt) //剩餘電力
            && (selectDeviceInfo.weight <= dataRack.reaminOfWeight) //剩餘負重
            && dataRack.eachSizeOfAvailableRU.Any(size => size >= selectDeviceInfo.heightU); //可以空間尺吋

            if (isSuitable) dataRack.ShowAvailableRuSpacer();
            else dataRack.HideAvailableRuSpacer();

            ChangeRackHeight(dataRack, isSuitable);
            ChangeRackColor(dataRack, isSuitable);

            dataRack.containers.ForEach(dataDevice => dataDevice.model.GetComponent<Collider>().enabled = isSuitable);
        });
    }

    /// <summary>
    /// 依照是否符合條件而調整機櫃大小
    /// </summary>
    private void ChangeRackHeight(Data_ServerRackAsset data, bool isSuitable)
    {
        DOTween.Kill(data.model);
        data.model
            .DOScaleY(isSuitable ? 1 : minScale, isSuitable ? duration : duration * 0.5f)
            .SetEase(isSuitable ? easeOut : easeIn).SetDelay(Random.Range(0f, duration)).SetAutoKill(true);
    }
    /// <summary>
    /// 依照是否符合條件而調整機櫃顏色
    /// </summary>
    private void ChangeRackColor(Data_ServerRackAsset data, bool isSuitable)
    {
        int rackMaterialIndex = data.model.name.Contains("ATEN") ? 7 : 4;
        int rackHoleIndex = data.model.name.Contains("ATEN") ? 9 : 2;
        Material[] mats = data.model.GetComponent<MeshRenderer>().materials;

        for (int i = 0; i < mats.Length; i++)
        {
            Color color = mats[i].color;
            if (i == rackMaterialIndex) //當目前mat為機櫃外殼時處理
            {
                if (isSuitable == false) color = originalRackColor;
                else
                {
                    //根據filter選項來取得剩餘資源百分八
                    List<float> filterUsagePercentList = new List<float>();
                    if (isFilterWatt) filterUsagePercentList.Add(data.percentOfWatt);
                    if (isFilterWeight) filterUsagePercentList.Add(data.percentOfWeight);
                    if (isFilterRuSpace) filterUsagePercentList.Add(data.percentOfRU);

                    float percentage = filterUsagePercentList.Sum(value => value) / filterUsagePercentList.Count;
                    List<Tuple<float, Color>> levelColors = new List<Tuple<float, Color>>()
                    {
                        { new Tuple<float, Color> (0.6f, colorGood) } ,
                        { new Tuple<float, Color> (0.8f, colorNormal) } ,
                        { new Tuple<float, Color> (1f, colorBad) } ,

                    };
                    color = ColorHandler.GetColorFromPercentage(percentage, levelColors);
                }
            }

            color.a = isSuitable ? 0 : 1;
            if (isSuitable || i == rackHoleIndex) MaterialHandler.SetTransparentMode(mats[i]);
            else MaterialHandler.SetOpaqueMode(mats[i]);

            DOTween.Kill(mats[i]);
            mats[i].DOColor(color, duration).SetEase(isSuitable ? easeOut : easeIn).SetAutoKill(true);
        };
    }

    /// <summary>
    /// 進行機櫃條件過濾
    /// </summary>
    public void ToFilterRack(ListItem_Device_RE target)
    {
        currentItem = target.isOn ? target : null;
        if (target.isOn) ShowFilterResult();
        else RestoreAllRack();
        ToShow();
    }

    /// <summary>
    /// 復原所有機櫃樣式
    /// </summary>
    private void RestoreAllRack()
        => dataRacks.ForEach(data =>
        {
            ChangeRackHeight(data, true);
            ChangeRackColor(data, false);
            data.containers.ForEach(device => device.model.GetComponent<Collider>().enabled = true);
        });

    #region [>>> Show/Hide]
    public void ToShow()
    {
        gameObject.SetActive(true);
        ToggleRemainRuSpace.onValueChanged.AddListener((isOn) => ShowFilterResult());
        ToggleRemainWatt.onValueChanged.AddListener((isOn) => ShowFilterResult());
        ToggleRemainWeight.onValueChanged.AddListener((isOn) => ShowFilterResult());
    }

    public void ToClose()
    {
        gameObject.SetActive(false);
        ToggleRemainRuSpace.onValueChanged.RemoveAllListeners();
        ToggleRemainWatt.onValueChanged.RemoveAllListeners();
        ToggleRemainWeight.onValueChanged.RemoveAllListeners();
        RestoreAllRack();
    }
    #endregion

    #region [>>> Components]
    private Transform _uiObject { get; set; }
    private Transform uiObject => _uiObject ??= transform.GetChild(0);
    private Transform _VLayout { get; set; }
    private Transform VLayout => _VLayout ??= uiObject.transform.Find("Container").Find("VLayout");
    private Toggle _ToggleRemainRuSpace { get; set; }
    private Toggle ToggleRemainRuSpace => _ToggleRemainRuSpace ??= VLayout.Find("ToggleRemainRuSpace").GetComponent<Toggle>();
    private Toggle _ToggleRemainWatt { get; set; }
    private Toggle ToggleRemainWatt => _ToggleRemainWatt ??= VLayout.Find("ToggleRemainWatt").GetComponent<Toggle>();
    private Toggle _ToggleRemainWeight { get; set; }
    private Toggle ToggleRemainWeight => _ToggleRemainWeight ??= VLayout.Find("ToggleRemainWeight").GetComponent<Toggle>();
    private bool isFilterRuSpace => ToggleRemainRuSpace.isOn;
    private bool isFilterWatt => ToggleRemainWatt.isOn;
    private bool isFilterWeight => ToggleRemainWeight.isOn;
    #endregion
}
