using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameScene
{
    public class DynamicCameraHelper : CustomMonoBehaviour
    {
        public enum TransitionType { ToPerspective, ToOrthographic }
        enum State { Orthographic, InTransition, Perspective }

        [Header("Blend Settings")]
        [SerializeField] cmdwtf.UnityTools.CameraProjectionBlender _cameraProjectionBlender;
        [SerializeField] float _blendDuration;

        [Header("Transform Settings")]
        [SerializeField] Transform _orthoPositionRef;
        [SerializeField] Transform _perspectivePositionRef;
        [SerializeField][Tooltip("Time to delay after launching the blend")] float _delayInterval;
        [SerializeField] float _transformTransitionDuration;

        [Header("Perspective FOV Settings")]
        [SerializeField] float _targetFieldOfView = 37f;
        [SerializeField] float _fovTransitionDuration;

        State _state;

        protected override void Start()
        {
            base.Start();
            _state =
                GameManager.Instance.GameDimension == Snake4D.Dimension.DimensionTwo
                ? State.Orthographic
                : State.Perspective;
        }

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<RequestTransitionEvent>((e) => Transition(e.transitionType));
        }

        private void Transition(TransitionType transitionType)
        {
            if (transitionType == TransitionType.ToPerspective && _state == State.Orthographic)
            {
                _state = State.InTransition;

                _cameraProjectionBlender.defaultBlendDuration = _blendDuration;
                _cameraProjectionBlender.Perspective();

                Sequence sequence = DOTween.Sequence();
                sequence
                    .AppendInterval(_delayInterval) // Wait for the blender to blend a bit first before rotating
                    .Append(
                        transform.DOMove(_perspectivePositionRef.position, _transformTransitionDuration)
                            .SetEase(Ease.InOutQuad)

                    )
                    .Join(
                        transform.DOLocalRotate(new Vector3(-45, 0, 0), _transformTransitionDuration)
                            .SetEase(Ease.InOutQuad)
                    )
                    .Append(
                        DOTween.To(
                            getter: () => _cameraProjectionBlender.fieldOfView,
                            setter: x => _cameraProjectionBlender.fieldOfView = x,
                            endValue: _targetFieldOfView,
                            duration: _fovTransitionDuration
                        ).SetEase(Ease.InOutQuad)
                    )
                    .OnComplete(() => _state = State.Perspective);
            }

            else if (transitionType == TransitionType.ToOrthographic && _state == State.Perspective)
            {
                _state = State.InTransition;
                transform.DOMove(_orthoPositionRef.position, _transformTransitionDuration)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => _state = State.Orthographic);
            }
        }

        public class RequestTransitionEvent
        {
            public TransitionType transitionType;

            public RequestTransitionEvent(TransitionType transitionType)
            {
                this.transitionType = transitionType;
            }
        }
    }

}
