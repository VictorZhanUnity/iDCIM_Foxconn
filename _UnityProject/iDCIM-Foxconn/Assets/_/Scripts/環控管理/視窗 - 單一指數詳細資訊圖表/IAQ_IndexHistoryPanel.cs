using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

/// <summary>
/// IAQ單一指數歷史資訊面板
/// </summary>
public class IAQ_IndexHistoryPanel : MonoBehaviour
{
    [SerializeField] private Image imgICON;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private DoTweenFadeController doTweenFade;
    [SerializeField] private RectTransformResizeLerp resizer;
    [SerializeField] private Toggle toggleContent;

    public UnityEvent onClose = new UnityEvent();

    public IAQIndexDisplayer data { get; private set; }
    private Vector3 originalPos;


    private void Awake()
    {
        originalPos = doTweenFade.transform.position;

        doTweenFade.OnFadeOutEvent.AddListener(CloseHandler);
    }
    private void CloseHandler()
    {
        toggleContent.isOn = false;
        resizer.Restore();
       ObjectPoolManager.PushToPool<IAQ_IndexHistoryPanel>(this);
    }

    public void ShowData(IAQIndexDisplayer item)
    {
        doTweenFade.transform.position = originalPos;

        data = item;
        imgICON.sprite = item.imgICON_Sprite;
        txtTitle.SetText(item.columnName);

        doTweenFade.FadeIn(true);
    }

    public void Close()
    {
        doTweenFade.FadeOut();
        onClose.Invoke();
    }
}
