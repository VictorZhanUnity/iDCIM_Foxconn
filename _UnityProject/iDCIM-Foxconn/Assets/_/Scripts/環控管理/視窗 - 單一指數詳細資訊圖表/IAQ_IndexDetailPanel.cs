using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

public class IAQ_IndexDetailPanel : MonoBehaviour
{
    [SerializeField] private Image imgICON;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private DoTweenFadeController doTweenFade;
    [SerializeField] private RectTransformResizeLerp resizer;
    [SerializeField] private Toggle toggleContent;

    public UnityEvent onClose = new UnityEvent();

    public GridItem_IAQIndex data { get; private set; }
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
       ObjectPoolManager.PushToPool<IAQ_IndexDetailPanel>(this);
    }

    public void ShowData(GridItem_IAQIndex item)
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
