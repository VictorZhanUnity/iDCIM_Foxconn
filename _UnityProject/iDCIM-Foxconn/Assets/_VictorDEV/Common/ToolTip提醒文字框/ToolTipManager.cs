using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

public class ToolTipManager : SingletonMonoBehaviour<ToolTipManager>
{
    [SerializeField] private DeviceAssetToolTip deviceAssetToolTip;

    [Header(">>> UI組件")]
    [SerializeField] private Vector2 offset = new Vector2(40f, 0f);  // ToolTip 與鼠標之間的偏移量
    [SerializeField] private RectTransform canvasRectTransform;

    private IToolTipPanel currentToolTip { get; set; }



    private void Start()
    {
        Instance.currentToolTip = Instance.deviceAssetToolTip;
    }

    /// <summary>
    /// 顯示ToolTip - 資產設備
    /// </summary>
    public static void ShowToolTip_DeviceAsset(Data_iDCIMAsset data)
    {
        Instance.currentToolTip = Instance.deviceAssetToolTip;
        Instance.deviceAssetToolTip.ShowData(data);
    }

    public static void CloseToolTip()
    {
        Instance.currentToolTip?.Close();
        Instance.currentToolTip = null;
    }

    private void Update()
    {
        if (Instance.currentToolTip != null && Instance.currentToolTip.isOn)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        // 將位置限制在 Canvas 範圍內
        Vector2 clampedPosition = ClampToCanvas(Input.mousePosition);
        // 更新 ToolTip Panel 的位置
        currentToolTip.UpdatePosition(clampedPosition);
    }

    /// <summary>
    /// 計算ToolTip所在的Canvas邊界
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector2 ClampToCanvas(Vector2 position)
    {
        Vector2 canvasSize = canvasRectTransform.sizeDelta;
        Vector2 toolTipSize = currentToolTip.sizeDelta;

        // 計算 ToolTip 的四邊界位置
        float minX = 0 + offset.x;
        float maxX = canvasSize.x - toolTipSize.x - offset.x;
        float minY = 0 + toolTipSize.y + offset.y;
        float maxY = canvasSize.y - offset.y;

        // 限制 ToolTip 的位置，使其保持在 Canvas 邊界內
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector2(clampedX + offset.x, clampedY + offset.y);
    }

  
}
