using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace VictorDev.Common
{
    /// <summary>
    /// GameObject物件處理
    /// </summary>
    public static class ObjectHandler
    {
        /// <summary>
        /// 根據關鍵字，針對目標物件底下所有子物件進行比對，找出名字包含關鍵字的子物件
        /// </summary>
        public static List<Transform> FindObjectsByKeyword(Transform target, string keyword, bool isCaseSensitive = false)
            => FindObjectsByKeywords(target, new List<string>() { keyword }, isCaseSensitive);

        /// <summary>
        /// 根據關鍵字，針對目標物件底下所有子物件進行比對，找出名字包含關鍵字的子物件
        /// </summary>
        public static List<Transform> FindObjectsByKeywords(Transform target, List<string> keywords, bool isCaseSensitive = false)
        {
            void FindObjectsRecursively(Transform parent, List<string> keywords, List<Transform> result)
            {
                string childName, key;
                foreach (Transform child in parent)
                {
                    // 檢查名稱是否包含任意關鍵字
                    foreach (string keyword in keywords)
                    {
                        childName = (isCaseSensitive) ? child.name : child.name.ToLower();
                        key = (isCaseSensitive) ? keyword : keyword.ToLower();

                        if (childName.Contains(key))
                        {
                            result.Add(child);
                            break; // 如果符合任意一個關鍵字，跳出內層迴圈
                        }
                    }
                    // 遞歸檢查子物件
                    FindObjectsRecursively(child, keywords, result);
                }
            }

            List<Transform> matchingObjects = new List<Transform>();
            FindObjectsRecursively(target, keywords, matchingObjects);
            return matchingObjects;
        }

        /// <summary>
        /// [泛型] 新增Collider型別到目標物件上
        /// <para>+ 泛型Collider只需傳入隨意一個實例化即可，例如: new BoxCollider()</para>
        /// </summary>
        public static void AddColliderToObjects<T>(List<Transform> targetObjects, T collider, bool removeExisting = true) where T : Collider
        {
            if (removeExisting) RemoveColliderFromObjects(targetObjects);
            targetObjects.ForEach(obj => obj.AddComponent<T>());
        }

        /// <summary>
        /// [泛型] 移除Collider型別到目標物件上
        /// </summary>
        public static void RemoveColliderFromObjects(List<Transform> targetObjects)
           => targetObjects.ForEach(obj =>
           {
               Collider[] existingColliders = obj.GetComponents<Collider>();
               foreach (var collider in existingColliders)
               {
#if UNITY_EDITOR
                   Object.DestroyImmediate(collider);
#else
                   Object.Destroy(collider);
#endif
               }
           });

        /// <summary>
        /// 檢查B的包圍盒是否完全位於A的包圍盒內
        /// </summary>
        public static bool IsModelBCompletelyInsideModelA(Collider modelA, Collider modelB)
            => modelA.bounds.Contains(modelB.bounds.min) && modelA.bounds.Contains(modelB.bounds.max);

        /// <summary>
        /// 檢查B是否部分在A內
        /// </summary>
        public static bool IsModelBPartiallyInsideModelA(Collider modelA, Collider modelB)
            => modelA.bounds.Intersects(modelB.bounds);


        /// <summary>
        /// 將底下所有子物件，依照Y軸高底進行排序
        /// </summary>
        public static void SortingChildByPosY(Transform target)
        {
            // 將子物件依 Y 座標排序（從高到低）
            var sortedChildren = target.Cast<Transform>()
                                             .OrderByDescending(child => child.position.y)
                                             .ToList();
            // 更新 Sibling Index
            for (int i = 0; i < sortedChildren.Count; i++)
            {
                sortedChildren[i].SetSiblingIndex(i);
            }
        }

        /// 檢查List裡面是否有實作T類別，將不符合的從List裡移除
        public static List<MonoBehaviour> CheckTypoOfList<T>(List<MonoBehaviour> list) where T : class
        {
            #region 取得上一層呼叫的資訊
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            var method = frame.GetMethod();
            #endregion

            for (int i = 0; i < list.Count; i++)
            {
                MonoBehaviour target = list[i];
                if (target != null && target is not T item)
                {
                    Debug.Log($">>> [接收器：{method.DeclaringType?.Name}] - 物件：{{{target.name}}} 並沒有實作{typeof(T).Name}, 已從列表移除.");
                    list.Remove(target);
                }
            }
            return list;
        }

        /// 檢查List裡面是否有繼承T類別，將不符合的從List裡移除
        public static List<MonoBehaviour> CheckSubTypoOfList<T>(List<MonoBehaviour> list) where T : class
        {
            #region 取得上一層呼叫的資訊
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            var method = frame.GetMethod();
            #endregion

            for (int i = 0; i < list.Count; i++)
            {
                MonoBehaviour target = list[i];
                if (target != null && target.GetType().IsSubclassOf(typeof(T)) == false)
                {
                    Debug.Log($">>> [接收器：{method.DeclaringType?.Name}] - 物件：{{{target.name}}} 並非繼承{typeof(T).Name}, 已從列表移除.");
                    list.Remove(target);
                }
            }
            return list;
        }
    }
}
