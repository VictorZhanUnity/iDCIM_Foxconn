 using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.CameraUtils;
using VictorDev.Common;
using VictorDev.Parser;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private LayerMask layerDefault, layerMouseDown, layerMouseOver;

    public SelectableObject currentSelectedObject, lastMouseOverObject;

    [SerializeField] private Material mouseOverMaterial;


    [Header(">>> ���I���ҫ������Invoke")]
    public UnityEvent<Transform> OnSelectedObject = new UnityEvent<Transform>();

    private void Start()
    {
        Login("TCIT", "TCIT"); 
    }

    /// <summary>
    /// �޲z�̵n�J (�n�J���H�Y��������]�ơA���g�b�@�_)
    /// </summary>
    public void Login(string account, string password)
    {
        //void onGetAllDevice(long responseCode, string jsonString) => dcrManager.Parse_AllDCRInfo(jsonString);
        void onGetAllDevice(long responseCode, string jsonString) => print(jsonString);
        void onFailed(long responseCode, string msg) => Debug.LogWarning($"\t\tonFailed [{responseCode}] - msg: {msg}");
        WebAPIManager.SignIn(account, password, onGetAllDevice, onFailed);
    }

    /// <summary>
    ///�_���ܪ��󪺪��A
    /// </summary>
    public static void RestoreSelectedObject()
    {
        if (Instance.currentSelectedObject != null)
        {
            Instance.currentSelectedObject.IsOn = false;
        }
    }

    public static void ToSelectTarget(Transform target) => Instance.SelectTarget(target);
    public void SelectTarget(Transform target)
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
        currentSelectedObject = target.GetComponent<SelectableObject>();
        currentSelectedObject.SetIsOnWithoutNotify(true);

        LayerMaskHandler.SetGameObjectLayerToLayerMask(currentSelectedObject.gameObject, layerMouseDown);

    }


    void Update()
    {
        MouseDownListener();
        MouseOverAndExitListener();
    }

    private GameObject lastHoveredObject;  // �W�@�����V������
    private GameObject currentHoveredObject;  // ��e���V������

    private void MouseOverAndExitListener()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            currentHoveredObject = hit.collider.gameObject;

            // �ˬd�O�_���V�F���P������
            if (currentHoveredObject != lastHoveredObject)
            {
                // �p�G�W�@��������A�B�w���A���V���AĲ�o MouseExit
                if (lastHoveredObject != null)
                {
                    OnMouseExitHandler(lastHoveredObject);
                }

                // �p�G�{�b���V�F�s������AĲ�o MouseOver
                if (currentHoveredObject != null)
                {
                    OnMouseOverHandler(currentHoveredObject);
                }

                // ��s�W�@�����V������
                lastHoveredObject = currentHoveredObject;
            }
        }
        else
        {
            // �p�GRaycast�S���R������B���e�����V������AĲ�o MouseExit
            if (lastHoveredObject != null)
            {
                OnMouseExitHandler(lastHoveredObject);
                lastHoveredObject = null;  // �M�ŤW�@�����V������
            }
        }
    }

    // ��ƹ����V�����Ĳ�o
    private void OnMouseOverHandler(GameObject hoveredObject)
    {
        if (currentSelectedObject != null)
        {
            if (hoveredObject == currentSelectedObject.gameObject) return;
        }
        // �b�o�̳B�z MouseOver ���޿� (�Ҧp��磌���C��)
        LayerMaskHandler.SetGameObjectLayerToLayerMask(hoveredObject, layerMouseOver);
    }

    // ��ƹ����}�����Ĳ�o
    private void OnMouseExitHandler(GameObject exitedObject)
    {
        if (currentSelectedObject != null)
        {
            if (exitedObject == currentSelectedObject.gameObject) return;
        }
        // �b�o�̳B�z MouseExit ���޿� (�Ҧp�٭쪫���C��)
        LayerMaskHandler.SetGameObjectLayerToLayerMask(exitedObject, layerDefault);
    }

    private void MouseDownListener()
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

                    OnSelectedObject.Invoke(target.transform);

                    Vector3 postOffset = Vector3.zero;
                    if (currentSelectedObject.name.Contains("RACK")) postOffset.y = 10;

                    OrbitCamera.MoveTargetTo(currentSelectedObject.transform, postOffset);
                }
            }
        }
    }


    public void CloseApp() => Application.Quit();

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
