using UnityEngine;

namespace VictorDev.Common
{
    public abstract class ResourceHandler
    {
        /// <summary>
        /// Ū����r�ɮסA���ɦW�Y�i�A���ΰ��ɦW
        /// <para>+ �|�۰ʷj�M���|�GAssets/Resources/���U</para>
        /// </summary>
        public static string LoadStringFile(string fileName) => Resources.Load<TextAsset>(fileName).text;
    }
}
