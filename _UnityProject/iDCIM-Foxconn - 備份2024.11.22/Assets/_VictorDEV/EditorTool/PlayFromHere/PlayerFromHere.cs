using UnityEngine;

namespace VictorDev.EditorTool  
{
    public class PlayerFromHere : MonoBehaviour
    {
        [SerializeField] private bool isActivated = true;
        [SerializeField] private Transform playerTrans;
        [SerializeField] private Transform playFromHerePos;
        void Start() => SetPlayerPosition();

        [ContextMenu("測試/傳送玩家至目的地")]
        public void SetPlayerPosition()
        {
            if (isActivated == false && playFromHerePos == null) return;
            playerTrans.position = playFromHerePos.position;
            playerTrans.rotation = playFromHerePos.rotation;
        }

        private void OnValidate()
        {
            name = ">>> PlayFromHere";

            if (isActivated == false) return;
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
            }
            if (GameObject.Find("PlayFromHerePos") != null)
            {
                playFromHerePos = GameObject.Find("PlayFromHerePos").transform;
            }
        }
    }
}
