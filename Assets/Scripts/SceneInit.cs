using UnityEngine;
using FairyGUIArchitecture;

public abstract class SceneInit : CustomMonoBehaviour
{
    [Header("Scene Init")]
    [SerializeField] protected ViewID _viewID;

    protected virtual void CreateView()
    {
        ViewManager.Instance.CreateView(_viewID);
    }
}