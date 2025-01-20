using System;
using UnityEngine;
using VictorDev.Common;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    [SerializeField] private Canvas _mainCanvas;
    public static Canvas mainCanvas => Instance._mainCanvas;

    private void Awake()
    {
        Setup_RaycastAndLandmark();
        System.GC.Collect();

    }

    /// <summary>
    /// �]�wRaycastHit�޲z���P�ϼк޲z���������ƥ󤬰�
    /// <para>+ ���I��/�����ϼ�Toggle�� �� �]�wRaycastHit�ؼмҫ�.isOn�Ainvoke�I��/�����ƥ�</para>
    /// <para>+ ���I��/����RaycastHit�ؼмҫ��� �� �]�w�ϼ�Toggle.isOn�A���i��invoke�I��/�����ƥ�</para>
    /// </summary>
    private void Setup_RaycastAndLandmark()
    {
        LandmarkManager_Ver3.onToggleValueChanged.AddListener((isOn, targetModel) =>
        {
            if (isOn) RaycastHitManager.ToSelectTarget(targetModel);
            else RaycastHitManager.CancellObjectSelected(targetModel);
        });
        void func(Transform targetModel, bool isOn)
        {
            LandmarkManager_Ver3.SetLandmarkIsOn(targetModel, isOn);
        }

        RaycastHitManager.onSelectObjectEvent.AddListener((targetModel) => func(targetModel, true));
        RaycastHitManager.onDeselectObjectEvent.AddListener((targetModel) => func(targetModel, false));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) Exit();
    }

    public void Exit() => Application.Quit();
}
