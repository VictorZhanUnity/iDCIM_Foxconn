using UnityEngine;
using VictorDev.Common;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // [SerializeField] private UIManager_DeviceAsset deviceAssetManager;

    [SerializeField] private Canvas _mainCanvas;
    public static Canvas mainCanvas => Instance._mainCanvas;

    private void Awake()
    {
        Setup_RaycastAndLandmark();
    }

    /// <summary>
    /// 設定RaycastHit管理器與圖標管理器之間的事件互動
    /// <para>+ 當點擊/取消圖標Toggle時 → 設定RaycastHit目標模型.isOn，invoke點選/取消事件</para>
    /// <para>+ 當點擊/取消RaycastHit目標模型時 → 設定圖標Toggle.isOn，不進行invoke點選/取消事件</para>
    /// </summary>
    private void Setup_RaycastAndLandmark()
    {
        LandmarkManager_Ver3.onToggleValueChanged.AddListener((isOn, targetModel) =>
        {
            if (isOn) RaycastHitManager.ToSelectTarget(targetModel);
            else RaycastHitManager.CancellObjectSelected(targetModel);
        });
        void func(Transform targetModel, bool isOn)
        {
            LandmarkManager_Ver3.SetLandmarkIsOn(targetModel, isOn);
        }

        RaycastHitManager.onSelectObjectEvent.AddListener((targetModel) => func(targetModel, true));
        RaycastHitManager.onDeselectObjectEvent.AddListener((targetModel) => func(targetModel, false));
    }

    private void Start()
    {
        /*    LandmarkManager_RE.onToggleOnEvent.AddListener((isOn, landmark) =>
            {
                if (isOn) RaycastHitManager.ToSelectTarget(landmark.targetModel, false);
                else RaycastHitManager.CancellObjectSelected(landmark.targetModel, false);
            });
            RaycastHitManager.onSelectObjectEvent.AddListener((target) => LandmarkManager_RE.SetToggleOnWithoutNotify(target));
            RaycastHitManager.onDeselectObjectEvent.AddListener((target) => LandmarkManager_RE.SetToggleOnWithoutNotify(target, false));
            RaycastHitManager.onSelectObjectEvent.AddListener(deviceAssetManager.OnSelectDeviceAsset);*/
    }

    /* /// <summary>
     /// 管理者登入 (登入後隨即抓取全部設備，先寫在一起)
     /// </summary>
     public void Login(string account, string password)
     {
         //void onGetAllDevice(long responseCode, string jsonString) => dcrManager.Parse_AllDCRInfo(jsonString);
         void onGetAllDevice(long responseCode, string jsonString) => print(jsonString);
         void onFailed(long responseCode, string msg) => Debug.LogWarning($"\t\tonFailed [{responseCode}] - msg: {msg}");
         WebAPIManager.SignIn(account, password, onGetAllDevice, onFailed);
     }*/

    public void CloseApp() => Application.Quit();
}
