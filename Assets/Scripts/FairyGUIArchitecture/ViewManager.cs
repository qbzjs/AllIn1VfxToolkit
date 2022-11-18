using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FairyGUI;

namespace FairyGUIArchitecture
{
    public class ViewManager : MonoBehaviour
    {
        public static ViewManager Instance { get; private set; }

        [Header("Packages")]
        [SerializeField] List<ViewInfo> _viewInfoList;

        [Header("Fonts")]
        [SerializeField] FontInfo _defaultFont;
        [SerializeField] List<FontInfo> _otherFonts;

        Dictionary<ViewID, GComponent> _activeViews = new Dictionary<ViewID, GComponent>();
        Dictionary<ViewID, IViewModel> _activeViewModels = new Dictionary<ViewID, IViewModel>();

        private void Awake()
        {
            if (ViewManager.Instance != null && ViewManager.Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            InitFonts();
            PackageManager.BindAllPackages();
        }

        private void InitFonts()
        {
            RegisterFont(_defaultFont);
            UIConfig.defaultFont = _defaultFont.FontName;

            foreach (FontInfo otherFontInfo in _otherFonts)
            {
                RegisterFont(otherFontInfo);
            }
        }

        private void RegisterFont(FontInfo fontInfo)
        {
            Font font = Resources.Load<Font>(fontInfo.ResourcePath);
            FontManager.RegisterFont(new DynamicFont(fontInfo.FontName, font), fontInfo.FontName);
        }

        /// <summary>
        /// Creates and initializes the corresponding view and view model.
        /// </summary>
        /// <param name="viewID">The view ID of the view.</param>
        /// <returns>The created view component.</returns>
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
            _activeViews.Add(viewID, view);
            FitScreen(view);

            IViewModel viewModel = ViewModel.GetViewModel(viewID, view);
            _activeViewModels.Add(viewID, viewModel);

            return view;
        }

        /// <summary>
        /// Removes and destroys the corresponding view and view model.
        /// </summary>
        /// <param name="viewID">The view ID of the view.</param>
        public void RemoveView(ViewID viewID)
        {
            ViewInfo viewInfo = GetViewInfo(viewID);
            if (viewInfo == null) return;

            if (!_activeViews.ContainsKey(viewID))
            {
                Debug.LogError($"ViewManager.RemoveView(): The view '{viewID.ToString()}' is not active!");
                return;
            }

            PackageManager.RemovePackage(viewInfo.PackageName);

            GComponent view;
            _activeViews.Remove(viewID, out view);
            DestroyView(view);

            IViewModel viewModel;
            _activeViewModels.Remove(viewID, out viewModel);
            viewModel.OnDestroy();
        }

        // TODO
        public void ChangeView(ViewID viewID)
        {

        }

        /// <summary>
        /// Gets the view info of the view, such as the package name and the view component name.
        /// </summary>
        /// <param name="viewID">The view ID of the view.</param>
        /// <returns>The view info of the view.</returns>
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
        /// <para>Be sure to add the UIContentScaler component into one of the GameObjects in the scene.</para>
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