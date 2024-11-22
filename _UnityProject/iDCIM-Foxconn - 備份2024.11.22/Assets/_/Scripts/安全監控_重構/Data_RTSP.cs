using System;

[Serializable]
public class Data_RTSP : ILandmarkData
{
    public string name;
    public string devicePath;
    /// <summary>
    /// 編號
    /// </summary>
    public string idNumber => name.Split('-')[1];

    public string DevicePath => devicePath;

    public DeviceInformation deviceInformation;

    [Serializable]
    public class DeviceInformation
    {
        public string rtsp_connection_string;
        public string description;
    }
}