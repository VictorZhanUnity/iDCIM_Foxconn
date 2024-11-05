using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using VictorDev.Common;

namespace VictorDev.Parser
{
    /// <summary>
    /// JSON資料解析 (使用static)
    /// <para>+ 不可被實例化</para>
    /// </summary>
    public abstract class JsonUtils
    {
        /// <summary>
        /// 解析Json字串資料
        /// <para>使用方式：JObject.Parse(jsonString)</para>
        /// </summary>
        /// <param name="jsonData">Json字串資料</param>
        /// <returns>單一Json物件：Dictionary物件[欄位名, 值]</returns>
        public static Dictionary<string, string> ParseJson(byte[] data)
            => ParseJson(JsonConvert.SerializeObject(StringHandler.Base64ToString(data)));

        /// <summary>
        /// 解析Json字串資料
        /// <para>使用方式：JObject.Parse(jsonString)</para>
        /// </summary>
        /// <param name="jsonData">Json字串資料</param>
        /// <returns>單一Json物件：Dictionary物件[欄位名, 值]</returns>
        public static Dictionary<string, string> ParseJson(string jsonData)
            => SetupJsonDictionaryItem(JObject.Parse(jsonData));

        /// <summary>
        /// 解析Json陣列資料 [ Json字串 ]
        /// <para>使用方式：JArray.Parse(jsonString)</para>
        /// </summary>
        /// <param name="jsonData">Json字串資料</param>
        /// <returns>Json物件陣列 - List[Dictionary[[欄位名, 值]]</returns>
        public static List<Dictionary<string, string>> ParseJsonArray(byte[] data)
            => ParseJsonArray(JsonConvert.SerializeObject(data));

        /// <summary>
        /// 解析Json陣列資料 [ Json字串 ]
        /// <para>使用方式：JArray.Parse(jsonString)</para>
        /// </summary>
        /// <param name="jsonData">Json字串資料</param>
        /// <returns>Json物件陣列 - List[Dictionary[[欄位名, 值]]</returns>
        public static List<Dictionary<string, string>> ParseJsonArray(string jsonData)
        {
            List<Dictionary<string, string>> resultDictList = new List<Dictionary<string, string>>();
            JArray jsonArray = JArray.Parse(jsonData);
            foreach (JObject jsonObject in jsonArray)
            {
                resultDictList.Add(SetupJsonDictionaryItem(jsonObject));
            }
            return resultDictList;
        }

        #region [>>> Private Functions]
        /// <summary>
        /// 設置Json Dictionary物件
        /// </summary>
        /// <param name="jsonObject">JObject.Parse後的資料</param>
        /// <returns>單一Json物件 Dictionary[string, string]</returns>
        private static Dictionary<string, string> SetupJsonDictionaryItem(JObject jsonObject)
        {
            Dictionary<string, string> resultJsonItem = new Dictionary<string, string>();
            foreach (JProperty property in jsonObject.Properties())
            {
                resultJsonItem[property.Name] = jsonObject[property.Name].ToString();
            }
            return resultJsonItem;
        }
        #endregion


        /// <summary>
        /// [待整合，整合或取代原有的Dictionary]
        /// 依據欄位路徑向JObject取值，並轉型為指定型態out
        /// <para>+ return：JObect是否具有目標路徑path</para>
        /// </summary>
        public static bool TryGetValueByPath<T>(JObject jObj, string path, out T result)
        {
            JToken token = jObj.SelectToken(path);
            bool isHaveValue = token != null;
            result = isHaveValue ? token.Value<T>() : default(T);
            return isHaveValue;
        }
    }
}



/**測試範例
    可以直接針對JObject進行SelectToken依據路徑取值Value，並且可以直接轉換指定的資料型態，例如：DateTime

  print("測試DateTime格式");

        var payload = new
        {
            timestamp = "2024-10-17T08:32:00Z",
            A = new
            {
                B = "2024-10-17T08:32:00"
            }
        };

        var jObj = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(payload));

        var t = jObj.SelectToken("timestamp").Value<DateTime>();
        var n = jObj.SelectToken("A.B").Value<DateTime>();

        print(t);
        print(t.ToLocalTime());
        print(n);
*/