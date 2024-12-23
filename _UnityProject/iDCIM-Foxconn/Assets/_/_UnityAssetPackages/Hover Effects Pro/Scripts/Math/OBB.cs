﻿using UnityEngine;
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public struct OBB
    {
        private Vector3 _size;
        private Vector3 _center;
        private Quaternion _rotation;
        private bool _isValid;

        public bool IsValid { get { return _isValid; } }
        public Vector3 Center { get { return _center; } set { _center = value; } }
        public Vector3 Size { get { return _size; } set { _size = value; } }
        public Vector3 Extents { get { return Size * 0.5f; } }
        public Quaternion Rotation { get { return _rotation; } set { _rotation = value; } }
        public Matrix4x4 RotationMatrix { get { return Matrix4x4.TRS(Vector3.zero, _rotation, Vector3.one); } }
        public Vector3 Right { get { return _rotation * Vector3.right; } }
        public Vector3 Up { get { return _rotation * Vector3.up; } }
        public Vector3 Look { get { return _rotation * Vector3.forward; } }

        public OBB(Vector3 center, Vector3 size)
        {
            _center = center;
            _size = size;
            _rotation = Quaternion.identity;
            _isValid = true;
        }

        public OBB(Vector3 center, Vector3 size, Quaternion rotation)
        {
            _center = center;
            _size = size;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(Vector3 center, Quaternion rotation)
        {
            _center = center;
            _size = Vector3.zero;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(Quaternion rotation)
        {
            _center = Vector3.zero;
            _size = Vector3.zero;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(Bounds bounds, Quaternion rotation)
        {
            _center = bounds.center;
            _size = bounds.size;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(AABB aabb)
        {
            _center = aabb.Center;
            _size = aabb.Size;
            _rotation = Quaternion.identity;
            _isValid = true;
        }

        public OBB(AABB aabb, Quaternion rotation)
        {
            _center = aabb.Center;
            _size = aabb.Size;
            _rotation = rotation;
            _isValid = true;
        }

        public OBB(AABB modelSpaceAABB, Transform worldTransform)
        {
            _size = Vector3.Scale(modelSpaceAABB.Size, worldTransform.lossyScale);
            _center = worldTransform.TransformPoint(modelSpaceAABB.Center);
            _rotation = worldTransform.rotation;
            _isValid = true;
        }

        public OBB(OBB copy)
        {
            _size = copy._size;
            _center = copy._center;
            _rotation = copy._rotation;
            _isValid = copy._isValid;
        }

        public static OBB GetInvalid()
        {
            return new OBB();
        }

        public void Inflate(float amount)
        {
            Size += Vector3Ex.FromValue(amount);
        }

        public void Inflate(Vector3 amount)
        {
            Size += amount.Abs();
        }

        public Matrix4x4 GetUnitBoxTransform()
        {
            if (!_isValid) return Matrix4x4.identity;
            return Matrix4x4.TRS(Center, Rotation, Size);
        }

        public List<Vector3> GetCornerPoints()
        {
            return BoxMath.CalcBoxCornerPoints(_center, _size, _rotation);
        }

        public List<Vector3> GetCenterAndCornerPoints()
        {
            List<Vector3> centerAndCorners = GetCornerPoints();
            centerAndCorners.Add(Center);

            return centerAndCorners;
        }

        public void Encapsulate(OBB otherOBB)
        {
            var otherPts = BoxMath.CalcBoxCornerPoints(otherOBB.Center, otherOBB.Size, otherOBB.Rotation);

            Matrix4x4 transformMtx = Matrix4x4.TRS(Center, Rotation, Vector3.one);
            var modelPts = transformMtx.inverse.TransformPoints(otherPts);

            AABB modelAABB = new AABB(Vector3.zero, Size);
            modelAABB.Encapsulate(modelPts);

            Center = (Rotation * modelAABB.Center) + Center;
            Size = modelAABB.Size;
        }
    }
}
