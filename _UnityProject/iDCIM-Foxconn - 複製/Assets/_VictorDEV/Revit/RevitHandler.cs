﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using VictorDev.Common;

namespace VictorDev.RevitUtils
{
    /// <summary>
    /// Revit相關處理
    /// </summary>
    public abstract class RevitHandler
    {
        /// <summary>
        /// 從模型名稱[] 裡取得deviceId
        /// </summary>
        public static string GetDeviceID(string modelName)
        {
            // 创建正则表达式来匹配方括号内的内容
            Regex regex = new Regex(@"\[(.*?)\]");

            // 通过正则表达式匹配
            Match match = regex.Match(modelName);

            // 提取匹配到的内容，match.Groups[0] = "[值]"
            if (match.Success) return match.Groups[1].Value;
            else return null;
        }

        /// <summary>
        /// 從模型上擷取DeviceName
        /// </summary>
        public static string GetDeviceNameFromModel(string modelName)
            => RevitHandler.GetDeviceID(modelName).Split(":")[1].Trim();


        /// <summary>
        /// COBie欄位對照表 (From冠宇 2024.10.25)
        /// </summary>
        private static Dictionary<string, string> cobieColumnTable { get; set; } = new Dictionary<string, string>()
        {
             {"component_description", "描述/設備名稱" },
             {"component_assetIdentifier", "FM資產識別字" },
             {"component_serialNumber", "產品序號" },
             {"component_installationDate", "安裝日期" },
             {"component_tagName", "設備編碼" },
             {"component_warrantyDurationPart", "保固時間" },
             {"component_warrantyDurationUnit", "保固時間單位" },
             {"component_warrantyGuarantorLabor", "保固廠商" },
             {"component_warrantyStartDate", "保固開始時間" },
             {"component_warrantyEndDate", "保固結束時間" },
             {"document_inspection", "保養檢查表" },
             {"document_handout", "使用手冊" },
             {"document_drawing", "圖說" },
             {"contact_company", "聯絡單位公司" },
             {"contact_department", "聯絡單位部門" },
             {"contact_email", "聯絡人Email" },
             {"contact_familyName", "聯絡人姓氏" },
             {"contact_givenName", "聯絡人名字" },
             {"contact_phone", "聯絡人電話" },
             {"contact_street", "聯絡人地址" },
             {"facility_name", "專案棟別名稱" },
             {"facility_projectName", "專案名稱" },
             {"facility_siteName", "項目地點" },
             {"equipment_supplier", "供應廠商" },
             {"floor_name", "樓層名稱/所屬樓層" },
             {"space_name", "項目地點" },
             {"space_roomTag", "空間名稱" },
             {"system_category", "系統類別 DCS、DCN" },
             {"system_name", "系統名稱" },
             {"type_category", "OmniClass編碼" },
             {"type_expectedLife", "使用年限" },
             {"type_manufacturer", "製造廠商" },
             {"type_modelNumber", "產品型號" },
             {"type_name", "設備品類名稱" },
             {"type_replacementCost", "設備售價" },
             {"type_accessibilityPerformance", "無障礙功能" },
             {"type_shape", "形狀" },
             {"type_size", "尺寸" },
             {"type_color", "顏色" },
             {"type_finish", "完成面" },
             {"type_grade", "設備分級" },
             {"type_material", "材質" },
        };
        /// <summary>
        /// 取得COBie欄位中文名稱
        /// </summary>
        public static string GetCOBieColumnName_ZH(string key) => cobieColumnTable.Keys.Contains(key) ? cobieColumnTable[key] : null;

        #region [>>>>>>>>> WebAPI 資料項架構]
        //資料解析運算是快的，不到一秒，只不過呈現在Inspector上會較慢些
        /// <summary>
        /// [資料項] 資產管理
        /// </summary>
        [Serializable]
        public class Data_iDCIMAsset : IToolTipPanel_Data
        {
            public string devicePath;
            public string deviceId;
            public string system => devicePath.Split("+")[5];

            public string manufacturer;
            public string modelNumber;
            public InfoWithCOBie information;

