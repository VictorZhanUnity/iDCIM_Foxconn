﻿using UnityEngine;
using System.Collections.Generic;

namespace HoverEffectsPro
{
    public static class GraphicsEx
    {
        public static void DrawWireCornerBox(AABB box, float wireCornerLinePercentage)
        {
            // Store these for easy access
            Mesh wireCornerLineMesh = MeshPool.Get.UnitCoordSystem;
            List<Vector3> boxCorners = box.GetCornerPoints();

            // Clamp percentage
            wireCornerLinePercentage = Mathf.Clamp(wireCornerLinePercentage, 0.0f, 1.0f);

            // Front bottom left point
            Vector3 originalScale = box.Extents * wireCornerLinePercentage;
            Vector3 scale = originalScale;
            Vector3 position = boxCorners[(int)BoxCorner.FrontBottomLeft];
            Matrix4x4 transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Front bottom right point
            position = boxCorners[(int)BoxCorner.FrontBottomRight];
            scale.x *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Front top right point
            position = boxCorners[(int)BoxCorner.FrontTopRight];
            scale.y *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Front top left point
            position = boxCorners[(int)BoxCorner.FrontTopLeft];
            scale = originalScale;
            scale.y *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back bottom left point
            position = boxCorners[(int)BoxCorner.BackBottomLeft];
            scale.y = originalScale.y;
            scale.x *= -1.0f;
            scale.z *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back bottom right point
            position = boxCorners[(int)BoxCorner.BackBottomRight];
            scale.x = originalScale.x;
            transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back top right point
            position = boxCorners[(int)BoxCorner.BackTopRight];
            scale.y *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back top left point
            position = boxCorners[(int)BoxCorner.BackTopLeft];
            scale.x *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);
        }

        /// <summary>
        /// Renders a wire box with lines meeting at corners.
        /// </summary>
        /// <param name="wireCornerLinePercentage">
        /// Can have values in the [0, 1] interval and it controls the length of the
        /// corner lines. A value of 0 draws lines of length 0 and a value of 1 
        /// draws lines of length equal to half the box length along a certain axis.
        /// </param>
        public static void DrawWireCornerBox(OBB box, float wireCornerLinePercentage)
        {
            // Store these for easy access
            Mesh wireCornerLineMesh = MeshPool.Get.UnitCoordSystem;
            List<Vector3> boxCorners = box.GetCornerPoints();

            // Clamp percentage
            wireCornerLinePercentage = Mathf.Clamp(wireCornerLinePercentage, 0.0f, 1.0f);

            // Front bottom left point
            Vector3 originalScale = box.Extents * wireCornerLinePercentage;
            Vector3 scale = originalScale;
            Vector3 position = boxCorners[(int)BoxCorner.FrontBottomLeft];
            Matrix4x4 transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Front bottom right point
            position = boxCorners[(int)BoxCorner.FrontBottomRight];
            scale.x *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Front top right point
            position = boxCorners[(int)BoxCorner.FrontTopRight];
            scale.y *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Front top left point
            position = boxCorners[(int)BoxCorner.FrontTopLeft];
            scale = originalScale;
            scale.y *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back bottom left point
            position = boxCorners[(int)BoxCorner.BackBottomLeft];
            scale.y = originalScale.y;
            scale.x *= -1.0f;
            scale.z *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back bottom right point
            position = boxCorners[(int)BoxCorner.BackBottomRight];
            scale.x = originalScale.x;
            transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back top right point
            position = boxCorners[(int)BoxCorner.BackTopRight];
            scale.y *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);

            // Back top left point
            position = boxCorners[(int)BoxCorner.BackTopLeft];
            scale.x *= -1.0f;
            transformMatrix = Matrix4x4.TRS(position, box.Rotation, scale);
            Graphics.DrawMeshNow(wireCornerLineMesh, transformMatrix);
        }
    }
}
