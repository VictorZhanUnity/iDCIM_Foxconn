using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 10f;  // 滚轮缩放速度
    [SerializeField] private float minDistance = 5f;  // 最小距离限制
    [SerializeField] private float maxDistance = 50f; // 最大距离限制

    private Camera cam;  // 摄像机组件

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("请将脚本附加到一个拥有 Camera 组件的 GameObject 上。");
        }
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (cam == null) return;

        // 获取鼠标滚轮输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 如果有滚轮输入，则根据缩放速度移动摄像机
        if (scrollInput != 0)
        {
            // 计算目标位置
            Vector3 forwardDirection = cam.transform.forward;
            Vector3 newPosition = cam.transform.position + forwardDirection * scrollInput * zoomSpeed;

            // 限制摄像机与原点的距离
            float distance = Vector3.Distance(newPosition, Vector3.zero); // 可根据需要更改原点
            if (distance >= minDistance && distance <= maxDistance)
            {
                cam.transform.position = newPosition;
            }
        }
    }
}
