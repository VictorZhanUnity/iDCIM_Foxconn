using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RevitUtils;

public class DeviceModelManager : MonoBehaviour
{
    [Header(">>> 欲顯示的模型物件")]
    [SerializeField] private ModelDisplayConfiguration modelForDisplay;

    [Header(">>> 庫存設備模型")]
    [SerializeField] private List<DeviceModel> serverModels;
    [SerializeField] private List<DeviceModel> switchModels;
    [SerializeField] private List<DeviceModel> routerModels;

    private List<DeviceModel> _allDeviceModel { get; set; }
    public List<DeviceModel> allDeviceModel => _allDeviceModel = serverModels.Union(switchModels).Union(routerModels).ToList();

   
    #region [ContextMenu]
    [ContextMenu("- [Parent] 根據Keywords尋找目標物件")]
    public void FindTargetObjects()
    {
        modelForDisplay.FindTargetObjects();
    }
    [ContextMenu("- 群組化設備")]
    private void FilterDevices()
    {
        serverModels = modelForDisplay.modelsList.Where(model => model.name.Contains("Server", StringComparison.OrdinalIgnoreCase)).GroupBy(model =>
        {
            string deviceName = RevitHandler.GetDevicePath(model.name).Split(":")[1];
            string splitKeyword = deviceName.Contains("Brocade", StringComparison.OrdinalIgnoreCase) ? "sim" : "Server";
            splitKeyword = $"-{splitKeyword}-";
            deviceName = deviceName.Split(splitKeyword)[0].Trim();
            return deviceName;
        }).Select(group => new DeviceModel(group.Key, group.First().transform)).OrderBy(model => model.name).ToList();

        switchModels = modelForDisplay.modelsList.Where(model => model.name.Contains("Switch", StringComparison.OrdinalIgnoreCase)).GroupBy(model =>
        {
            string deviceName = RevitHandler.GetDevicePath(model.name).Split(":")[1];
            string splitKeyword = deviceName.Contains("Juniper", StringComparison.OrdinalIgnoreCase)
            || model.name.Contains("Z系列", StringComparison.OrdinalIgnoreCase)
            || model.name.Contains("Brocade", StringComparison.OrdinalIgnoreCase)
            ? "Switch" : "-6U+";

            splitKeyword = splitKeyword.Equals("-6U+") ? splitKeyword : $"-{splitKeyword}-";

            deviceName = deviceName.Split(splitKeyword)[0].Trim();
            return deviceName;
        }).Select(group => new DeviceModel(group.Key, group.First().transform)).OrderBy(model => model.name).ToList();

        routerModels = modelForDisplay.modelsList.Where(model => model.name.Contains("Router", StringComparison.OrdinalIgnoreCase)).GroupBy(model =>
        {
            string deviceName = RevitHandler.GetDevicePath(model.name).Split(":")[1];
            string splitKeyword = "Router";
            splitKeyword = $"-{splitKeyword}-";
            deviceName = deviceName.Split(splitKeyword)[0].Trim();
            return deviceName;
        }).Select(group => new DeviceModel(group.Key, group.First().transform)).OrderBy(model => model.name).ToList();
    }

    [Serializable]
    public struct DeviceModel
    {
        public string name;
        public Transform model;
        public DeviceModel(string name, Transform model)
        {
            this.name = name;
            this.model = model;
        }
    }

    #endregion
}
