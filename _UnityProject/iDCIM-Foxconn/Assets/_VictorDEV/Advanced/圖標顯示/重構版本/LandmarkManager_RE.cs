using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 圖標管理器
    /// </summary>
    public class LandmarkManager_RE : SingletonMonoBehaviour<LandmarkManager_RE>
    {
        /// <summary>
        /// 有ToggeOn為True時Invoke
        /// </summary>
        public static UnityEvent<bool, ILandmarkHandler> onToggleOnEvent = new UnityEvent<bool, ILandmarkHandler>();

        [Header(">>>攝影機遠近之地標尺吋調整")]
        [SerializeField] private float minScale = 0.8f; // 最小缩放比例
        [SerializeField] private float maxScale = 1f; // 最大缩放比例
        [SerializeField] private float maxDistance = 500f; // 最大距离
        [SerializeField] private float minDistance = 10f; // 最小距离

        [Header(">>> Components")]
        [SerializeField] private Transform container;
        [SerializeField] private ToggleGroup toggleGroup;

        private Camera mainCamera;

        /// <summary>
        /// 目前顯示的landmark圖標，未來可以用is來判斷類別型態
        /// </summary>
        private List<ILandmarkHandler> landmarkForDisplay { get; set; } = new List<ILandmarkHandler>();

        private void Start()
        {
            mainCamera = Camera.main;
        }

        /// <summary>
        /// 選取目標對像
        /// </summary>
        public static void SetToggleOnWithoutNotify(Transform target, bool isOn = true)
        {
            ILandmarkHandler result = Instance.landmarkForDisplay.FirstOrDefault(landmark => landmark.targetModel == target);
            if (result != null) result.SetToggleOnWithoutNotify(isOn);
        }

        /// <summary>
        /// 新增地標 {圖標Prefab (ILandmarkHandler)、資料項(ILandmarkData), 欲顯示的模型清單(Transform)}
        /// </summary>
        public static void AddLandMarks<T, D>(T landmarkPrefab, List<D> datas, List<Transform> models) where T : LandmarkHandler<D> where D : ILandmarkData
        {
            RemoveLandmarks<T, D>();
            datas.ForEach(data =>
            {
                //用devicePath比對出目標物件
                Transform targetModel = models.FirstOrDefault(model => model.name.Contains(data.DevicePath));

                T landmark = ObjectPoolManager.GetInstanceFromQueuePool(landmarkPrefab, Instance.container);
                landmark.name = $"[{typeof(T).Name}] - {targetModel.name}";
                landmark.SetToggleOnWithoutNotify(RaycastHitManager.isModelSelected(targetModel));
                landmark.ShowData(data, targetModel);
                Instance.landmarkForDisplay.Add(landmark);
                landmark.onToggleEvent.AddListener(onToggleOnEvent.Invoke);
            });
        }

        /// <summary>
        /// 移除除目標Landmark圖標物件 {圖標類型(ILandmarkHandler)、資料項(ILandmarkData)}
        /// </summary>
        public static void RemoveLandmarks<T, D>() where T : LandmarkHandler<D> where D : ILandmarkData
        {
            ObjectPoolManager.PushToPool<T>(Instance.container);
            Instance.landmarkForDisplay.OfType<T>().ToList().ForEach(landmark => landmark.onToggleEvent.RemoveAllListeners());
            Instance.landmarkForDisplay.RemoveAll(landmark => landmark is T);
        }

        private void Update()
        {
            landmarkForDisplay.ForEach(landmark =>
            {
                // 更新每个Landmark的UI位置
                Vector3 pos = landmark.targetModel.position;
                pos.y += landmark.offsetHeight;
                landmark.rectTransform.position = mainCamera.WorldToScreenPoint(pos);

                // 更新每个Landmark的UI尺吋
                float distance = Vector3.Distance(mainCamera.transform.position, landmark.targetModel.position);
                float scale = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
                float size = Mathf.Lerp(maxScale, minScale, scale);
                landmark.rectTransform.localScale = new Vector3(size, size, size);
                landmark.distanceToCamera = Vector3.Distance(mainCamera.transform.position, landmark.targetModel.position);
            });
            // 根据距离对Landmark进行排序并调整Sibling Index
            landmarkForDisplay.Sort((a, b) => b.distanceToCamera.CompareTo(a.distanceToCamera));
            for (int i = 0; i < landmarkForDisplay.Count; i++)
            {
                landmarkForDisplay[i].rectTransform.SetSiblingIndex(i);
            }
        }
    }
}