using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public GameObject toolTipPanel;  // ToolTip 背景 Panel
    public TextMeshProUGUI toolTipText;         // ToolTip 顯示文字
    public Vector2 offset = new Vector2(40f, 0f);  // ToolTip 與鼠標之間的偏移量
    private RectTransform toolTipRectTransform;
    private RectTransform canvasRectTransform;

    private void Awake()
    {
        toolTipRectTransform = toolTipPanel.GetComponent<RectTransform>();
        canvasRectTransform = toolTipPanel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        //HideToolTip();
    }

    public void ShowToolTip(string message)
    {
        toolTipText.text = message;
        toolTipPanel.SetActive(true);
    }

    public void HideToolTip()
    {
        toolTipPanel.SetActive(false);
    }

    private void Update()
    {
        if (toolTipPanel.activeSelf)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        // 將位置限制在 Canvas 範圍內
        Vector2 clampedPosition = ClampToCanvas(Input.mousePosition);
        // 更新 ToolTip Panel 的位置
        toolTipPanel.transform.position = clampedPosition;
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        Vector2 canvasSize = canvasRectTransform.sizeDelta;
        Vector2 toolTipSize = toolTipRectTransform.sizeDelta;

        // 計算 ToolTip 的四邊界位置
        float minX = 0;
        float maxX = canvasSize.x - toolTipSize.x;
        float minY = 0 + toolTipSize.y;
        float maxY = canvasSize.y - toolTipSize.y;


        // 限制 ToolTip 的位置，使其保持在 Canvas 邊界內
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector2(clampedX, clampedY);
    }
}
