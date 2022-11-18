using UnityEngine;

namespace FairyGUIArchitecture
{
    [System.Serializable]
    public struct FontInfo
    {
        public string FontName;
        [Tooltip("The relative path to the resources folder.")]
        public string ResourcePath;
    }
}