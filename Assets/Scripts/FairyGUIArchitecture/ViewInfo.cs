using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FairyGUIArchitecture
{
    [System.Serializable]
    public class ViewInfo
    {
        public ViewManager.ViewID ViewID => _viewID;
        public string PackageName => _packageName;
        public string ViewName => _viewName;

        [SerializeField] ViewManager.ViewID _viewID;
        [SerializeField] string _packageName;
        [SerializeField] string _viewName;
    }
}