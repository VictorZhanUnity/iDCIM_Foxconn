#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace HoverEffectsPro
{
    public static class UndoEx
    {
        public static void Record(UnityEngine.Object recordObject)
        {
            Undo.RecordObject(recordObject, "Hover Effects");
        }
    }
}
#endif