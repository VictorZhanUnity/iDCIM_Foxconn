using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// [資料項] 資產管理
/// </summary>
[Serializable]
public class Data_iDCIMAsset : IToolTipPanel_Data
{
    public string devicePath;
    public string deviceId;

    public string manufacturer;
    public string modelNumber;
    public InfoWithCOBie information;

    public string system => devicePath.Split("+")[5];
    public string deviceName => devicePath.Split(':')[1].Trim();

    [Header(">>> 設備模型")]
    public Transform model;
}
/// <summary>
/// [資料項] 機櫃Rack與其設備清單
/// </summary>
[Serializable]
public class Data_ServerRackAsset : Data_iDCIMAsset
{
    public string rackId;
    public string description;

    [Header(">>> 內容設備")]
    public List<Data_DeviceAsset> containers;

    /// <summary>
    /// 可用的RU空間每種尺吋大小 (需計算)
    /// </summary>
    public List<int> eachSizeOfAvailableRU { get; set; } = new List<int>();

    public Data_ServerRackAsset RefreshData(Data_ServerRackAsset source)
    {
        devicePath = source.devicePath;
        deviceId = source.deviceId;
        manufacturer = source.manufacturer;
        modelNumber = source.modelNumber;
        information = source.information;
        rackId = source.rackId;
        description = source.description;
        model = source.model;
        //重新整理，擷取機櫃裡設備資料，Model不為null的資料項 (就是場景上無此設備模型)
        containers = source.containers.Where(dataDevice => dataDevice.model != null).ToList();
        return this;
    }

    /// <summary>
    /// 儲存建立的空白RU Spacer
    /// </summary>
    public List<RackSpacer> availableRackSpacerList { get; set; } = new List<RackSpacer>();
    public void ShowAvailableRuSpacer() => availableRackSpacerList.ForEach(spacer => spacer.gameObject.SetActive(true));
    public void HideAvailableRuSpacer() => availableRackSpacerList.ForEach(spacer => spacer.gameObject.SetActive(false));

    public void RemoveAvailableRackSpacer(List<RackSpacer> rackSpacers)
    {
        availableRackSpacerList = availableRackSpacerList.Except(rackSpacers).ToList();
        rackSpacers.ForEach(rackSpacer => GameObject.Destroy(rackSpacer.gameObject));
    }

    /// <summary>
    ///顯示RU層數
    /// </summary>
    public List<RackSpacer> ShowRackSpacer(int ruIndex, int heightU)
    {
        int endRuIndex = ruIndex + heightU;
        List<RackSpacer> result = availableRackSpacerList.Where(rackSpacer => rackSpacer.RuLocation >= ruIndex && rackSpacer.RuLocation < endRuIndex).ToList();
        result.ForEach(rack => rack.isForceToShow = true);
        availableRackSpacerList.Except(result).ToList().ForEach(rackSpacer => rackSpacer.isForceToShow = false);
        return result;
    }

    #region [(">>> 計算資源使用率]
    // 使用數量
    public float _usageOfWatt = -1;
    public float usageOfWatt => (_usageOfWatt == -1) ? _usageOfWatt = containers.Sum(device => device.information.watt) : _usageOfWatt;
    public float _usageOfRU = -1;
    public float usageOfRU => (_usageOfRU == -1) ? _usageOfRU = containers.Sum(device => device.information.heightU) : _usageOfRU;
    public float _usageOfWeight = -1;
    public float usageOfWeight => (_usageOfWeight == -1) ? _usageOfWeight = containers.Sum(device => device.information.weight) : _usageOfWeight;
    // 百分比
    public float _percentOfWatt = -1;
    public float percentOfWatt => (_percentOfWatt == -1) ? _percentOfWatt = usageOfWatt / information.watt : _percentOfWatt;
    public float _percentOfRU = -1;
    public float percentOfRU => (_percentOfRU == -1) ? _percentOfRU = usageOfRU / information.heightU : _percentOfRU;
    public float _percentOfWeight = -1;
    public float percentOfWeight => (_percentOfWeight == -1) ? _percentOfWeight = usageOfWeight / information.weight : _percentOfWeight;
    /// 剩餘資源數量
    public float reaminOfWatt => information.watt - usageOfWatt;
    public float reaminOfWeight => information.weight - usageOfWeight;
    public float reaminOfRU => information.heightU - usageOfRU;
    #endregion
}
/// <summary>
/// [資料項] 設備資訊
/// </summary>
[Serializable]
public class Data_DeviceAsset : Data_iDCIMAsset
{
    public string containerId;
    public int rackLocation;
    public int state;
    public Data_ServerRackAsset rack { get; set; }
}

/// <summary>
/// [資料項] COBie資訊與長寬高
/// </summary>
[Serializable]
public class Information
{
    public float length;
    public float width;
    public float height;
    public int heightU;
    public float watt;
    public float weight;
}
[Serializable]
public class InfoWithCOBie : Information
{
    public string component_description;
    public string component_assetIdentifier;
    public string component_serialNumber;
    public string component_installationDate;
    public string component_tagName;
    public string component_warrantyDurationPart;
    public string component_warrantyDurationUnit;
    public string component_warrantyGuarantorLabor;
    public string component_warrantyStartDate;
    public string component_warrantyEndDate;
    public string document_inspection;
    public string document_handout;
    public string document_drawing;
    public string contact_company;
    public string contact_department;
    public string contact_email;
    public string contact_familyName;
    public string contact_givenName;
    public string contact_phone;
    public string contact_street;
    public string facility_name;
    public string facility_projectName;
    public string facility_siteName;
    public string equipment_supplier;
    public string floor_name;
    public string space_name;
    public string space_roomTag;
    public string system_category;
    public string system_name;
    public string type_category;
    public string type_expectedLife;
    public string type_manufacturer;
    public string type_modelNumber;
    public string type_name;
    public string type_replacementCost;
    public string type_accessibilityPerformance;
    public string type_shape;
    public string type_size;
    public string type_color;
    public string type_finish;
    public string type_grade;
    public string type_material;

    private Dictionary<string, string> cobieMap { get; set; }
    /// <summary>
    /// COBie資訊對照表 {COBie名稱, 值}
    /// </summary>
    public Dictionary<string, string> COBieMapData
    {
        get
        {
            cobieMap ??= StringHandler.ToClassInstanceVariableMap<string>(this);
            return cobieMap;
        }
    }
}