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
    /// �]�wRaycastHit�޲z���P�ϼк޲z���������ƥ󤬰�
    /// <para>+ ���I��/�����ϼ�Toggle�� �� �]�wRaycastHit�ؼмҫ�.isOn�Ainvoke�I��/�����ƥ�</para>
    /// <para>+ ���I��/����RaycastHit�ؼмҫ��� �� �]�w�ϼ�Toggle.isOn�A���i��invoke�I��/�����ƥ�</para>
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
     /// �޲z�̵n�J (�n�J���H�Y��������]�ơA���g�b�@�_)
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
