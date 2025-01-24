using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VictorDev.MaterialUtils;

namespace _Demo_管線監控.Scripts
{
    public class PipeGroupFinder : MonoBehaviour
    {
        [ContextMenu("- 顯示管線組")]
        public void ToShow() =>pipeList.ForEach(pipe=> ModelMaterialHandler.ToShow(pipe.pipeGroup));

        [ContextMenu("- 恢復所有模型的材質")]
        public void RestoreMaterial() => ModelMaterialHandler.ToShowAll();
        
        
        [ContextMenu("- 每個PipeSet進行搜尋比對")]
        public void FindEachPipeSetGroup() => pipeList.ForEach(GetModelWithCheck);

        /// 檢查鄰近且包含關鍵字的模型
        private void GetModelWithCheck(PipeSet pipeSet)
        {
            pipeSet.pipeGroup.Clear(); // 清空之前的結果
            if (pipeSet.pipeTarget == null)
            {
                Debug.LogError("請設定 Pipe Target！");
                return;
            }

            // 初始化搜尋隊列和已檢查的集合
            Queue<GameObject> toCheck = new Queue<GameObject>();
            HashSet<GameObject> checkedObjects = new HashSet<GameObject>();

            // 將目標物件加入隊列
            toCheck.Enqueue(pipeSet.pipeTarget);
            checkedObjects.Add(pipeSet.pipeTarget);

            // 開始搜索
            while (toCheck.Count > 0)
            {
                // 取出隊列中的第一個物件
                GameObject current = toCheck.Dequeue();

                // 將其加入結果列表
                pipeSet.pipeGroup.Add(current.transform);

                // 獲取當前物件的 Renderer，若無則跳過
                Renderer currentRenderer = current.GetComponent<Renderer>();
                if (currentRenderer == null) continue;

                // 遍歷場景中的所有物件
                foreach (GameObject obj in AllObjects)
                {
                    // 跳過已檢查的物件
                    if (checkedObjects.Contains(obj)) continue;

                    // 確保比對的對像物件有 Renderer
                    if (obj.TryGetComponent(out Renderer objRenderer) == false)
                        continue;

                    // 1、檢查名稱條件
                    if (!string.IsNullOrEmpty(nameFilter) &&
                        !obj.name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // 2、判斷邊界是否相交
                    if (currentRenderer.bounds.Intersects(objRenderer.bounds))
                    {
                        // 將物件加入隊列並標記為已檢查
                        toCheck.Enqueue(obj);
                        checkedObjects.Add(obj);
                        obj.tag = pipeType.ToString();
                        obj.AddComponent<MeshCollider>();
                    }
                }
            }
        }

        #region Components

        [Header(">>> 設定TagName")] [SerializeField]
        private PipeManager.EnumPipeType pipeType;
        public PipeManager.EnumPipeType PipeType => pipeType;

        [Header(">>> 篩選名稱有包含關鍵字的物件")] [SerializeField]
        private string nameFilter;
        
        [Header("[資料項] - 管線組")] [SerializeField]
        private List<PipeSet> pipeList = new List<PipeSet>();

        // 場景中的所有GameObject物件
        private GameObject[] AllObjects => _allObjects ??= FindObjectsOfType<GameObject>();
        private GameObject[] _allObjects;

        [Serializable]
        public class PipeSet
        {
            [Header(">>> 以此目標物件來搜尋比對")] [SerializeField]
            public GameObject pipeTarget;

            [Header("[資料項] - 找到的鄰近群組清單")] [SerializeField]
            public List<Transform> pipeGroup = new List<Transform>();
        }

        #endregion
    }
}