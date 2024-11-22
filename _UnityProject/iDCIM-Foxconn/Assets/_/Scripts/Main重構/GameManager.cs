using UnityEngine;
using VictorDev.Advanced;
using VictorDev.Common;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    [SerializeField] private UIManager_DeviceAsset deviceAssetManager;

    private void Start()
    {
        LandmarkManager_RE.onToggleOnEvent.AddListener((isOn, landmark) =>
        {
            if (isOn) RaycastHitManager.ToSelectTarget(landmark.targetModel, false);
            else RaycastHitManager.CancellObjectSelected(landmark.targetModel, false);
        });
        RaycastHitManager.onSelectObjectEvent.AddListener((target) => LandmarkManager_RE.SetToggleOnWithoutNotify(target));
        RaycastHitManager.onDeselectObjectEvent.AddListener((target) => LandmarkManager_RE.SetToggleOnWithoutNotify(target, false));
        RaycastHitManager.onSelectObjectEvent.AddListener(deviceAssetManager.OnSelectDeviceAsset);
    }

    /// <summary>
    /// �޲z�̵n�J (�n�J���H�Y��������]�ơA���g�b�@�_)
    /// </summary>
    public void Login(string account, string password)
    {
        //void onGetAllDevice(long responseCode, string jsonString) => dcrManager.Parse_AllDCRInfo(jsonString);
        void onGetAllDevice(long responseCode, string jsonString) => print(jsonString);
        void onFailed(long responseCode, string msg) => Debug.LogWarning($"\t\tonFailed [{responseCode}] - msg: {msg}");
        WebAPIManager.SignIn(account, password, onGetAllDevice, onFailed);
    }

    public void CloseApp() => Application.Quit();
}
