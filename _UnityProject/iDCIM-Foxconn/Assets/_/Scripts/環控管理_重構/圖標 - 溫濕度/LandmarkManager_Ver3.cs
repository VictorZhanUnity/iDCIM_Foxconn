using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

public class LandmarkManager_Ver3 : SingletonMonoBehaviour<LandmarkManager_Ver3>
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

    /// <summary>
    /// 目前顯示的landmark圖標，未來可以用is來判斷類別型態
    /// </summary>
    public List<Landmark_RE> landmarkForDisplay = new List<Landmark_RE>();
 
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

    #region [Components]
    private Camera _mainCamera { get; set; }
    private Camera mainCamera => _mainCamera ??= Camera.main;
    private ToggleGroup _toggleGroup { get; set; }
    private ToggleGroup toggleGroup => _toggleGroup ??= GetComponent<ToggleGroup>();
    private Transform _container { get; set; }
    private Transform container => _container ??= transform.GetChild(0);
    #endregion
}
