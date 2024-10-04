using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Advanced;

namespace VictorDev.Common
{
    public class MaterialHandler : MonoBehaviour
    {
        [Header(">>> 指定要替換的材質")]
        [SerializeField] private Material replaceMaterial;

        [Header(">>> 指定要替換的物件對像")]
        [SerializeField] private Transform targetTransform;

        [Space(20)]
        [Header("[僅顯示用] 物件對像下的每個Transform物件")]
        [SerializeField] private List<DictionaryVisualizer<Transform, Material[]>> materialDictionaryVisualize;

        [Header("[僅顯示用] 排除替換之外Transfomr對像")]
        [SerializeField] private List<Transform> excludeList;

        /// <summary>
        /// 存儲每個物件及其原始材質的字典 {物件Transform, 材質陣列}
        /// </summary>
        private Dictionary<Transform, Material[]> originalMaterials { get; set; } = new Dictionary<Transform, Material[]>();

        /// <summary>
        ///  替換當前物件及所有子物件的材質
        /// </summary>
        public void ReplaceMaterial(HashSet<Transform> exlcudeTargets = null, Material material = null)
        {
            RestoreOriginalMaterials();
            ReplaceMaterialRecursively(targetTransform, material ?? replaceMaterial, exlcudeTargets);
        }

        /// <summary>
        /// [遞迴] 替換物件及其子物件的材質
        /// <para>+ 排除的對像(選填)</para>
        /// </summary>
        private void ReplaceMaterialRecursively(Transform objTransform, Material material, HashSet<Transform> exlcudeTargets = null)
        {
            excludeList.Clear();
            exlcudeTargets.ToList().ForEach(target => excludeList.Add(target));

            // 取得該物件的 Renderer 組件
            // 如果物件有 Renderer 組件，替換材質
            if (objTransform.TryGetComponent<Renderer>(out Renderer renderer))
            {
                //不屬於排除對像，則替換材質
                if (exlcudeTargets != null && exlcudeTargets.Contains(objTransform) == false)
                {
                    // 如果尚未保存原始材質，將它存儲到字典中
                    if (!originalMaterials.ContainsKey(objTransform))
                    {
                        originalMaterials[objTransform] = renderer.sharedMaterials;
                        materialDictionaryVisualize.Add(new DictionaryVisualizer<Transform, Material[]>(objTransform, renderer.sharedMaterials));
                    }

                    // 判斷是否有多個材質
                    if (renderer.sharedMaterials.Length > 1)
                    {
                        // 如果有多個材質，建立新的材質陣列
                        Material[] newMaterials = new Material[renderer.sharedMaterials.Length];

                        for (int i = 0; i < newMaterials.Length; i++)
                        {
                            // 替換為指定的材質
                            newMaterials[i] = material;
                        }

                        // 設定新的材質陣列
                        renderer.materials = newMaterials;
                    }
                    else
                    {
                        // 如果只有一個材質，直接替換
                        renderer.material = material;
                    }

                    //關閉Collider
                    if (objTransform.TryGetComponent<Collider>(out Collider collider))
                    {
                        collider.enabled = false;
                    }
                }
            }

            // 遞迴處理所有子物件
            foreach (Transform child in objTransform)
            {
                if (exlcudeTargets != null && exlcudeTargets.Contains(objTransform) == false)
                {
                    ReplaceMaterialRecursively(child, material, exlcudeTargets);
                }
            }
        }


        // <summary>
        ///  恢復原始材質
        /// </summary>
        public void RestoreOriginalMaterials()
        {
            foreach (var kvp in originalMaterials)
            {
                Transform objTransform = kvp.Key;
                Material[] originalMats = kvp.Value;

                // 取得物件的 Renderer 組件
                // 如果有 Renderer 組件，恢復原本的材質
                if (objTransform.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    renderer.materials = originalMats;
                    //開啟Collider
                    if (objTransform.TryGetComponent<Collider>(out Collider collider))
                    {
                        collider.enabled = true;
                    }
                }
            }
        }

        private void OnValidate() => name = "MaterialHandler";
    }
}
