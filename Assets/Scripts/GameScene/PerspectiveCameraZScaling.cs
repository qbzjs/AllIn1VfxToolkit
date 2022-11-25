using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class PerspectiveCameraZScaling : CustomMonoBehaviour
    {
        public static int SizeReference => SIZE_REFERENCE;
        public const int SIZE_REFERENCE = 10;

        [SerializeField] GameManager _gameManager;

        [Header("Perspective Camera Settings")]
        [SerializeField] float _zReference = -32f;

        [Header("Orthographic Camera Settings")]
        [SerializeField] MeshRenderer _platformMeshRenderer;
        [SerializeField] float _cameraHeight = 20f; // The intended fixed y position
        [SerializeField] float _orthographicSizeFactor = 0.5f; // Converts game size to orthographic size

        Camera _thisCamera;
        cmdwtf.UnityTools.CameraProjectionBlender _cameraProjectionBlender;

        private void Awake()
        {
            // transform.parent = null;
        }

        protected override void Start()
        {
            base.Start();

            _thisCamera = GetComponent<Camera>();
            _cameraProjectionBlender = GetComponent<cmdwtf.UnityTools.CameraProjectionBlender>();

            _cameraProjectionBlender.orthographicSize = CameraManager.CalculateFlatCameraOrthographicize(_gameManager.GameSize, _orthographicSizeFactor);
            _cameraProjectionBlender.fieldOfView = _thisCamera.fieldOfView;

            // StartCoroutine(SetCamera());

            // if (_gameManager.GameDimension == Snake4D.Dimension.DimensionTwo)
            //     StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            yield return null;

            DebugLog("Transition in 3...");
            yield return new WaitForSeconds(1f);
            DebugLog("2...");
            yield return new WaitForSeconds(1f);
            DebugLog("1...");
            yield return new WaitForSeconds(1f);
            DebugLog("Transitioning!");

            _cameraProjectionBlender.Perspective();
        }

        private IEnumerator SetCamera()
        {
            yield return null; // Need to skip a frame before can set the absolute world position directly, especially in DimensionTwo
            _thisCamera = GetComponent<Camera>();

            if (_gameManager.GameDimension == Snake4D.Dimension.DimensionTwo)
            {
                _thisCamera.orthographic = true;
                // transform.localEulerAngles = new Vector3(45, 0, 0);
                transform.localEulerAngles = new Vector3(90, 0, 0);

                transform.position = CameraManager.CalculateFlatCameraWorldPosition(_platformMeshRenderer, _cameraHeight);
                _thisCamera.orthographicSize = CameraManager.CalculateFlatCameraOrthographicize(_gameManager.GameSize, _orthographicSizeFactor);
            }

            else if (_gameManager.GameDimension == Snake4D.Dimension.DimensionThree || _gameManager.GameDimension == Snake4D.Dimension.DimensionFour)
            {
                _thisCamera.orthographic = false;
                // transform.localEulerAngles = new Vector3(0, 0, 0);
                transform.localEulerAngles = new Vector3(45, 0, 0);

                // float z = CalculateLocalZScaling(_zReference, _gameManager.GameSize);
                // transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
            }

            _cameraProjectionBlender.UpdateInitialFields();
        }

        public static float CalculateLocalZScaling(float zReference, int gameSize)
        {
            return zReference * gameSize / SizeReference;
        }
    }
}

