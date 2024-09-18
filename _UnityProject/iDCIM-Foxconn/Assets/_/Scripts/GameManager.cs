using System.Collections.Generic;
using UnityEngine;
using VictorDev.CameraUtils;
using VictorDev.Common;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LayerMask layerDefault, layerMouseDown, layerMouseOver;

    private SelectableObject currentSelectObject, lastMouseOverObject;

    [SerializeField] private Material mouseOverMaterial;

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
                    if (currentSelectObject != null)
                    {
                        currentSelectObject.IsShow = false;
                        LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectObject.gameObject, layerDefault);
                    }
                    currentSelectObject = target;
                    currentSelectObject.IsShow = true;

                    LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectObject.gameObject, layerMouseDown);
                    OrbitCamera.MoveTargetTo(currentSelectObject.transform);

                    print($"Hit: {currentSelectObject.name}");
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