            public string deviceName => devicePath.Split(':')[1].Trim();
        }
        /// <summary>
        /// [資料項] 機櫃Rack與其設備清單
        /// </summary>
        [Serializable]
        public class Data_ServerRackAsset : Data_iDCIMAsset
        {
            public string rackId;
            public string description;
            public List<Data_DeviceAsset> containers;

            #region [>>> 計算資源使用率]
            private float _usageOfWatt = -1;
            public float usageOfWatt
            {
                get
                {
                    if (_usageOfWatt == -1) _usageOfWatt = containers.Sum(device => device.information.watt);
                    return _usageOfWatt;
                }
            }
            private float _usageOfRU = -1;
            public float usageOfRU
            {
                get
                {
                    if (_usageOfRU == -1) _usageOfRU = containers.Sum(device => device.information.heightU);
                    return _usageOfRU;
                }
            }
            private float _usageOfWeight = -1;
            public float usageOfWeight
            {
                get
                {
                    if (_usageOfWeight == -1) _usageOfWeight = containers.Sum(device => device.information.weight);
                    return _usageOfWeight;
                }
            }
            #endregion
        }
        /// <summary>
        /// [資料項] 設備資訊
        /// </summary>
        [Serializable]
        public class Data_DeviceAsset : Data_iDCIMAsset
        {
            public string containerId;
            public int rackLocation;
            public int state;
        }

        /// <summary>
        /// [資料項] COBie資訊與長寬高
        /// </summary>
        [Serializable]
        public class Information
        {
            public float length;
            public float width;
            public float height;
            public float heightU;
            public float watt;
            public float weight;
        }
        [Serializable]
        public class InfoWithCOBie : Information
        {
            public string component_description;
            public string component_assetIdentifier;
            public string component_serialNumber;
            public string component_installationDate;
            public string component_tagName;
            public string component_warrantyDurationPart;
            public string component_warrantyDurationUnit;
            public string component_warrantyGuarantorLabor;
            public string component_warrantyStartDate;
            public string component_warrantyEndDate;
            public string document_inspection;
            public string document_handout;
            public string document_drawing;
            public string contact_company;
            public string contact_department;
            public string contact_email;
            public string contact_familyName;
            public string contact_givenName;
            public string contact_phone;
            public string contact_street;
            public string facility_name;
            public string facility_projectName;
            public string facility_siteName;
            public string equipment_supplier;
            public string floor_name;
            public string space_name;
            public string space_roomTag;
            public string system_category;
            public string system_name;
            public string type_category;
            public string type_expectedLife;
            public string type_manufacturer;
            public string type_modelNumber;
            public string type_name;
            public string type_replacementCost;
            public string type_accessibilityPerformance;
            public string type_shape;
            public string type_size;
            public string type_color;
            public string type_finish;
            public string type_grade;
            public string type_material;

            private Dictionary<string, string> cobieMap { get; set; }
            /// <summary>
            /// COBie資訊對照表 {COBie名稱, 值}
            /// </summary>
            public Dictionary<string, string> COBieMapData
            {
                get
                {
                    cobieMap ??= StringHandler.ToClassInstanceVariableMap<string>(this);
                    return cobieMap;
                }
            }
        }
        #endregion

        //========================================================================================================


        /// <summary>
        /// 透過DCS、DCN設備的deviceId，取得其設備的高度U
        /// </summary>
        public static int GetHightUFromDeviceID(string deviceId)
        {
            string result = deviceId.Split(':')[0];
            return int.Parse(result.Split('-')[1]);
        }

        public static int GetHightUFromPhotoName(string photoName)
        {
            string result = photoName.Split(".rvt")[0];
            string[] ary = result.Split('-');

            int heightU;
            heightU = int.Parse(ary[ary.Length - 1].Substring(0, 1));

            return heightU;
        }

        /// <summary>
        /// 透過deviceId抓取得其設備的種類(DCS/DCN)
        /// </summary>
        public static string GetSystemTypeFromDeviceID(string deviceId)
        {
            string[] types = { enumSystemType.DCS.ToString(), enumSystemType.DCN.ToString() };

            foreach (string deviceType in types)
            {
                if (deviceId.Contains(deviceType)) return deviceType;
            }
            return "";
        }

