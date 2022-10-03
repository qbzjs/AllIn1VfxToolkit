using UnityEngine;
using FairyGUIArchitecture;

public abstract class SceneInit : MonoBehaviour
{
    [SerializeField] protected ViewManager.ViewID _viewID;

    protected virtual void CreateView()
    {
        ViewManager.Instance.CreateView(_viewID);
    }
}