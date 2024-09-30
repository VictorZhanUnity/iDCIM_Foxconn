using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 設備選單 - IAQ
/// </summary>
public class DeviceList_IAQ : MonoBehaviour
{
    [SerializeField] private DeviceModelVisualizerWithLandmark deviceModelVisualizer_IAQ;
    [SerializeField] private GameObject uiObject;
    [SerializeField] private ToggleGroup toggleGroup;

    [SerializeField] private DeviceListItem_IAQ listItemPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        void CreateListItems(List<SelectableObject> selectableObjects, List<Landmark> landmarks)
        {
            for (int i = 0; i < selectableObjects.Count; i++)
            {
                DeviceListItem_IAQ listItem = ObjectPoolManager.GetInstanceFromQueuePool<DeviceListItem_IAQ>(listItemPrefab, scrollRect.content);
                listItem.label = selectableObjects[i].name;
                listItem.toggleGroup = toggleGroup;
                listItem.SetupSelectableObjectAndLandmark(selectableObjects[i], landmarks[i]);
            }
        }

        deviceModelVisualizer_IAQ.onInitlializedWithLandMark.AddListener(CreateListItems);
    }
}