        /// <summary>
        /// 從deviceId抓取DCS機櫃的型號(Brocade-7X-1)
        /// deviceId: "HWACOM+TPE+IDC+FL1+1+DCS++Server-1: Brocade-7X-1+98",
        /// </summary>
        public static string GetDCSModelNumberFromDeviceID(string deviceId)
        {
            string result = deviceId.Split(":")[1];
            return result.Split("+")[0].Trim();
        }

        private static string RegHandler(string deviceId, string pattern)
        {
            Match match = Regex.Match(deviceId, pattern);
            return (match.Success) ? (match.Groups[1].Value) : "";
        }

        /// <summary>
        /// 以機櫃U換算實際高度座標
        /// </summary>
        /// <param name="rackLocationU"></param>
        /// <returns></returns>
        public static Vector3 GetPositionFromRackU(int rackLocationU)
            => new Vector3(0, HeightOfRU * rackLocationU, 0);

        /// <summary>
        /// 每RU單位的高度
        /// </summary>
        public static float HeightOfRU
        {
            get
            {
                float posTop = 1.933f;  //機櫃頂部座標
                float posBottom = 0.08952563f;  //機櫃底部座標
                float eachU = (posTop - posBottom) / 42;    //總共42U
                return eachU;
            }
        }
        /// <summary>
        /// 每RU單位的寬度
        /// </summary>
        public static float WidthOfRU => 47.61683940887451f;
        /// <summary>
        /// 每RU單位的深度
        /// </summary>
        public static float LengthOfRU => 64.23128247261047f;
        /*
                /// <summary>
                /// 比對材質名稱Dictionary，建立DCS/DCN模型
                /// </summary>
                /// <param name="soDCS">ScriptableObject</param>
                /// <param name="textureDictionary">設備材質Dictionary</param>
                /// <param name="prefab">設備Prefab</param>
                /// <param name="container">放在哪個容器</param>
                public static Transform CreateDeviceFromDict(IDeviceSizeInfo soData, Dictionary<string, Texture> textureDictionary, Transform prefab, Transform rackContainer)
                {
                    Transform result = null;
                    string deviceType = GetDCSModelNumberFromDeviceID(soData.deviceId);
                    // 建立DCR內的設備
                    if (textureDictionary.ContainsKey(deviceType))
                    {
                        result = ObjectPoolManager.GetInstanceFromQueuePool(prefab, rackContainer);
                        MeshRenderer meshRenderer = result.GetComponent<MeshRenderer>();
                        meshRenderer.material.mainTexture = textureDictionary[deviceType];

        #if true // 關閉材質反光、陰影
                        meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                        meshRenderer.receiveShadows = false;
                        // 取得材質
                        Material material = meshRenderer.material;
                        material.color = Color.white;
                        if (material != null)
                        {
                            // 如果是標準着色器，設置金屬度和光澤度為0
                            if (material.shader.name == "Standard")
                            {
                                material.SetFloat("_Metallic", 0f);
                                material.SetFloat("_Glossiness", 0f);
                            }
                        }
        #endif

                        result.name = GetGameObjectNameFormat(soData.deviceId);

                        //依照給的長寬高來設置尺吋
                        result.localScale = new Vector3(soData.width - 13f, soData.height - 0.5f, soData.length) * 0.01f;      // 高度-0.5f微調，避免重疊； 單位除100

                        //依照給的位置U來設置在機櫃裡的高度位置
                        Vector3 pos = GetPositionFromRackU(soData.rackLocation);
                        pos.y += result.localScale.y * 0.5f; //物件Pivot為中心點，所以再加上自身高度*0.5f
                        pos.z = -0.53f + result.localScale.z * 0.5f;      // 機櫃口座標0.58，減掉物件自身長度*0.5f
                        result.transform.localPosition = pos;
                        result.transform.localRotation = Quaternion.Euler(0, 180, 0);  //轉向
                    }
                    else Debug.Log($"沒有材質圖：{deviceType}");
                    return result;
                }
        */
        /// <summary>
        /// 設定名稱：[類型] 型號 
        /// </summary>
        public static string GetGameObjectNameFormat(string deviceId) => $"[{GetSystemTypeFromDeviceID(deviceId)}] {GetDCSModelNumberFromDeviceID(deviceId)}";

