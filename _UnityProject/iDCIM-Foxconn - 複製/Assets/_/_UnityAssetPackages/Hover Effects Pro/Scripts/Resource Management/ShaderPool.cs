using UnityEngine;

namespace HoverEffectsPro
{
    public class ShaderPool : Singleton<ShaderPool>
    {
        private Shader _simpleColor;
        private Shader _gizmoSolidHandle;

        public Shader SimpleColor
        {
            get
            {
                if (_simpleColor == null) _simpleColor = Shader.Find("HoverEffectsPro/SimpleColor");
                return _simpleColor;
            }
        }
        public Shader GizmoSolidHandle
        {
            get
            {
                if (_gizmoSolidHandle == null) _gizmoSolidHandle = Shader.Find("HoverEffectsPro/GizmoSolidHandle");
                return _gizmoSolidHandle;
            }
        }
    }
}
