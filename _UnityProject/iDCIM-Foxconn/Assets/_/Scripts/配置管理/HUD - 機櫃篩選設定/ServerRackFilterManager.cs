using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ServerRackFilterManager : MonoBehaviour
{
    public iDCIM_ModuleManager deviceConfigureManager;

    [ContextMenu("- 顯示合適的機櫃")]
    public void ShowFilterResult()
    {
        DeviceModelManager.RackDataList.ForEach(data => ChangeHeight(data.model));
    }
    private void ChangeHeight(Transform target)
    {
        int index = Random.Range(0, 2);
        target.DOScaleY(index == 0 ? 0.01f: 1, 0.3f).SetEase(Ease.OutQuad).SetDelay(Random.Range(0f, 0.3f)).SetAutoKill(true);
    }

    /// <summary>
    /// 當改變勾選過濾項目時
    /// </summary>
    private void OnFilterOptionChangeHandler()
    {
        ShowFilterResult();
    }

    #region [>>> Show/Hide]
    private void OnEnable()
    {
        ToggleRemainRuSpace.onValueChanged.AddListener((isOn) => OnFilterOptionChangeHandler());
        ToggleRemainWatt.onValueChanged.AddListener((isOn) => OnFilterOptionChangeHandler());
        ToggleRemainWeight.onValueChanged.AddListener((isOn) => OnFilterOptionChangeHandler());
    }

    private void OnDisable()
    {
        ToggleRemainRuSpace.onValueChanged.RemoveAllListeners();
        ToggleRemainWatt.onValueChanged.RemoveAllListeners();
        ToggleRemainWeight.onValueChanged.RemoveAllListeners();
    }
    #endregion

    #region [>>> Components]
    private Transform _VLayout { get; set; }
    private Transform VLayout => _VLayout ??= transform.Find("Container").Find("VLayout");
    private Toggle _ToggleRemainRuSpace { get; set; }
    private Toggle ToggleRemainRuSpace => _ToggleRemainRuSpace ??= VLayout.Find("ToggleRemainRuSpace").GetComponent<Toggle>();
    private Toggle _ToggleRemainWatt { get; set; }
    private Toggle ToggleRemainWatt => _ToggleRemainWatt ??= VLayout.Find("ToggleRemainWatt").GetComponent<Toggle>();
    private Toggle _ToggleRemainWeight { get; set; }
    private Toggle ToggleRemainWeight => _ToggleRemainWeight ??= VLayout.Find("ToggleRemainWeight").GetComponent<Toggle>();
    private bool isFilterRuSpace => ToggleRemainRuSpace.isOn;
    private bool isFilterWatt => ToggleRemainWatt.isOn;
    private bool isFilterWeight => ToggleRemainWeight.isOn;
    #endregion
}
