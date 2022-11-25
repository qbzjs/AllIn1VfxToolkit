using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveHolder : MonoBehaviour
{
    [SerializeField] Transform _perspectiveCamTransform;
    [SerializeField] cmdwtf.UnityTools.CameraProjectionBlender _blender;

    Vector3 _initialPosition;
    Vector3 _finalPosition;

    Vector3 _initialRotation;
    Vector3 _finalRotation;

    float _elapsedTimeBetweenUpdateInterval = 0;
    float _lerpTime = 1.8f;

    bool _lerp = false;
    bool _lerpRotation = false;

    IEnumerator Start()
    {
        Debug.Log("Transition in 2 seconds");
        yield return new WaitForSeconds(2f);

        _blender.Perspective();

        yield return new WaitForSeconds(0.2f);

        _initialPosition = transform.position;
        _finalPosition = _perspectiveCamTransform.position;

        _initialRotation = new Vector3(0, 0, 0);
        _finalRotation = new Vector3(-45, 0, 0);

        _elapsedTimeBetweenUpdateInterval = 0;
        _lerp = true;
        _lerpRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTimeBetweenUpdateInterval += Time.deltaTime;
        float interpolationRatio = _elapsedTimeBetweenUpdateInterval / _lerpTime;

        if (_lerp)
        {
            transform.position = Vector3.Lerp(_initialPosition, _finalPosition, interpolationRatio);
        }

        if (_lerpRotation)
        {
            transform.localEulerAngles = Vector3.Lerp(_initialRotation, _finalRotation, interpolationRatio);
        }
    }
}
