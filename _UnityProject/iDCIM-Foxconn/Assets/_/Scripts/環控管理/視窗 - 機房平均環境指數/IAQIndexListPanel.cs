using System.Collections.Generic;
using UnityEngine;

public class IAQIndexListPanel : MonoBehaviour
{
    [SerializeField] private List<GridItem_IAQIndex> indexBtns;
    public IAQ_IndexDetailPanel detailPanel;

    private void Start()
    {
        ListenGridItemsEvent();
    }

    private void ListenGridItemsEvent()
    {
        indexBtns.ForEach(item => item.onClick.AddListener(detailPanel.ShowData));
    }
}
