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
        if (_viewID == ViewID.NONE)
        {
            DebugLog("ViewID is set to NONE. Not showing any UI.");
            return;
        }

        ViewManager.Instance.CreateView(_viewID, viewParameters);
    }
}