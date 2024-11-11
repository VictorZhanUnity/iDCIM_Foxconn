using System;
using System.Collections.Generic;

[Serializable]
public class Data_RTSP
{
    public string name;
    public string devicePath;
   
    public DeviceInformation deviceInformation;

    [Serializable]
    public class DeviceInformation
    {
        public string rtsp_connection_string;
        public string description;
    }
}


[Serializable]
public class SearchDeviceFormat
{
    public bool isSuccess;

    public string state;
    public DataPages data;

    [Serializable]
    public class DataPages
    {
        public int currentPageIndex;
        public int totalPage;
        public List<Data_RTSP> pageData;
    }
}
