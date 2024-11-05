using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.CameraUtils;
using VictorDev.Common;
using static VictorDev.RevitUtils.RevitHandler;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private LayerMask layerDefault, layerMouseDown, layerMouseOver;

    public SelectableObject currentSelectedObject, lastMouseOverObject;

    [SerializeField] private Material mouseOverMaterial;
    [SerializeField] private UIManager_DeviceAsset deviceAssetManager;


    [Header(">>> 當點擊模型物件時Invoke")]
    public UnityEvent<Transform> OnSelectedObject = new UnityEvent<Transform>();

    private void Start()
    {
        Login("TCIT", "TCIT");
    }

    /// <summary>
    /// 管理者登入 (登入後隨即抓取全部設備，先寫在一起)
    /// </summary>
    public void Login(string account, string password)
    {
        //void onGetAllDevice(long responseCode, string jsonString) => dcrManager.Parse_AllDCRInfo(jsonString);
        void onGetAllDevice(long responseCode, string jsonString) => print(jsonString);
        void onFailed(long responseCode, string msg) => Debug.LogWarning($"\t\tonFailed [{responseCode}] - msg: {msg}");
        WebAPIManager.SignIn(account, password, onGetAllDevice, onFailed);
    }

    /// <summary>
    ///復原選擇物件的狀態
    /// </summary>
    public static void RestoreSelectedObject()
    {
        if (Instance.currentSelectedObject != null)
        {
            Instance.currentSelectedObject.IsOn = false;
        }
    }

    public static void ToSelectTarget(Transform target) => Instance.SelectTarget(target);
    public void SelectTarget(Transform target)
    {
        if (currentSelectedObject != null)
        {
            currentSelectedObject.IsOn = false;
            LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectedObject.gameObject, layerDefault);

            //若點選為同一個物件，則進行顯示/隱藏之切換
            if (currentSelectedObject == target)
            {
                currentSelectedObject = null;
                return;
            }
        }
        currentSelectedObject = target.GetComponent<SelectableObject>();
        currentSelectedObject.SetIsOnWithoutNotify(true);

        LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectedObject.gameObject, layerMouseDown);

    }


    void Update()
    {
        MouseDownListener();
        MouseOverAndExitListener();
    }

    private GameObject lastHoveredObject;  // 上一次指向的物體
    private GameObject currentHoveredObject;  // 當前指向的物體

    private void MouseOverAndExitListener()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            currentHoveredObject = hit.collider.gameObject;

            // 檢查是否指向了不同的物體
            if (currentHoveredObject != lastHoveredObject)
            {
                // 如果上一次有物體，且已不再指向它，觸發 MouseExit
                if (lastHoveredObject != null)
                {
                    OnMouseExitHandler(lastHoveredObject);
                }

                // 如果現在指向了新的物體，觸發 MouseOver
                if (currentHoveredObject != null)
                {
                    OnMouseOverHandler(currentHoveredObject);
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
                OnMouseExitHandler(lastHoveredObject);
                lastHoveredObject = null;  // 清空上一次指向的物體
            }
        }
    }

    // 當滑鼠指向物體時觸發
    private void OnMouseOverHandler(GameObject hoveredObject)
    {
        if (currentSelectedObject != null)
        {
            if (hoveredObject == currentSelectedObject.gameObject) return;
        }
        // 在這裡處理 MouseOver 的邏輯 (例如更改物體顏色)
        LayerMaskHandler.SetGameObjectLayerToLayerMask(hoveredObject, layerMouseOver);

        //處理ToolTip
        Data_iDCIMAsset data = deviceAssetManager.SearchDeviceAssetByModel(hoveredObject.transform);

        if (data != null)
        {
            if (data.system.ToUpper().Equals("DCS") || data.system.ToUpper().Equals("DCN"))
            {
                Data_DeviceAsset deviceAsset = (Data_DeviceAsset)data;
                ToolTipManager.ShowToolTip_DeviceAsset(data);
            }
        }
    }

    // 當滑鼠離開物體時觸發
    private void OnMouseExitHandler(GameObject exitedObject)
    {
        ToolTipManager.CloseToolTip();
        if (currentSelectedObject != null)
        {
            if (exitedObject == currentSelectedObject.gameObject) return;
        }
        // 在這裡處理 MouseExit 的邏輯 (例如還原物體顏色)
        LayerMaskHandler.SetGameObjectLayerToLayerMask(exitedObject, layerDefault);
    }

    private void MouseDownListener()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<Transform> result = RaycastManager.RaycastHitObjects();
            if (result.Count > 0)
            {
                if (result[0].TryGetComponent<SelectableObject>(out SelectableObject target))
                {
                    if (currentSelectedObject != null)
                    {
                        currentSelectedObject.IsOn = false;
                        LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectedObject.gameObject, layerDefault);

                        //若點選為同一個物件，則進行顯示/隱藏之切換
                        if (currentSelectedObject == target)
                        {
                            currentSelectedObject = null;
                            return;
                        }
                    }
                    currentSelectedObject = target;
                    currentSelectedObject.IsOn = true;

                    LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectedObject.gameObject, layerMouseDown);

                    OnSelectedObject.Invoke(target.transform);

                    Vector3 postOffset = Vector3.zero;
                    if (currentSelectedObject.name.Contains("RACK")) postOffset.y = 10;

                    OrbitCamera.MoveTargetTo(currentSelectedObject.transform, postOffset);
                }
            }
        }
    }


    public void CloseApp() => Application.Quit();

    /* private void MouseOverHandler()
     {
         List<Transform> result = RaycastManager.RaycastHitObjects();
         if (result.Count > 0)
         {
             if (result[0].gameObject == currentSelectObject) return;

             if (lastMouseOverObject != null) LayerMaskHandler.SetGameObjectLayerToLayerMask(lastMouseOverObject, layerDefault);
             lastMouseOverObject = result[0].gameObject;
             LayerMaskHandler.SetGameObjectLayerToLayerMask(lastMouseOverObject, layerMouseOver);
         }
     }*/
}
