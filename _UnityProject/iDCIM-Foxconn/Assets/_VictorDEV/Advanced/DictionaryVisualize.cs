using System;
using System.Collections.Generic;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 字典視覺化，T為內容值，可以為string，也可以為字典
    /// <para>+ 利用泛型，達到巢狀資料結的存儲</para>
    /// </summary>
    [Serializable]
    public class DictionaryVisualize<K, V>
    {
        public K key;
        public V value;

        public DictionaryVisualize(K key, V value)
        {
            this.key = key;
            this.value = value;
        }

        /// <summary>
        /// 解析JSON字典資料
        /// </summary>
        public static List<DictionaryVisualize<K, V>> Parse(Dictionary<K, V> jsonDict)
        {
            List<DictionaryVisualize<K, V>> result = new List<DictionaryVisualize<K, V>>();

            foreach (var data in jsonDict)
            {
                result.Add(new DictionaryVisualize<K, V>(data.Key, data.Value));
            }
            return result;
        }
    }
}
