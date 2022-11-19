using System.Collections.Generic;
using UnityEngine;
using FairyGUIArchitecture;

public abstract class SceneInit : CustomMonoBehaviour
{
    [Header("Scene Init")]
    [SerializeField] protected ViewID _viewID;

    protected virtual void CreateView(Dictionary<string, object> viewParameters = null)
    {
        ViewManager.Instance.CreateView(_viewID, viewParameters);
    }
}