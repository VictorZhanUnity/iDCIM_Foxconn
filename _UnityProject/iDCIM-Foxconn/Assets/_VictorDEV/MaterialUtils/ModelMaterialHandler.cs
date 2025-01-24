using System.Collections.Generic;
using UnityEngine;
using VictorDev.Common;

namespace VictorDev.MaterialUtils
{
    /// 管理指定模型的材質替換
    public class ModelMaterialHandler : SingletonMonoBehaviour<ModelMaterialHandler>
    {
        [Header(">>> 欲弱化顯示的材質")] [SerializeField]
        private Material materialHide;

        [Header(">>> 欲替換材質的模型主體")] [SerializeField]
        private Transform targetModel;

        public static void ToShow(List<Transform> targetToShow)
        {
            MaterialReplacer.ReplaceMaterialRecursively(Instance.targetModel, Instance.materialHide, targetToShow);
        }

        [ContextMenu("ToShowAll")]
        public static void ToShowAll() => MaterialReplacer.RestoreAllMaterials();

        [ContextMenu("ToHideAll")]
        public static void ToHideAll()
        {
            MaterialReplacer.ReplaceMaterialRecursively(Instance.targetModel, Instance.materialHide);
        }

        #region ContextMenu

        [ContextMenu("RestoreAllToVisible")]
        public void ContextMenu_RestoreAllToVisible() => ToShowAll();

        [ContextMenu("ToHideAll")]
        public void ContextMenu_ToHideAll() => ToHideAll();

        #endregion
    }
}