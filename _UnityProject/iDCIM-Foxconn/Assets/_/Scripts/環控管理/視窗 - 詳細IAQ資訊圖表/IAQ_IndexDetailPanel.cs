using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAQ_IndexDetailPanel : MonoBehaviour
{
    [SerializeField] private Image imgICON;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private DoTweenFadeController doTweenFadeController;

    public void ShowData(GridItem_IAQIndex item)
    {
        imgICON.sprite = item.imgICON_Sprite;
        txtTitle.SetText(item.columnName);

        doTweenFadeController.FadeIn();
    }

    public void Close()
    {
        doTweenFadeController.FadeOut();
    }
}
