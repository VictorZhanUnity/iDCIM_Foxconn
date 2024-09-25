using UnityEngine;
using UnityEngine.UI;

public class CCTV_FullScreenPlayer : MonoBehaviour
{
    [SerializeField] private Image cctvScreen;
    [SerializeField] private GameObject canvasObj;

    private void Start()
    {
        canvasObj.SetActive(false);
    }

    public void Show(Sprite texture)
    {
        cctvScreen.sprite = texture;
        canvasObj.SetActive(true);
    }

    public void Hide()
    {
        canvasObj.SetActive(false);
    }
}
