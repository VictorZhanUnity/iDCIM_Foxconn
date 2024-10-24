using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 地標管理器
    /// </summary>
    public class LandmarkManager : SingletonMonoBehaviour<LandmarkManager>
    {
        [Header(">>>地標列表")]
        [SerializeField] private List<Landmark> landmarks = new List<Landmark>();
        public static List<Landmark> Landmarks => Instance.landmarks;

        [Header(">>>攝影機遠近之地標尺吋調整")]
        [SerializeField] private float minScale = 0.8f; // 最小缩放比例
        [SerializeField] private float maxScale = 1f; // 最大缩放比例
        [SerializeField] private float maxDistance = 500f; // 最大距离
        [SerializeField] private float minDistance = 10f; // 最小距离

        [Space(10)]
        [SerializeField] private static Transform _container;
        public static Transform container
        {
            get
            {
                _container ??= Instance.transform.GetChild(0);
                return _container;
            }
        }

        [SerializeField] private ToggleGroup toggleGroup;

        /// <summary>
        /// 依照地標類別存放於字典
        /// </summary>
        private Dictionary<LandmarkCategory, List<Landmark>> categorizedLandmarks { get; set; } = new Dictionary<LandmarkCategory, List<Landmark>>();


        private void Awake()
        {
            landmarks.ForEach(landmark =>
            {
                if (categorizedLandmarks.ContainsKey(landmark.category) == false)
                {
                    categorizedLandmarks[landmark.category] = new List<Landmark>();
                }
                categorizedLandmarks[landmark.category].Add(landmark);
            });
        }

        /// <summary>
        /// 新增多個地標 {建築物、UI組件、地標高度}
        /// </summary>
        public static void AddLandMarks(List<Landmark> landmarks) => landmarks.ForEach(landmark => AddLandMark(landmark));
        /// <summary>
        /// 新增地標 {建築物、UI組件、地標高度}
        /// </summary>
        public static void AddLandMark(Landmark landmark)
        {
            Instance.landmarks.Add(landmark);
            landmark.toggleGroup = Instance.toggleGroup;
        }

        /// <summary>
        /// 控制特定分类的显示/隐藏
        /// </summary>
        public static void SetCategoryVisibility(LandmarkCategory category, bool isVisible)
        {
            if (Instance.categorizedLandmarks.ContainsKey(category))
            {
                Instance.categorizedLandmarks[category].ForEach(landmark => landmark.uiElement.gameObject.SetActive(isVisible));
            }
        }

        private void Update()
        {
            landmarks.ForEach(landmark =>
            {
                // 更新每个Landmark的UI位置
                Vector3 pos = landmark.targetObject.position;
                pos.y += landmark.offsetHeight;
                landmark.uiElement.position = Camera.main.WorldToScreenPoint(pos);

                // 更新每个Landmark的UI尺吋
                float distance = Vector3.Distance(Camera.main.transform.position, landmark.targetObject.position);
                float scale = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
                float size = Mathf.Lerp(maxScale, minScale, scale);
                landmark.uiElement.localScale = new Vector3(size, size, size);
                landmark.distanceToCamera = Vector3.Distance(Camera.main.transform.position, landmark.targetObject.position);
            });
            // 根据距离对Landmark进行排序并调整Sibling Index
            landmarks.Sort((a, b) => b.distanceToCamera.CompareTo(a.distanceToCamera));
            for (int i = 0; i < landmarks.Count; i++)
            {
                landmarks[i].uiElement.SetSiblingIndex(i);
            }
        }
    }

    

    /// <summary>
    /// 地標分類
    /// </summary>
    public enum LandmarkCategory
    {
        CCTV, IAQ, AccessDoor, DCR, DCN, DCS
    }

    /* [System.Serializable]
     public class Landmark
     {
         [Header(">>> 目標對像的Transform")]
         public Transform targetObject;
         [Header(">>> 對應的UI元素")]
         public RectTransform uiElement;
         [Header(">>> 地標高度")]
         public float height;
         [Header(">>> 地標分類")]
         public LandmarkCategory category;

         [HideInInspector]
         public float distanceToCamera; // 与摄像机的距离

         /// <summary>
         /// 地標 {建筑物的Transform} {对应的UI元素} {地標高度}
         /// </summary>
         public Landmark(Transform targetObject, RectTransform uiElement, float height, LandmarkCategory category = default)
         {
             this.targetObject = targetObject;
             this.uiElement = uiElement;
             this.height = height;
             this.category = category;
         }
     }*/
}