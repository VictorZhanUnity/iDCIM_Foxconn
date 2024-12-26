using System;
using System.Collections.Generic;
using UnityEngine;

namespace VictorDev.Common
{
    [Serializable]
    public class ModelDisplayConfiguration
    {
        [SerializeField] List<Transform> models;
        [SerializeField] List<string> objKeyWords;
        /// <summary>
        /// 欲顯示的物件
        /// </summary>
        public List<Transform> modelsList => models;

        public void FindTargetObjects()
        {
            models = MaterialHandler.FindTargetObjects(objKeyWords);
        }

        public void AddColliderToObjects()
        {
            ObjectHandler.AddColliderToObjects(models, new BoxCollider());
        }
        public void RemoveColliderFromObjects()
        {
            ObjectHandler.RemoveColliderFromObjects(models);
        }
/*
        #region [ContextMenu]
        [ContextMenu("- [Parent] 根據Keywords尋找目標物件")]
        public void FindTargetObjects() => modelForDisplay.FindTargetObjects();
        [ContextMenu("- [Parent] 新增BoxCollider到目標物件")]
        public void AddColliderToObjects() => modelForDisplay.AddColliderToObjects();
        [ContextMenu("- [Parent] 移除目標物件的Collider")]
        public void RemoveColliderFromObjects() => modelForDisplay.RemoveColliderFromObjects();
        #endregion
*/
    }
}