using UnityEngine;
using VictorDev.Common;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

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

    public void CloseApp() => Application.Quit();
}
