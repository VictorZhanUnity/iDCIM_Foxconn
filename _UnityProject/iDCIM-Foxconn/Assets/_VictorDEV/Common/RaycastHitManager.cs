using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VictorDev.Common
{
    public class RaycastHitManager : SingletonMonoBehaviour<RaycastHitManager>
    {
        [Header(">>> IRaycastHitReceiver接收器列表")]
        [SerializeField] private List<MonoBehaviour> _iRayCastReceiverList;
        private List<IRaycastHitReceiver> iRayCastReceiverList { get; set; }

        private void Awake()
        {
            iRayCastReceiverList = _iRayCastReceiverList.OfType<IRaycastHitReceiver>().ToList();
            InitListener();
        }

        private void InitListener()
        {
            onSelectObjectEvent.AddListener((target) => iRayCastReceiverList.ForEach(receiver => receiver.OnSelectObjectHandler(target)));
            onMouseOverObjectEvent.AddListener((target) => iRayCastReceiverList.ForEach(receiver => receiver.OnMouseOverObjectEvent(target)));
            onMouseExitObjectEvent.AddListener((target) => iRayCastReceiverList.ForEach(receiver => receiver.OnMouseExitObjectEvent(target)));
            onDeselectObjectEvent.AddListener((target) => iRayCastReceiverList.ForEach(receiver => receiver.OnDeselectObjectHandler(target)));
        }

        private void OnValidate() => _iRayCastReceiverList = ObjectHandler.CheckTypoOfList<IRaycastHitReceiver>(_iRayCastReceiverList);

        /// <summary>
        /// 當點擊模型物件時Invoke
        /// </summary>
        public static UnityEvent<Transform> onSelectObjectEvent { get; set; } = new UnityEvent<Transform>();
        /// <summary>
        /// 當取消選取模型物件時Invoke
        /// </summary>
        public static UnityEvent<Transform> onDeselectObjectEvent { get; set; } = new UnityEvent<Transform>();
        /// <summary>
        /// 當MouseOver模型物件時Invoke
        /// </summary>
        public static UnityEvent<Transform> onMouseOverObjectEvent { get; set; } = new UnityEvent<Transform>();
        /// <summary>
        /// 當MouseExit模型物件時Invoke
        /// </summary>
        public static UnityEvent<Transform> onMouseExitObjectEvent { get; set; } = new UnityEvent<Transform>();

        [Header(">>> 滑鼠事件之對像物件LayerMask設置")]
        [SerializeField] private LayerMask layerDefault;
        [SerializeField] private LayerMask layerMouseDown, layerMouseOver;

        [Header(">>> 是否允許運行")]
        [SerializeField] private bool isActivated = true;
        public static bool IsActivated { set => Instance.isActivated = value; }

        [Header(">>> [僅顯示] - 目前已選取的物件清單")]
        [SerializeField] private List<GameObject> selectedObject = new List<GameObject>();

        [Header(">>> [僅顯示] - 滑鼠事件之對像物件")]
        [SerializeField] private Transform currentSelectedObject;
        public static Transform CurrentSelectedObject => Instance.currentSelectedObject;
        [SerializeField] private GameObject lastHoveredObject;  // 上一次指向的物體
        [SerializeField] private GameObject currentHoveredObject;  // 當前指向的物體

        private Camera _mainCamera;
        private Camera mainCamera => _mainCamera ??= Camera.main;

        public static bool isModelSelected(Transform target)
            => LayerMaskHandler.IsSameLayerMask(target.gameObject, Instance.layerMouseDown);
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
             //   if (currentHoveredObject.CompareTag("BuildContainer_Device")) return;

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
            onMouseExitObjectEvent.Invoke(exitedObject.transform);
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
                    if (LayerMaskHandler.IsSameLayerMask(result[0].gameObject, layerMouseDown)) CancellObjectSelected(result[0]);
                    else ToSelectTarget(result[0]);
                }
            }
        }

        /// <summary>
        /// 選取目標對像 [多選邏輯]
        /// </summary>
        public static void ToSelectTarget(Transform target, bool isInvokeEvent = true)
        {
           // if (target.CompareTag("BuildContainer_Device")) return;

            //檢查對像是否已被選取
            bool isAlreadySelected = false;
            bool isRack = target.name.Contains("RACK", StringComparison.OrdinalIgnoreCase) || target.name.Contains("ATEN", StringComparison.OrdinalIgnoreCase);
            if (target.childCount > 0 && isRack == false)
            {
                isAlreadySelected = target.GetChild(0) != null ? target.GetChild(0).gameObject.activeSelf : false;
            }

            if (isAlreadySelected) Instance.selectedObject.Remove(target.gameObject);
            else Instance.selectedObject.Add(target.gameObject);

            if (Instance.currentSelectedObject != null) LayerMaskHandler.SetGameObjectLayerToLayerMask(Instance.currentSelectedObject.gameObject, Instance.layerDefault);

            Instance.currentSelectedObject = isAlreadySelected ? null : target;

            //設定對像狀態
            //target.GetChild(0)?.gameObject.SetActive(!isAlreadySelected);
            LayerMaskHandler.SetGameObjectLayerToLayerMask(target.gameObject, isAlreadySelected ? Instance.layerDefault : Instance.layerMouseDown);

            if (isInvokeEvent)
            {
                if (isAlreadySelected) onDeselectObjectEvent.Invoke(target);
                else onSelectObjectEvent.Invoke(target);
            }
        }
        /// <summary>
        ///復原全部已選擇物件的狀態
        /// </summary>
        public static void RestoreSelectedObjects()
        {
            Instance.selectedObject.ForEach(target => onDeselectObjectEvent.Invoke(target.transform));
            Instance.selectedObject.Clear();
            if(Instance.currentSelectedObject != null) CancellObjectSelected(Instance.currentSelectedObject, true);
            Instance.currentSelectedObject = null;
        }

        public static void CancellObjectSelected(string modelName, bool isInvokeEvent = true)
        {
            List<Transform> model = MaterialHandler.FindTargetObjects(modelName);
            if (model.Count > 0) CancellObjectSelected(model[0]);
        }

        public static void CancellObjectSelected(Transform target, bool isInvokeEvent = true)
        {
            //設定對像狀態
            if (target.childCount > 0)
            {
                target.GetChild(0).gameObject.SetActive(false);
            }
            LayerMaskHandler.SetGameObjectLayerToLayerMask(target.gameObject, Instance.layerDefault);
            Instance.selectedObject.Remove(target.gameObject);
            if (isInvokeEvent) onDeselectObjectEvent.Invoke(target);
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

    public interface IRaycastHitReceiver
    {
        void OnSelectObjectHandler(Transform target);
        void OnDeselectObjectHandler(Transform target);
        void OnMouseOverObjectEvent(Transform target);
        void OnMouseExitObjectEvent(Transform target);
    }
}
