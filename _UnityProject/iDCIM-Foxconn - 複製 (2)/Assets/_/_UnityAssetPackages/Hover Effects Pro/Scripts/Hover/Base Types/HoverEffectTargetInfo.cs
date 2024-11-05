using UnityEngine;
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public struct HoverEffectTargetInfo
    {
        public GameObject TargetObject;
        public Transform TargetTransform;
        public List<Renderer> TargetRenderers;
        public bool IncludeChildren;
        public bool UseSkinLocalBounds;
    }
}