        public static string GetDeviceModelName(string deviceName)
        {
            string result = deviceName.Split("[")[0];
            string[] str = result.Split("_");
            result = str[2].Substring(0, str[2].Length - 4); //去掉-6U
            return result;
        }

        /// <summary>
        /// 電氣設備_Schneider-RACK_Schneider-RACK_22[NTU+TPE+EE++Schneider-RACK: Schneider-RACK+48]
        /// 資料裝置_Server_Dell-PowerEdge系列-MX9116n-Server-2U_2[NTU+TPE+EE++Server: Dell-PowerEdge系列-MX9116n-Server-2U+66]
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static string GetRackModelName(string deviceName)
        {
            string result = deviceName.Split("[")[0];
            string[] str = result.Split("_");
            return str[2];
        }

        public static string GetDeviceModelSystem(string deviceName)
        {
            string result = deviceName.Split("[")[0];
            return result.Split("_")[1];
        }

        public static int GetDeviceModelHeightU(string deviceName)
        {
            string result = deviceName.Split("[")[0];
            string[] str = result.Split("-");
            result = str[str.Length - 1].Split("_")[0];
            return int.Parse(result.Split("U")[0]);
        }

        #region [>>> COBie欄位對照表]

        /// <summary>
        /// 將COBie字典資訊分類成各大項目
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> COBie_ClassfieldDict(Dictionary<string, string> sourceDict)
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>()
            {
                {"模型資訊", COBie_GetDictWithKeyWord(sourceDict, true, "COBie", "Other")},
                {"設備資訊", COBie_GetDictWithKeyWord(sourceDict, false, "System", "Component", "Equipment")},
                {"設備品類", COBie_GetDictWithKeyWord(sourceDict, false,  "Type")},
                {"位置區域", COBie_GetDictWithKeyWord(sourceDict, false, "Facility", "Floor", "Space")},
                {"聯絡資訊", COBie_GetDictWithKeyWord(sourceDict, false, "Contact")},
                {"圖資鏈結", COBie_GetDictWithKeyWord(sourceDict, false, "Document")},
            };
            return result;
        }

        /// <summary>
        /// 依照關鍵字取值，存成Dictionary進行回傳
        /// </summary>
        /*  private static Dictionary<string, string> COBie_GetDictWithKeyWord(Dictionary<string, string> sourceDict, params string[] keyWords)
          {
              Dictionary<string, string> result = new Dictionary<string, string>();
              foreach (string keyName in sourceDict.Keys)
              {
                  foreach (string key in keyWords)
                  {
                      if (keyName.Contains(key))
                      {
                          result[keyName] = sourceDict[keyName];
                      }
                  }
              }
              return result;
          }*/

