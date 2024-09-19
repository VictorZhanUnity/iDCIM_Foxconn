using System.Collections.Generic;
using UnityEngine;
using VictorDev.CameraUtils;
using VictorDev.Common;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private LayerMask layerDefault, layerMouseDown, layerMouseOver;

    public SelectableObject currentSelectedObject, lastMouseOverObject;

    [SerializeField] private Material mouseOverMaterial;

    /// <summary>
    ///�_���ܪ��󪺪��A
    /// </summary>
    public static void RestoreSelectedObject()
    {
        SelectableObject target = Instance.currentSelectedObject;

        if(target != null)
        {
            target.IsOn = false;
        }
    }
 


    void Update()
    {
        MouseDownHandler();
        //MouseOverHandler();
    }

    private void MouseDownHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<Transform> result = RaycastManager.RaycastHitObjects();
            if (result.Count > 0)
            {
                if (result[0].TryGetComponent<SelectableObject>(out SelectableObject target))
                {
                    if (currentSelectedObject != null)
                    {
                        currentSelectedObject.IsOn = false;
                        LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectedObject.gameObject, layerDefault);

                        //�Y�I�אּ�P�@�Ӫ���A�h�i�����/���ä�����
                        if (currentSelectedObject == target)
                        {
                            currentSelectedObject = null;
                            return;
                        }
                    }
                    currentSelectedObject = target;
                    currentSelectedObject.IsOn = true;

                    LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectedObject.gameObject, layerMouseDown);
                    OrbitCamera.MoveTargetTo(currentSelectedObject.transform);

                    print($"Hit: {currentSelectedObject.name}");
                }
            }
        }
    }

    /* private void MouseOverHandler()
     {
         List<Transform> result = RaycastManager.RaycastHitObjects();
         if (result.Count > 0)
         {
             if (result[0].gameObject == currentSelectObject) return;

             if (lastMouseOverObject != null) LayerMaskHandler.SetGameObjectLayerToLayerMask(lastMouseOverObject, layerDefault);
             lastMouseOverObject = result[0].gameObject;
             LayerMaskHandler.SetGameObjectLayerToLayerMask(lastMouseOverObject, layerMouseOver);
         }
     }*/
}
