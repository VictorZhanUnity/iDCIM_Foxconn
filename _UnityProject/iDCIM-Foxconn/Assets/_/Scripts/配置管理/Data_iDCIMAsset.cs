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
    public string system => devicePath.Split("+")[5];

    public string manufacturer;
    public string modelNumber;
    public InfoWithCOBie information;

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

    [Header(">>> 計算資源使用率")]
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

    [Header(">>> 內容設備")]
    public List<Data_DeviceAsset> containers;
}
/// <summary>
/// [資料項] 設備資訊
/// </summary>
[Serializable]
public class Data_DeviceAsset : Data_iDCIMAsset, INotifyData
{
    public string containerId;
    public int rackLocation;
    public int state;
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
    public float heightU;
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