        private static Dictionary<string, string> COBie_GetDictWithKeyWord(Dictionary<string, string> sourceDict, bool isExclude, params string[] keyWords)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            bool isContain = false;
            foreach (string keyName in sourceDict.Keys)
            {
                isContain = false;
                foreach (string key in keyWords)
                {
                    isContain = isContain || keyName.Contains(key);
                }

                if (isExclude ^ isContain) result[keyName] = sourceDict[keyName];
                /*if (isExclude)
                {
                    if (isContain == false) result[keyName] = sourceDict[keyName];
                }
                else
                {
                    if (isContain) result[keyName] = sourceDict[keyName];
                }*/
            }
            return result;
        }

        /// <summary>
        /// COBie中英欄位對照表 字典{英，中)
        /// </summary>
        public static Dictionary<string, string> dictCOBieColumnZH = new Dictionary<string, string>()
        {
            #region [模型資訊 - 其它非COBie開頭的資訊欄位]

            {"ElementId","設備簡碼"},
            {"Category","Category"},
            {"CategoryId","CategoryId"},
            {"parent","parent"},
            {"Level","Level"},
            {"樓層","樓層"},
            {"距離樓層的高程","距離樓層的高程"},
            {"主體","主體"},
            {"距離主體的偏移","距離主體的偏移"},
            {"與鄰近元素一同移動","與鄰近元素一同移動"},
            {"建立階段","建立階段-3D"},
            {"拆除階段","拆除階段"},
            {"體積","體積"},
            {"影像","影像"},
            {"備註","備註"},
            {"標註","標註"},
            {"08設備名稱","08設備名稱"},
            {"09編號","09編號"},
            {"04樓層","04樓層"},
            {"01公司名稱","01公司名稱"},
            {"02地區","02地區"},
            {"03建物名","03建物名"},
            {"[00Device ID]","設備編碼"},
            {"10ID","10ID"},
            {"所屬系統","所屬系統"},
            {"07子系統","07子系統"},
            {"hostedRack","hostedRack"},
            {"05空間編號","05空間編號"},
            {"06系統","06系統"},
            {"類型名稱","類型名稱"},
            {"房間名稱","房間名稱"},
            {"房間編號","房間編號"},
            {"viewable_in","viewable_in"},
            {"預設高程","預設高程"},
            {"類型影像","類型影像"},
            {"工項編碼","工項編碼"},
            {"模型","模型"},
            {"製造商","製造商"},
            {"類型備註","類型備註"},
            {"URL","URL"},
            {"描述","描述"},
            {"組合代碼","組合代碼"},
            {"成本","成本"},
            {"組合描述","組合描述"},
            {"類型標記","類型標記"},
            {"OmniClass 編號","OmniClass 編號"},
            {"OmniClass 標題","OmniClass 標題"},
            {"代碼名稱","代碼名稱" },
            #endregion

            {"COBie.Component.Description", "設備描述"},
            {"COBie.Component.TagName", "設備編碼"},
            {"COBie.Component.AssetIdentifier", "FM資產識別字"},
            {"COBie.Component.InstallationDate", "安裝日期"},
            {"COBie.Component.SerialNumber", "產品序號"},
            {"COBie.Component.WarrantyGuarantorLabor", "保固廠商"},
            {"COBie.Component.WarrantyDurationPart", "保固時間"},
            {"COBie.Component.WarrantyDurationUnit", "保固時間單位"},
            {"COBie.Component.WarrantyStartDate", "保固開始時間"},
            {"Other.Component.WarrantyEndDate", "保固結束時間"},
            {"COBie.System.Category", "系統類別"},
            {"COBie.System.Name", "系統名稱"},
            {"Other.Equipment.Supplier", "供應廠商"},

            {"COBie.Type.Name", "設備品類名稱"},
            {"COBie.Type.Manufacturer", "製造廠商"},
            {"COBie.Type.ModelNumber", "產品型號"},
            {"COBie.Type.Category", "OmniClass編碼"},
            {"COBie.Type.AccessibilityPerformance", "無障礙功能"},
            {"COBie.Type.Shape", "形狀"},
            {"COBie.Type.Size", "尺寸"},
            {"COBie.Type.Color", "顏色"},
            {"COBie.Type.Finish", "完成面"},
            {"COBie.Type.Grade", "設備分級"},
            {"COBie.Type.Material", "材質"},
            {"COBie.Type.ReplacementCost", "設備售價"},
            {"COBie.Type.ExpectedLife", "使用年限"},

            {"COBie.Facility.Name", "專案棟別名稱"},
            {"COBie.Facility.ProjectName", "專案名稱"},
            {"COBie.Facility.SiteName", "項目地點"},
            {"COBie.Floor.Name", "樓層名稱"},
            {"COBie.Space.Name", "空間名稱"},
            {"COBie.Space.RoomTag", "空間代號"},

            {"COBie.Contact.Company", "聯絡單位公司"},
            {"COBie.Contact.Department", "聯絡單位部門"},
            {"COBie.Contact.Email", "聯絡人Email"},
            {"COBie.Contact.FamilyName", "聯絡人姓氏"},
            {"COBie.Contact.GivenName", "聯絡人名字"},
            {"COBie.Contact.Phone", "聯絡人電話"},
            {"COBie.Contact.Street", "聯絡人地址"},

            {"Other.Document.Inspection", "保養檢查表"},
            {"Other.Document.Handout", "使用手冊"},
            {"Other.Document.Drawing", "圖說"},
        };
        #endregion
    }
}

/// <summary>
/// 設備類型：DCR / DCS / DCN / DCE / DCP
/// </summary>
public enum enumSystemType { DCR, DCS, DCN, DCE, DCP }