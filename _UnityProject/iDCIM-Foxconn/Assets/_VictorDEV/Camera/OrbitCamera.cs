using UnityEngine;
using UnityEngine.EventSystems;
using VictorDev.Common;

namespace VictorDev.CameraUtils
{
    /// <summary>
    /// Orbit攝影機平滑操控
    /// <para> + 滑鼠左鍵+移動 : 移動攝影機</para>
    /// <para> + 滑鼠右鍵+移動 : 旋轉攝影機</para>
    /// <para> + MoveTargetTo: 移動至目標座標</para>
    /// <para> + FollowTarget: 跟隨目標</para>
    /// <para> + StopFollowing: 停止跟隨目標</para>
    /// </summary>
    public class OrbitCamera : SingletonMonoBehaviour<OrbitCamera>
    {
        public static bool isMoveable = true;

        [Space(10)]
        public Transform target; // 初始目标
        public float distance = 12f; // 初始距离
        public float xSpeed = 120f; // 水平旋转速度
        public float ySpeed = 120f; // 垂直旋转速度
        public float yMinLimit = -20f; // 垂直旋转的最小限制
        public float yMaxLimit = 80f; // 垂直旋转的最大限制
        public float distanceMin = 2f; // 最小距离
        public float distanceMax = 12f; // 最大距离
        public float smoothTime = 0.07f; // 相机平滑时间 (0.07f為最佳效果，不建議更改)
        public float moveSmoothTime = 0.2f; // 目标移动平滑时间
        public float moveSpeed = 20f; // 目标物体的移动速度
        public float scrollSpeed = 10f; // 鼠标滚轮缩放速度
        public float keyboardMoveSpeed = 6f; // 键盘移动速度

        private float x = 0f;
        private float y = 0f;
        private float currentDistance;
        private Vector3 targetPosition;
        private Vector3 targetVelocity = Vector3.zero;

        private Transform followTarget; // 跟随的目标对象
        private bool isFollowing = false; // 是否在跟随模式

        /// <summary>
        /// 記錄原始SmoothTime，調整數值以處理移動時鏡頭卡頓問題
        /// </summary>
        private float originalSmoothTime { get; set; }

        private void OnValidate()
        {
            originalSmoothTime = smoothTime;
        }

        void Start()
        {
            originalSmoothTime = smoothTime;

            if (target == null)
                return;

            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            currentDistance = distance;
            targetPosition = target.position;
        }

        // 检测鼠标点击是否在UI上
        private bool isClickOnUI { get; set; } = false;

        private bool IsMouseInScreen()
        {
            Vector3 mousePosition = Input.mousePosition;
            return mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
                   mousePosition.y >= 0 && mousePosition.y <= Screen.height;
        }

        void LateUpdate()
        {
            if (IsMouseInScreen() == false) return;

            if (target == null)
                return;

            if (EventHandler.IsUsingInputField) return;

            // 检测鼠标中键是否按下以取消跟随
            if (isFollowing && Input.GetMouseButtonDown(2)) StopFollowing();

            //偵測鼠標在UI組件與3D空間之間的行為
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) isClickOnUI = true;
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) isClickOnUI = false;

            if (isClickOnUI == false)
            {
                // 右键旋转相机
                if (Input.GetMouseButton(1))
                {
                    if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) smoothTime = originalSmoothTime;
                    x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                    y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
                }

                // 左键水平移动目标
                if (Input.GetMouseButton(0))
                {
                    smoothTime = originalSmoothTime * 10;
                    MoveTargetWithMouse();
                }

                // 左键水平移动目标
                if (Input.GetMouseButtonUp(0))
                {
                    smoothTime = originalSmoothTime;
                }

