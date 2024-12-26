using System;
using System.Collections;
using UnityEngine;
using VictorDev.Common;

public class FileLoader : SingletonMonoBehaviour<FileLoader>
{

    /// <summary>
    /// 讀取JSON檔
    /// <para>+ path: Resources資料夾裡的路徑，不用包含Resources與副檔名.json</para>
    /// <para>+ 例如: myJsonData</para>
    /// </summary>
    public static Coroutine LoadJsonFile(string path, Action<string> onSuccess, Action<string> onFailed) => Instance.LoadFile(path, onSuccess, onFailed);

    #region [讀取檔案]
    private Coroutine LoadFile(string path, Action<string> onSuccess, Action<string> onFailed) => StartCoroutine(LoadFileCoroutine(path.Trim(), onSuccess, onFailed));
    private IEnumerator LoadFileCoroutine(string path, Action<string> onSuccess, Action<string> onFailed)
    {
        // 加載 JSON 文件
        TextAsset jsonFile = Resources.Load<TextAsset>(path);
        if (jsonFile != null) onSuccess?.Invoke(jsonFile.text);
        else onFailed?.Invoke("JSON 文件讀取失敗！");
        yield return null;
    }
    #endregion

}
