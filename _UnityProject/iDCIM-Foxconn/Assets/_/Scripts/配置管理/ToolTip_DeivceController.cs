using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ToolTip_DeivceController : MonoBehaviour
{
    [Header(">>> [資料項] - 設備資料")] private Data_DeviceAsset dataDevice;

    [Header(">>> 點擊搬移項時Invoke")]
    public UnityEvent<Data_DeviceAsset> onClickMoveDeviceEvent = new UnityEvent<Data_DeviceAsset>();
    [Header(">>> 點擊下架時Invoke")]
    public UnityEvent<Data_DeviceAsset> onClickRemoveDeviceEvent = new UnityEvent<Data_DeviceAsset>();

    public void ShowData(Transform model)
    {
        dataDevice = DeviceModelManager_OLD.RackDataList.SelectMany(rack => rack.containers).FirstOrDefault(device => model.name.Contains(device.deviceName));
        gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        if (dataDevice != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(dataDevice.model.position);
        }
    }

    public void ToClose()
    {
        dataDevice = null;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        btnMove.onClick.AddListener(() => onClickMoveDeviceEvent.Invoke(dataDevice));
        btnRemove.onClick.AddListener(() => onClickRemoveDeviceEvent.Invoke(dataDevice));
    }
    private void OnDisable()
    {
        btnMove.onClick.RemoveAllListeners();
        btnRemove.onClick.RemoveAllListeners();
    }

    #region[>>> Componenets]
    private Button _btnMove { get; set; }
    private Button btnMove => _btnMove ??= transform.GetChild(1).GetComponent<Button>();
    private Button _btnRemove { get; set; }
    private Button btnRemove => _btnRemove ??= transform.GetChild(0).GetComponent<Button>();
    #endregion
}