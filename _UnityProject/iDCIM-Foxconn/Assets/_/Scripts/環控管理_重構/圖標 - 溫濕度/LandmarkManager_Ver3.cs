using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

/// <summary>
/// 模型圖標管理器
/// </summary>
public class LandmarkManager_Ver3 : SingletonMonoBehaviour<LandmarkManager_Ver3>
{
    [Header(">>>Toggle狀態變更時Invoke {isOn, 目標模型}")]
    public static UnityEvent<bool, Transform> onToggleValueChanged = new UnityEvent<bool, Transform>();

    [Header(">>>欲顯示的Landmark，在此用Update將3D座梳換算2D座標")]
    [SerializeField] private List<Landmark_RE> landmarkForDisplay = new List<Landmark_RE>();

    /// <summary>
    /// 設定Toggle.isOn狀態
    /// <para>+ targetModel: 依目標模型尋找Landmark Toggle</para>
    /// <para>+ isOn: true/false</para>
    /// <para>+ isInvokeEvent: 是否觸發Toggle事件</para>
    /// </summary>
    public static void SetLandmarkIsOn(Transform targetModel, bool isOn, bool isInvokeEvent = false)
        => Instance.landmarkForDisplay.FirstOrDefault(landmark => landmark.targetModel == targetModel)?.SetToggleStatus(isOn, isInvokeEvent);

    /// <summary>
    /// 新增圖標
    /// </summary>
    public static void AddLandmarks(List<Landmark_RE> landmarks, bool isSetToggleGroup = true) => landmarks.ForEach(landmark => AddLandmark(landmark, isSetToggleGroup));
    public static void AddLandmark(Landmark_RE landmark, bool isSetToggleGroup = true)
    {
        if (Instance.landmarkForDisplay.Contains(landmark) == false)
        {
            if (isSetToggleGroup) landmark.toggleGroup = Instance.toggleGroup;
            Instance.landmarkForDisplay.Add(Instance.InitListener(landmark));
        }
    }

    private Landmark_RE InitListener(Landmark_RE landmark)
    {
        landmark.onToggleValueChanged.RemoveAllListeners();
        landmark.onToggleValueChanged.AddListener(onToggleValueChanged.Invoke);
        return landmark;
    }

    private void OnEnable() => landmarkForDisplay.ForEach(landmark => InitListener(landmark));
    private void OnDisable() => landmarkForDisplay.ForEach(landmark => landmark.onToggleValueChanged.RemoveAllListeners());

    private void Update()
    {
        //計算每個Landmark位置
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

    [Header(">>>攝影機遠近之地標尺吋調整")]
    [SerializeField] private float minScale = 0.8f; // 最小缩放比例
    [SerializeField] private float maxScale = 1f; // 最大缩放比例
    [SerializeField] private float maxDistance = 500f; // 最大距离
    [SerializeField] private float minDistance = 10f; // 最小距离

    #region [Components]
    private Camera _mainCamera { get; set; }
    private Camera mainCamera => _mainCamera ??= Camera.main;
    private ToggleGroup _toggleGroup { get; set; }
    private ToggleGroup toggleGroup => _toggleGroup ??= GetComponent<ToggleGroup>();
    private Transform _container { get; set; }
    private Transform container => _container ??= transform.GetChild(0);
    #endregion
}
