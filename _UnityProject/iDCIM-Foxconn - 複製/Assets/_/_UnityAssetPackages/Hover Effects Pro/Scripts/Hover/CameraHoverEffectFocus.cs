using UnityEngine;

namespace HoverEffectsPro
{
    public static class CameraHoverEffectFocus
    {
        public struct Data
        {
            public Vector3 TargetWorldPosition;
            public Vector3 FocusPoint;
            public float FocusPointOffset;

            public Data(Vector3 cameraWorldPosition, Vector3 focusPoint)
            {
                TargetWorldPosition = cameraWorldPosition;
                FocusPoint = focusPoint;
                FocusPointOffset = (cameraWorldPosition - focusPoint).magnitude;
            }
        }

        public static Data CalculateFocusData(Camera camera, AABB focusAABB, CameraHoverEffectSettings settings)
        {
            float frustumHeight = focusAABB.Size.magnitude * settings.FocusScale;
            float frustumDistance = camera.GetFrustumDistanceFromHeight(frustumHeight);
            if (frustumDistance < camera.nearClipPlane) frustumDistance += (camera.nearClipPlane - frustumDistance);

            Vector3 cameraPosition = focusAABB.Center - camera.transform.forward * frustumDistance;
            return new Data(cameraPosition, focusAABB.Center);
        }
    }
}
