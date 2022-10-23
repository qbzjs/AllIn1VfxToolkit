using UnityEngine;
using FairyGUIArchitecture;

public abstract class SceneInit : CustomMonoBehaviour
{
    [SerializeField] protected ViewID _viewID;

    protected virtual void CreateView()
    {
        ViewManager.Instance.CreateView(_viewID);
    }
}