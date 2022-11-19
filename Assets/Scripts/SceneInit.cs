using System.Collections.Generic;
using UnityEngine;
using FairyGUIArchitecture;

public abstract class SceneInit : CustomMonoBehaviour
{
    [Header("Scene Init")]
    [SerializeField] protected ViewID _viewID;

    protected Dictionary<string, object> _viewParameters;

    protected override void Start()
    {
        base.Start();
        CreateView(_viewParameters);
    }

    protected virtual void CreateView(Dictionary<string, object> viewParameters)
    {
        ViewManager.Instance.CreateView(_viewID, viewParameters);
    }
}