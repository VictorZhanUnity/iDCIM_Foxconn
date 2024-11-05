using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VictorDev.Common
{
    public class RaycastManager : SingletonMonoBehaviour<RaycastManager>
    {
        private Camera _mainCamera;
        private Camera mainCamera
        {
            get
            {
                _mainCamera ??= Camera.main;
                return _mainCamera;
            }
        }

        /// <summary>
        /// 取得射線經過的3D物件
        /// <para>  + numOfGetObj: 從近到遠要抓多少個物件</para>
        /// <para>  + rayDistance: 射線距離</para>
        /// </summary>
        public static List<Transform> RaycastHitObjects(int numOfGetObj = 1, float rayDistance = float.MaxValue)
        {
            List<Transform> result = new List<Transform>();

            //點擊在UI 元素上
            if (EventSystem.current.IsPointerOverGameObject()) return result;

            // 從攝影機的位置發射射線，從滑鼠位置開始
            Ray ray = Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            // 使用 Physics.RaycastAll 來獲取射線經過的所有物件(並未排序)
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

            // 根據距離從最近到最遠排序
            List<RaycastHit> sortedHits = hits.OrderBy(hit => hit.distance).ToList();

            // 遍歷所有碰撞到的物件
            foreach (RaycastHit hit in sortedHits)
            {
                Transform hitObject = hit.collider.gameObject.transform;
                if (result.Count < numOfGetObj) result.Add(hitObject);
            }
            return result;
        }
    }
}
