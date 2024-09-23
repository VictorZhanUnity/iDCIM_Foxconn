using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 安全監視小地圖
/// </summary>
public class Minimap_CCTV : MonoBehaviour
{
    [SerializeField] public List<Landmark> landmarkList;
    [SerializeField] private ToggleGroup toggleGroup;

    public UnityEvent<int> onClickPin = new UnityEvent<int>();

    private void Awake()
    {
        landmarkList.ForEach(landmark =>
        {
            landmark.toggleGroup = toggleGroup;
            landmark.onToggleChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    onClickPin.Invoke(landmarkList.IndexOf(landmark));
                }
            });
        });
    }
}
