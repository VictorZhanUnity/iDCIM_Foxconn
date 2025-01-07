using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VictorDev.Advanced
{
    public abstract class EnumHandler
    {
        /// <summary>
        /// 將字串轉化為指定enum類型
        /// </summary>
        public static E Parse<E>(string strValue)
        {
            E result = default;
            try
            {
                result = (E)Enum.Parse(typeof(E), strValue);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($">>> EnumHandler.Parse: {ex.ToString()} / {strValue}");
            }
            return result;
        }

        /// <summary>
        /// 從指定enum裡取亂數值
        /// </summary>
        /// <param name="startIndex">從第幾個開始亂數取值</param>
        public static E GetRandomFromEnum<E>(int startIndex = 0)
        {
            // 獲取 enum 的所有值
            Array ary = Enum.GetValues(typeof(E));
            // 根據隨機索引選擇 enum 值
            return (E)ary.GetValue(Random.Range(startIndex, ary.Length));
        }

        /// <summary>
        /// 將指定Enum類型裡的值，轉換成字串List
        /// </summary>
        public static List<string> SetEnumToStringList<E>() where E : Enum
           => new List<string>(Enum.GetNames(typeof(E)));
        
        /// 依照字串取得該Enum項目的Value
        public static int GetValueWithEnumString<T>(string enumString) where T : struct, Enum
        {
            if (Enum.TryParse<T>(enumString, true, out var result))
            {
                return Convert.ToInt32(result); // 返回整數值
            }
            return -1; // 如果無法解析，返回 -1
        }
    }
}
