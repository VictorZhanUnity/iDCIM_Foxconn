using UnityEngine;

/// <summary>
/// 視窗 - 機櫃資訊
/// </summary>
public class RackInfoPanel : MonoBehaviour
{
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private ListItem_Rack listItem;

    private void Start()
    {
        doTweenFadeController.OnFadeOutEvent.AddListener(() =>
        {
            ObjectPoolManager.PushToPool<RackInfoPanel>(this);
        });
    }

    public void SetListItem(ListItem_Rack listItem)
    {
        this.listItem = listItem;
        UpdateUI();
        doTweenFadeController.FadeIn();
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
    }

    public void Close() => doTweenFadeController.FadeOut();
}