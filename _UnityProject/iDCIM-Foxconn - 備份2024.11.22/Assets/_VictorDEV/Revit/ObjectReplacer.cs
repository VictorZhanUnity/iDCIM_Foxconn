using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VictorDev.Common;
using VictorDev.ComponentUtils;
using VictorDev.EditorTool;

namespace VictorDev.RevitUtils
{
    /// <summary>
    /// [Singleton] 物件替換功能
    /// <para> + 複製 position、rotation、name、childIndex</para>
    /// <para> + 保留原目標對像物件</para>
    /// </summary>
    public class ObjectReplacer : SingletonMonoBehaviour<ObjectReplacer>
    {
        [Header(">>> 來源物件")]
        [SerializeField] private Transform sourceObject;

        [Header(">>> 欲替換的目標物件列表")]
        [SerializeField] private List<Transform> targets;

        private Transform newObject;

        public static void ToReplace(Transform sourceObject, List<Transform> targets)
        {
            if (sourceObject != null) Instance.sourceObject = sourceObject;
            if (targets != null) Instance.targets = targets;

            Instance.targets.ForEach(target =>
             {
                 Instance.newObject = Instantiate(Instance.sourceObject, target.parent);
                 Instance.newObject.position = target.position;
                 Instance.newObject.rotation = target.rotation;
                 Instance.newObject.name = target.name;
                 Instance.newObject.SetSiblingIndex(ComponentHandler.GetChildIndex(target));
             });
        }

#if UNITY_EDITOR
        /// <summary>
        /// 選取欲替換的目標物件
        /// </summary>
        private void SelectAllTargets()
        {
            List<GameObject> selectedObjects = new List<GameObject>();
            Instance.targets.ForEach(target => selectedObjects.Add(target.gameObject));
            Selection.objects = selectedObjects.ToArray();
        }

        [CustomEditor(typeof(ObjectReplacer))]
        private class Inspector : InspectorEditor<ObjectReplacer>
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                GUIStyle btnStyle = _CreateButtonStyle();
                _CreateButton("進行替換", btnStyle, () => ObjectReplacer.ToReplace(null, null));
                _CreateButton("選取全部目標對像物件", btnStyle, Instance.SelectAllTargets);
            }
        }
#endif
    }
}
