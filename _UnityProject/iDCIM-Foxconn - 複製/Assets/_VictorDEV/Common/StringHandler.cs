using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VictorDev.Common
{
    public abstract class StringHandler
    {
        /// <summary>
        /// 設置文字大小(HTML)
        /// </summary>
        public static string SetFontSizeString(string str, int fontSize) => $"<size='{fontSize}'>{str}</size>";

        /// <summary>
        /// 解碼Base64 byte[] 轉成UTF8字串
        /// </summary>
        public static string Base64ToString(byte[] data)
        {
            string base64String = JsonConvert.SerializeObject(data).Trim('\"');
            byte[] byteArray = Convert.FromBase64String(base64String);
            // 將 byte[] 解碼為字符串
            return Encoding.UTF8.GetString(byteArray);
        }

        private static StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 將多個字串組在一起
        /// <para> + 使用StringBuilder更有效率</para>
        /// <para> + 直接用原始值，故不進行Trim()</para>
        /// </summary>
        public static string StringBuilderAppend(params string[] strValues)
        {
            sb.Clear();
            foreach (string strValue in strValues)
            {
                sb.Append(strValue);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 四捨五入到小數點後1位，若為0則不顯示小數點
        /// </summary>
        public static string FormatFloat(float value, int numOfDecimal = 1)
        {
            float divide = Mathf.Pow(10f, numOfDecimal);

            // 四捨五入到小數點後1位
            float roundedValue = Mathf.Round(value * divide) / divide;

            // 如果小數點後{numOfDecimal}位為0，則僅顯示整數部分，否則顯示一位小數
            if (Mathf.Approximately(roundedValue % 1, 0))
            {
                return ((int)roundedValue).ToString();
            }
            else
            {
                return roundedValue.ToString($"F{numOfDecimal}");
            }
        }

        /// <summary>
        /// 產生隨機的英文字母，組成指定長度的字串內容
        /// </summary>
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            System.Random random = new System.Random();
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        /// <summary>
        /// 產生隨機的數字，組成指定長度的字串內容
        /// </summary>
        public static string GenerateRandomNumberString(int length, int min = 0, int max = 9)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += Random.Range(min, max + 1).ToString();
            }
            return result;
        }


        /// <summary>
        /// 將目標類別實例裡的所有變數名稱，轉成字典對照表 {變數名稱, 變數值}
        /// <para>+ 適用於COBie資訊的UI組件指派</para>
        /// </summary>
        public static Dictionary<string, T> ToClassInstanceVariableMap<T>(object target)
        {
            Dictionary<string, T> result = new Dictionary<string, T>();

            // 使用反射獲取所有公有和私有的欄位
            FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                // 檢查欄位是否為字串類型
                if (field.FieldType == typeof(string))
                {
                    string name = field.Name;
                    object value = field.GetValue(target);
                    result[name] = (T)value;
                }
            }
            return result;
        }

        /// <summary>
        /// 是否為IPv4格式
        /// </summary>
        public static bool IsIPv4Format(string ipAddress)
        {
            string pattern = @"^((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])$";
            return Regex.IsMatch(ipAddress, pattern);
        }
    }
}
