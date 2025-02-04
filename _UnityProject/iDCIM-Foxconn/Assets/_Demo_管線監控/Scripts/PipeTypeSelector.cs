using System;
using UnityEditor;
using UnityEngine;

/// [抽像類別] - 管線選擇器
public abstract class PipeTypeSelector : MonoBehaviour
{
    [HideInInspector] public PipeType pipeType;
    public Enum PipeName;
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(PipeTypeSelector), true)]
public class PipeTypeSelectorEditor : Editor
{
    private PipeTypeSelector _pipeTypeSelector;

    /// 產生Dropdown選單
    private void CreateDropdown<T>(T enumTypeDefault) where T : struct, Enum
    {
        string labelName = "Pipe Name";
        if (_pipeTypeSelector.PipeName == null ||
            Enum.TryParse(_pipeTypeSelector.PipeName.ToString(), out T result) == false)
        {
            _pipeTypeSelector.PipeName = EditorGUILayout.EnumPopup(labelName, enumTypeDefault);
        }
        else
        {
            _pipeTypeSelector.PipeName = EditorGUILayout.EnumPopup(labelName, _pipeTypeSelector.PipeName);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        _pipeTypeSelector = (PipeTypeSelector)target;
        // 顯示 MainType 列舉選擇
        EditorGUILayout.LabelField(">>> 管線種類");
        _pipeTypeSelector.pipeType = (PipeType)EditorGUILayout.EnumPopup("Pipe Type", _pipeTypeSelector.pipeType);

        EditorGUILayout.LabelField(">>> 管線項目");

        // 根據 MainType 顯示對應的子類型選項
        switch (_pipeTypeSelector.pipeType)
        {
            case PipeType.電力:
                CreateDropdown(ElectricityType.高低壓變電站);
                break;
            case PipeType.冷卻水:
                CreateDropdown(CoolingWaterType.冷凝水);
                break;
            case PipeType.空調:
                CreateDropdown(AirConditioningType.一般空調);
                break;
            case PipeType.給排水:
                CreateDropdown(WaterType.生活用水);
                break;
            case PipeType.消防:
                CreateDropdown(FireType.消防用水);
                break;
        }

        // 確保有改動時刷新 Inspector
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        base.OnInspectorGUI();
    }
}
#endif
#endregion