using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VictorDev.Common
{
    public class RaycastHitManager : SingletonMonoBehaviour<RaycastHitManager>
    {
        /// <summary>
        /// 當點擊模型物件時Invoke
        /// </summary>
        public static UnityEvent<Transform> onSelectObjectEvent { get; set; } = new UnityEvent<Transform>();
        /// <summary>
        /// 當MouseOver模型物件時Invoke
        /// </summary>
        public static UnityEvent<Transform> onMouseOverObjectEvent { get; set; } = new UnityEvent<Transform>();
        /// <summary>
        /// 當MouseExit模型物件時Invoke
        /// </summary>
        public static UnityEvent onMouseExitObjectEvent { get; set; } = new UnityEvent();

        [Header(">>> 滑鼠事件之對像物件LayerMask設置")]
        [SerializeField] private LayerMask layerDefault;
        [SerializeField] private LayerMask layerMouseDown, layerMouseOver;

        [Header(">>> 是否允許運行")]
        [SerializeField] private bool isActivated = true;
        public static bool IsActivated { set => Instance.isActivated = value; }

        [Header(">>> [僅顯示] - 滑鼠事件之對像物件")]
        [SerializeField] private Transform currentSelectedObject;
        public static Transform CurrentSelectedObject => Instance.currentSelectedObject;
        [SerializeField] private GameObject lastHoveredObject;  // 上一次指向的物體
        [SerializeField] private GameObject currentHoveredObject;  // 當前指向的物體

        private Camera _mainCamera;
        private Camera mainCamera
        {
            get
            {
                _mainCamera ??= Camera.main;
                return _mainCamera;
            }
        }

        void Update()
        {
            if (isActivated)
            {
                MouseOverAndExitListener();
                MouseDownListener();
            }
        }

        /// <summary>
        /// 處理MouserOver與MouseExit
        /// </summary>
        private void MouseOverAndExitListener()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                currentHoveredObject = hit.collider.gameObject;

                // 檢查是否指向了不同的物體
                if (currentHoveredObject != lastHoveredObject)
                {
                    // 如果上一次有物體，且已不再指向它，進行MouseExit處理
                    if (lastHoveredObject != null) MouseExitHandler(lastHoveredObject);

                    // 如果現在指向了新的物體，觸發 MouseOver
                    if (currentHoveredObject != null)
                    {
                        if (EventSystem.current.IsPointerOverGameObject()) return;
                        MouseOverHandler(currentHoveredObject);
                    }

                    // 更新上一次指向的物體
                    lastHoveredObject = currentHoveredObject;
                }
            }
            else
            {
                // 如果Raycast沒有命中物體且之前有指向的物體，觸發 MouseExit
                if (lastHoveredObject != null)
                {
                    MouseExitHandler(lastHoveredObject);
                    lastHoveredObject = null;  // 清空上一次指向的物體
                }
            }
        }

        /// <summary>
        /// MouseExit事件處理
        /// </summary>
        private void MouseExitHandler(GameObject exitedObject)
        {
            if (currentSelectedObject != null)
            {
                if (exitedObject != currentSelectedObject.gameObject)
                {
                    // 在這裡處理 MouseExit 的邏輯 (例如還原物體顏色)
                    LayerMaskHandler.SetGameObjectLayerToLayerMask(exitedObject, layerDefault);
                }
            }
            onMouseExitObjectEvent.Invoke();
        }
        /// <summary>
        /// MouseOver事件處理
        /// </summary>
        private void MouseOverHandler(GameObject hoveredObject)
        {
            if (currentSelectedObject != null)
            {
                if (hoveredObject != currentSelectedObject.gameObject)
                {
                    // 在這裡處理 MouseOver 的邏輯 (例如更改物體顏色)
                    LayerMaskHandler.SetGameObjectLayerToLayerMask(hoveredObject, layerMouseOver);
                }
            }
            onMouseOverObjectEvent.Invoke(hoveredObject.transform);
        }

        /// <summary>
        /// MouseDown事件
        /// </summary>
        private void MouseDownListener()
        {
            if (Input.GetMouseButtonDown(0))
            {
                List<Transform> result = RaycastHitObjects();
                if (result.Count > 0)
                {
                    ToSelectTarget(result[0]);
                }
            }
        }
        /// <summary>
        /// 選取目標對像
        /// </summary>
        public static void ToSelectTarget(Transform target, bool isInvokeEvent = true)
        {
            RestoreSelectedObject();
            Instance.currentSelectedObject = target;
            LayerMaskHandler.SetGameObjectLayerToLayerMask(Instance.currentSelectedObject.gameObject, Instance.layerMouseDown);
            target.GetChild(0).gameObject.SetActive(true);

            if (isInvokeEvent) onSelectObjectEvent.Invoke(target);
        }
        /// <summary>
        ///復原選擇物件的狀態
        /// </summary>
        public static void RestoreSelectedObject()
        {
            if (Instance.currentSelectedObject != null)
            {
                CancellObjectSelected(Instance.currentSelectedObject);
                Instance.currentSelectedObject = null;
            }
        }

        public static void CancellObjectSelected(Transform target)
        {
            target.GetChild(0).gameObject.SetActive(false);
            LayerMaskHandler.SetGameObjectLayerToLayerMask(target.gameObject, Instance.layerDefault);
        }

        //==============================

        /// <summary>
        /// 取得射線經過的3D物件
        /// <para>  + numOfGetObj: 從近到遠要抓多少個物件</para>
        /// <para>  + rayDistance: 射線距離</para>
        /// </summary>
        public static List<Transform> RaycastHitObjects(int numOfGetObj = 1, float rayDistance = float.MaxValue)
        {
            List<Transform> result = new List<Transform>();

            //點擊在UI 元素上
            if (EventSystem.current.IsPointerOverGameObject()) return result;

            // 從攝影機的位置發射射線，從滑鼠位置開始
            Ray ray = Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            // 使用 Physics.RaycastAll 來獲取射線經過的所有物件(並未排序)
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

            // 根據距離從最近到最遠排序
            List<RaycastHit> sortedHits = hits.OrderBy(hit => hit.distance).ToList();

            // 遍歷所有碰撞到的物件
            foreach (RaycastHit hit in sortedHits)
            {
                Transform hitObject = hit.collider.gameObject.transform;
                if (result.Count < numOfGetObj) result.Add(hitObject);
            }
            return result;
        }
    }
}
