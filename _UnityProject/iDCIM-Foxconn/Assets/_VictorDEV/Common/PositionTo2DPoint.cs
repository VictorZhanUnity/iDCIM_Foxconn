using UnityEngine;

namespace VictorDev.Common
{
    /// 依指定3D物件之座標，轉成螢幕2D座標進行定位
    public class PositionTo2DPoint : MonoBehaviour
    {
        [SerializeField] private Vector2 posOffset;

        void Update()
        {
            // 1. 轉換 3D 世界座標到螢幕座標
            Vector3 screenPos = MainCamera.WorldToScreenPoint(target3DObject.position);

            // 2. 取得 Canvas 尺寸

            float scaleX = CanvasRect.sizeDelta.x / Screen.width;
            float scaleY = CanvasRect.sizeDelta.y / Screen.height;

            // 3. 計算相對於 Canvas 的座標
            Vector2 localPos = new Vector2(
                (screenPos.x - (Screen.width * 0.5f)) * scaleX,
                (screenPos.y - (Screen.height * 0.5f)) * scaleY
            );

            // 4. 設定 UI 位置
            RectTrans.anchoredPosition = localPos + posOffset;
        }

        #region Components

        public Transform target3DObject; // 目標3D物件
        private Camera MainCamera => _mainCamera ??= Camera.main; // 指定主要攝影機
        private Camera _mainCamera; // 指定主要攝影機
        private RectTransform RectTrans => _rectTrans ??= GetComponent<RectTransform>();
        private RectTransform _rectTrans; // UI 按鈕 (或任何 UI 元件)

        private RectTransform CanvasRect =>
            _canvasRect ??= GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        private RectTransform _canvasRect; // UI 按鈕 (或任何 UI 元件)

        #endregion
    }
}