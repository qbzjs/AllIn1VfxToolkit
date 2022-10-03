using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace FairyGUIArchitecture
{
    // TODO : Make as a singleton, such that there can only be one.
    // TODO : Every scene has this ViewManager, but checks if already exists in the DontDestroyOnLoad.
    // TODO : If scene already has, then destroy self GameObject.
    public class ViewManager : MonoBehaviour
    {
        public enum ViewID { NONE = -1, GameInit, COUNT }

        [SerializeField] List<ViewInfo> _viewInfoList;

        Dictionary<ViewID, GComponent> _activeViews = new Dictionary<ViewID, GComponent>();

        IEnumerator Start()
        {
            DontDestroyOnLoad(this);

            PackageManager.BindAllPackages();

            while (true)
            {
                // TODO : make a SceneInit abstract class, and requires a ViewID for the default UI for the scene
                CreateView(ViewID.GameInit);
                yield return new WaitForSeconds(1);
                RemoveView(ViewID.GameInit);
                yield return new WaitForSeconds(1);
            }
        }

        // TODO : summary
        public GComponent CreateView(ViewID viewID)
        {
            ViewInfo viewInfo = GetViewInfo(viewID);

            if (_activeViews.ContainsKey(viewID))
            {
                Debug.LogError($"ViewManager.CreateView(): The view '{viewID.ToString()}' is already active!");
                return null;
            }

            PackageManager.AddPackage(viewInfo.PackageName);
            GComponent view = InstantiateView(viewInfo.PackageName, viewInfo.ViewName);
            FitScreen(view);

            _activeViews.Add(viewID, view);

            return view;
        }

        // TODO : summary
        public void RemoveView(ViewID viewID)
        {
            ViewInfo viewInfo = GetViewInfo(viewID);

            if (!_activeViews.ContainsKey(viewID))
            {
                Debug.LogError($"ViewManager.RemoveView(): The view '{viewID.ToString()}' is not active!");
                return;
            }

            PackageManager.RemovePackage(viewInfo.PackageName);

            GComponent view;
            _activeViews.Remove(viewID, out view);

            DestroyView(view);
        }

        // TODO : summary
        private ViewInfo GetViewInfo(ViewID viewID)
        {
            ViewInfo viewInfo = _viewInfoList.Find((x) => x.ViewID == viewID);
            if (viewInfo is null)
            {
                Debug.LogError($"ViewManager.GetViewInfo(): There is no viewInfo that has the viewID '{viewID}'");
                return null;
            }
            return viewInfo;
        }


        /// <summary>
        /// Instantiates the view component into the Unity scene.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="viewComponentName"></param>
        private static GComponent InstantiateView(string packageName, string viewComponentName)
        {
            GComponent view = UIPackage.CreateObject(packageName, viewComponentName).asCom;
            GRoot.inst.AddChild(view);

            return view;
        }

        /// <summary>
        /// Destroys the view component from the Unity scene.
        /// </summary>
        /// <param name="view"></param>
        private static void DestroyView(GComponent view)
        {
            GRoot.inst.RemoveChild(view);
            view.Dispose();
        }

        /// <summary>
        /// Sets the size of the view component to fit the screen size.
        /// The scale factor is based on the UIContentScaler.
        /// Be sure to add the UIContentScaler component into one of the GameObjects in the scene.
        /// </summary>
        /// <param name="view">The view component.</param>
        private static void FitScreen(GComponent view)
        {
            int width = Screen.width;
            int height = Screen.height;
            width = Mathf.CeilToInt(width / UIContentScaler.scaleFactor);
            height = Mathf.CeilToInt(height / UIContentScaler.scaleFactor);
            view.SetSize(width, height);
            view.SetXY(0, 0, true);
        }
    }
}