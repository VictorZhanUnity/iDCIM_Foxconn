using System.Collections.Generic;
using UnityEngine;

public class IAQIndexListPanel : MonoBehaviour
{
    [SerializeField] private List<GridItem_IAQIndex> indexBtns;
    [SerializeField] private IAQ_IndexDetailPanel indexDetailPanelPrefab;
    [SerializeField] private Transform detialPanelContaitner;

    public IAQ_IndexDetailPanel currentIndexDetailPanel;

    private void Start()
    {
        ListenGridItemsEvent();
    }

    private void ListenGridItemsEvent()
    {
        void CreateDetailPanel(GridItem_IAQIndex iaqIndex)
        {
            if (currentIndexDetailPanel != null) currentIndexDetailPanel.Close();
            ObjectPoolManager.PushToPool<IAQ_IndexDetailPanel>(detialPanelContaitner);
            currentIndexDetailPanel = ObjectPoolManager.GetInstanceFromQueuePool<IAQ_IndexDetailPanel>(indexDetailPanelPrefab, detialPanelContaitner);
            currentIndexDetailPanel.ShowData(iaqIndex);
        }

        indexBtns.ForEach(item => item.onClick.AddListener(CreateDetailPanel));
    }
}
