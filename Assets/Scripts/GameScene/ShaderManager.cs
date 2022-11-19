using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class ShaderManager : CustomMonoBehaviour
    {
        [Header("Shader Manager")]
        [SerializeField] GameManager _gameManager;
        [SerializeField] List<Material> _clippingMaterials = new List<Material>();

        [Header("Default Values")]
        [SerializeField] Vector3 _xMinPlane;
        // [SerializeField] Vector3 _xMaxPlane;

        // [SerializeField] Vector3 _yMinPlane;
        [SerializeField] Vector3 _yMaxPlane;

        [SerializeField] Vector3 _zMinPlane;
        // [SerializeField] Vector3 _zMaxPlane;


        protected override void Start()
        {
            base.Start();

            foreach (Material clippingMaterial in _clippingMaterials)
            {
                clippingMaterial.SetVector("_X_Min_Plane_Position", new Vector3(-5 + _gameManager.GameSize, 0, 0));
                clippingMaterial.SetVector("_Y_Max_Plane_Position", new Vector3(0, -0.5f + _gameManager.GameSize, 0));
                clippingMaterial.SetVector("_Z_Min_Plane_Position", new Vector3(0, 0, -5 + _gameManager.GameSize));
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            DebugLog("Resetting clipping material parameters to default values...");
            foreach (Material clippingMaterial in _clippingMaterials)
            {
                clippingMaterial.SetVector("_X_Min_Plane_Position", _xMinPlane);
                clippingMaterial.SetVector("_Y_Max_Plane_Position", _yMaxPlane);
                clippingMaterial.SetVector("_Z_Min_Plane_Position", _zMinPlane);
            }
        }
    }
}

