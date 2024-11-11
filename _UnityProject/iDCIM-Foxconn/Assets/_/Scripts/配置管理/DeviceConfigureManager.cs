using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Net.WebAPI;

/// <summary>
/// [配置管理] DeviceConfigure
/// </summary>
public class DeviceConfigureManager : MonoBehaviour
{

    [Header(">>> UI")]
    [SerializeField] GameObject canvasUI;

    [Header(">>> 欲顯示的目標物件")]
    [SerializeField] ModelDisplay modelForDisplay;

    [Header(">>> WebAPI")]
    [SerializeField] DeviceConfigure_WebAPI webAPI;

    public bool isOn
    {
        set
        {
            canvasUI.SetActive(value);
            if (value) ToShow();
            else ToClose();
        }
    }
    private void Start()
    {
        
    }



    private void ToShow()
    {
        MaterialHandler.ReplaceMaterialWithExclude(modelForDisplay.models.ToHashSet());
    }
    private void ToClose()
    {
        MaterialHandler.RestoreOriginalMaterials();
    }

    #region [ContextMenu]
    [ContextMenu("- [WebAPI] 取得所有庫存設備")]
    private void GetAllStockDevice()
    {
        modelForDisplay.FindTargetObjects();
    }

    [ContextMenu("-根據Keywords尋找目標物件")]
    private void FindTargetObjects()
    {
        modelForDisplay.FindTargetObjects();
    }
    #endregion

    [Serializable]
    public class ModelDisplay
    {
        public List<Transform> models;

        [SerializeField]
        private List<string> objKeyWords;

        public void FindTargetObjects()
        {
            models = MaterialHandler.FindTargetObjects(objKeyWords);
        }
    }
}
