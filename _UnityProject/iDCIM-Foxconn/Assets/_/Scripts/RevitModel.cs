using System.Collections.Generic;

public abstract class ReviteModel
{
    public string deviceId;
    public string system;

    public Dictionary<string, string> data = new Dictionary<string, string>();

    public string GetValue(string key) => data.ContainsKey(key) ? data[key] : "";
}