using System.Collections;
using System.Collections.Generic;
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
        txtTitle.SetText(item.columnName + " - 詳細資訊圖表");

        doTweenFadeController.FadeIn();
    }
}
