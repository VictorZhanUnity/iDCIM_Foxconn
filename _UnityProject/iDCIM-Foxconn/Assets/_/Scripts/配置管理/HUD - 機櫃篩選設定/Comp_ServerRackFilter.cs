using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using Random = UnityEngine.Random;

/// <summary>
/// [組件] 機櫃過濾
/// </summary>
public class Comp_ServerRackFilter : MonoBehaviour
{
    [Header(">>> Dotween設定")]
    [SerializeField] private float minScale = 0.001f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease easeOut = Ease.OutBack;
    [SerializeField] private Ease easeIn = Ease.InQuad;

    public Color rackGood, rackNormal, rackBad;

    private Color originalRackColor = ColorHandler.HexToColor(0x181818, 1);

    [ContextMenu("- 顯示合適的機櫃")]
    public void ShowFilterResult()
    {
        DeviceModelManager.RackDataList.ForEach(data =>
        {
            ChangeRackStyle(data.model);
        });
    }
    /// <summary>
    /// 依篩選等級過濾外觀
    /// </summary>
    private void ChangeRackStyle(Transform target)
    {
        bool isScaleOut = Random.Range(0, 2) == 1;
        target.DOScaleY(isScaleOut ? 1 : minScale, isScaleOut ? duration : duration * 0.5f).SetEase(isScaleOut ? easeOut : easeIn).SetDelay(Random.Range(0f, duration)).SetAutoKill(true);


        int materialIndex = target.name.Contains("ATEN") ? 7 : 4;

        Material[] mats = target.GetComponent<MeshRenderer>().materials;

        for (int i = 0; i < mats.Length; i++)
        {
            Color color = mats[i].color;
            if (i == materialIndex)
            {
                if (isScaleOut == false) color = originalRackColor;
                else
                {
                    int index = Random.Range(0, 3);
                    if (index == 0) color = rackGood;
                    else if (index == 1) color = rackNormal;
                    else if (index == 2) color = rackBad;


                }
            }

            color.a = isScaleOut ? 0: 1;
            if (isScaleOut) MaterialHandler.SetTransparentMode(mats[i]);
            else MaterialHandler.SetOpaqueMode(mats[i]);
            mats[i].DOColor(color, duration).SetEase(isScaleOut ? easeOut : easeIn).SetAutoKill(true);
        };
    }


    /// <summary>
    /// 當改變勾選過濾項目時
    /// </summary>
    private void OnFilterOptionChangeHandler()
    {
        ShowFilterResult();
    }



    /// <summary>
    /// 進行機櫃條件過濾
    /// </summary>
    public void ToFilterRack(StockDeviceListItem target)
    {
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
