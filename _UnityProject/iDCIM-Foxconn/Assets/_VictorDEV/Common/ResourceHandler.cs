using UnityEngine;

namespace VictorDev.Common
{
    public abstract class ResourceHandler
    {
        /// <summary>
        /// 讀取文字檔案，僅檔名即可，不用副檔名
        /// <para>+ 會自動搜尋路徑：Assets/Resources/底下</para>
        /// </summary>
        public static string LoadStringFile(string fileName) => Resources.Load<TextAsset>(fileName).text;
    }
}
