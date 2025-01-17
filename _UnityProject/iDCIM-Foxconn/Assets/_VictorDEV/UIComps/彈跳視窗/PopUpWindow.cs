using System.Linq;
using UnityEngine;

namespace VictorDev.Advanced
{
    public abstract class PopUpWindow : MonoBehaviour
    {
        protected void ToShow() => BlackScreen?.SetActive(true);

        public void ToClose()
        {
            gameObject.SetActive(false);
            
            BlackScreen?.SetActive(false);
            
            /*bool hasActiveChild = transform.parent.transform.Cast<Transform>()
                .Any(child => child.gameObject.activeSelf && child.gameObject != BlackScreen);

            if (hasActiveChild == false) BlackScreen?.SetActive(false);*/
        }

        private GameObject BlackScreen => _blackScreen ??= transform.parent.Find("BlackScreen")?.gameObject;
        private GameObject _blackScreen;
    }
}