using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Minimap_CCTV : MonoBehaviour
{
    [SerializeField] private List<Landmark> landmarkList;
    [SerializeField] private ToggleGroup toggleGroup;

    public UnityEvent<int> onClickPin = new UnityEvent<int>();

    private void Awake()
    {
        landmarkList.ForEach(landmark => landmark.toggleGroup = toggleGroup);
    }

    public void SetLandMarkWithListItem(ListItem item, int index)
    {
        landmarkList[index].listItem = item;
    }
}
