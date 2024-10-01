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
        void CreateDetailPanel(GridItem_IAQIndex iaqIndexItem)
        {
            if (currentIndexDetailPanel != null)
            {
                if (currentIndexDetailPanel.data == iaqIndexItem) return;
                currentIndexDetailPanel.Close();
                currentIndexDetailPanel = null;
            }
            //«Ø¥ßPanel
            IAQ_IndexDetailPanel newPanel = ObjectPoolManager.GetInstanceFromQueuePool<IAQ_IndexDetailPanel>(indexDetailPanelPrefab, detialPanelContaitner);
            newPanel.ShowData(iaqIndexItem);
            newPanel.onClose.AddListener(()=> currentIndexDetailPanel = null);
            currentIndexDetailPanel = newPanel;
        }

        indexBtns.ForEach(item => item.onClick.AddListener(CreateDetailPanel));
    }
}
