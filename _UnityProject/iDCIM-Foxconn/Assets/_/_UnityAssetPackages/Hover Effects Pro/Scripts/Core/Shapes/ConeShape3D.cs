using System.Collections.Generic;
using UnityEngine;

namespace HoverEffectsPro
{
    public class ConeShape3D : Shape3D
    {
        private Vector3 _baseCenter = ModelBaseCenter;
        private Quaternion _rotation = Quaternion.identity;
        private float _baseRadius = 1.0f;
        private float _height = 1.0f;

        public Vector3 BaseCenter { get { return _baseCenter; } set { _baseCenter = value; } }
        public Vector3 Tip { get { return _baseCenter + CentralAxis * _height; } set { _baseCenter = value - CentralAxis * _height; } }
        public float BaseRadius { get { return _baseRadius; } set { _baseRadius = Mathf.Abs(value); } }
        public float Height { get { return _height; } set { _height = Mathf.Abs(value); } }
        public Quaternion Rotation { get { return _rotation; } set { _rotation = QuaternionEx.NormalizeEx(value); } }
        public Vector3 CentralAxis { get { return Up; } }
        public Vector3 Right { get { return _rotation * ModelRight; } }
        public Vector3 Up { get { return _rotation * ModelUp; } }
        public Vector3 Look { get { return _rotation * ModelLook; } }

        public static Vector3 ModelRight { get { return Vector3.right; } }
        public static Vector3 ModelUp { get { return Vector3.up; } }
        public static Vector3 ModelLook { get { return Vector3.forward; } }
        public static Vector3 ModelBaseCenter { get { return Vector3.zero; } }

        public void AlignTip(Vector3 axis)
        {
            Rotation = QuaternionEx.FromToRotation3D(CentralAxis, axis, Look) * _rotation;
        }

        public override void RenderSolid()
        {
            Graphics.DrawMeshNow(MeshPool.Get.UnitCone, Matrix4x4.TRS(_baseCenter, _rotation, new Vector3(_baseRadius, _height, _baseRadius)));
        }
    }
}
