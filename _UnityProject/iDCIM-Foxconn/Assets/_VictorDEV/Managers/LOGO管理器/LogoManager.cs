using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.RTSP;

namespace VictorDev.Common
{
    /// <summary>
    /// 公司Logo管理器
    /// </summary>
    public class LogoManager : MonoBehaviour
    {
        [Header(">>> [接收器ILogoReceiver]")]
        [SerializeField] private List<MonoBehaviour> receiveList;

        [Header(">>> [ScriptableObject] 公司LOGO設定, 以第一個資料項做為顯示")]
        [SerializeField] private List<SoData_Logo> logoList;
        private SoData_Logo selectedLogo => logoList[0];

        private void Start() => InovkeData();

        [ContextMenu("- 發送資料")]
        private void InovkeData()
        {
            receiveList.OfType<ILogoReceiver>().ToList().ForEach(receiver => receiver.Receive(selectedLogo));
        }

        private void OnValidate()
        {
            receiveList = ObjectHandler.CheckTypoOfList<ILogoReceiver>(receiveList);
            InovkeData();
        }

        public interface ILogoReceiver
        {
            void Receive(SoData_Logo logo);
        }
    }
}
