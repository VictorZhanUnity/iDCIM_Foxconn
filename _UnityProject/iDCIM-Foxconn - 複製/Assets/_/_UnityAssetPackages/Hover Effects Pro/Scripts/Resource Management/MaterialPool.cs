using UnityEngine;

namespace HoverEffectsPro
{
    public class MaterialPool : Singleton<MaterialPool>
    {
        private Material _simpleColor;
        private Material _gizmoSolidHandle;

        public Material SimpleColor
        {
            get
            {
                if (_simpleColor == null) _simpleColor = new Material(ShaderPool.Get.SimpleColor);
                return _simpleColor;
            }
        }
        public Material GizmoSolidHandle
        {
            get
            {
                if (_gizmoSolidHandle == null) _gizmoSolidHandle = new Material(ShaderPool.Get.GizmoSolidHandle);
                return _gizmoSolidHandle;
            }
        }
    }
}