                // 键盘移动控制
                MoveTargetWithKeyboard();
            }
            // 如果在跟随模式下，持续跟随目标
            if (isFollowing && followTarget != null) targetPosition = followTarget.position;

            // 平滑移动到目标位置
            target.position = Vector3.SmoothDamp(target.position, targetPosition, ref targetVelocity, moveSmoothTime);

            // 鼠标滚轮调整距离
            float scroll = (EventSystem.current.IsPointerOverGameObject())? 0:  Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0) smoothTime = originalSmoothTime;
            currentDistance = Mathf.Clamp(currentDistance - scroll * scrollSpeed, distanceMin, distanceMax);

            // 计算相机的位置和旋转
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -currentDistance);
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothTime);

            // 确保相机面向目标
            transform.LookAt(target.position);
        }

        /// <summary>
        /// 鼠标左键水平移动目标
        /// </summary>
        private void MoveTargetWithMouse()
        {
            if (isMoveable == false) return;

            float moveX = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            float moveY = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;

            Vector3 forward = transform.forward;
            forward.y = 0;

            Vector3 right = transform.right;
            right.y = 0;

            Vector3 moveDirection = right * moveX + forward * moveY;
            targetPosition += moveDirection;
        }

        /// <summary>
        /// 键盘移动目标
        /// </summary>
        private void MoveTargetWithKeyboard()
        {
            Vector3 forward = transform.forward;
            forward.y = 0; // 防止向上或向下移动
            forward.Normalize();

            Vector3 right = transform.right;
            right.y = 0;
            right.Normalize();

            float turbo = Input.GetKey(KeyCode.LeftShift) ? 5f : 1f;
            turbo = 1f;

            if (isMoveable == false) return;
            // WASD 控制前后左右移动
            if (Input.GetKey(KeyCode.W))
            {
                targetPosition += forward * keyboardMoveSpeed * Time.deltaTime * turbo;
            }
            if (Input.GetKey(KeyCode.S))
            {
                targetPosition -= forward * keyboardMoveSpeed * Time.deltaTime * turbo;
            }
            if (Input.GetKey(KeyCode.A))
            {
                targetPosition -= right * keyboardMoveSpeed * Time.deltaTime * turbo;
            }
            if (Input.GetKey(KeyCode.D))
            {
                targetPosition += right * keyboardMoveSpeed * Time.deltaTime * turbo;
            }

            // E 键控制向上移动
            if (Input.GetKey(KeyCode.E))
            {
                targetPosition += Vector3.up * keyboardMoveSpeed * Time.deltaTime * turbo;
            }

            // Q 键控制向下移动
            if (Input.GetKey(KeyCode.Q))
            {
                targetPosition -= Vector3.up * keyboardMoveSpeed * Time.deltaTime * turbo;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
            {
                smoothTime = originalSmoothTime * 10;
            }
        }

        /// <summary>
        /// 跟随传入的目标对象
        /// </summary>
        public void FollowTarget(Transform obj)
        {
            if (obj == null)
                return;

            smoothTime = originalSmoothTime * 10;

            followTarget = obj;
            isFollowing = true;
            targetPosition = followTarget.position; // 将目标位置立即更新为新的目标
        }

        /// <summary>
        /// 取消跟随當前目標
        /// </summary>
        public void StopFollowing()
        {
            smoothTime = originalSmoothTime;

            isFollowing = false; // 取消跟随
            followTarget = null;
        }


        /// <summary>
        /// 攝影機置中
        /// </summary>
        public static void MoveToCenter() => Instance.MoveTargetToPos(new Vector3(0, 1, 0));
        /// <summary>
        /// 移动到某个目标位置并旋转相机面向目标
        /// </summary>
        public static void MoveTargetTo(Transform obj, Vector3? posOffset = null) => Instance.MoveTargetToObj(obj, posOffset);
        public static void MoveTargetTo(Vector3 pos, Vector3? possOffset = null) => Instance.MoveTargetToPos(pos);

        /// <summary>
        /// 移动到某个目标位置并旋转相机面向目标
        /// </summary>
        public void MoveTargetToObj(Transform obj, Vector3? posOffset = null)
        {
            if (target == null || obj == null)
                return;

            //先取得對像模型的中心點位置
            Vector3 targetPos = obj.position;
            if (obj.TryGetComponent<Renderer>(out Renderer renderer))
            {
                targetPos = renderer.bounds.center + new Vector3(0, 0.05f, 0);
            }

            Vector3 objForward = obj.forward; // 目标的正Z方向
            Vector3 cameraPosition = targetPos - objForward * currentDistance; // 相机位置在目标的Z轴前方

            MoveTargetToPos(targetPos);

            // 平滑移动相机位置
            transform.position = Vector3.SmoothDamp(transform.position, cameraPosition, ref targetVelocity, moveSpeed);

            // 确保相机朝向目标，並加上偏移值
            Vector3 lookAtPos = targetPos;
            lookAtPos = lookAtPos + (posOffset.HasValue ? posOffset.Value : Vector3.zero);
            transform.LookAt(lookAtPos);
        }

        public void MoveTargetToPos(Vector3 pos)
        {
            targetPosition = pos; // 更新目标位置
        }
    }
}
