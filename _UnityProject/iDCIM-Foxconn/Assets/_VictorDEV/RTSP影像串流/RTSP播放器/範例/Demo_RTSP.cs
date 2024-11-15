using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.RTSP;

public class Demo_RTSP : MonoBehaviour
{
    public RtspScreen rtspScreenPrefab;
    public Transform gridContainer;
    public List<Button> buttonList = new List<Button>();

    private string url = "rtsp://admin:Pass1234@192.168.201.{0}/stream0";

    private void Start()
    {
        buttonList.ForEach(btn => btn.onClick.AddListener(() => OnClickBtnHandler(btn)));
    }

    private void OnClickBtnHandler(Button btn)
    {
        RtspScreen player = Instantiate(rtspScreenPrefab, gridContainer);
        player.Play(string.Format(url, btn.name.Split("-")[1].Trim()));
    }
}
