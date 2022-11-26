using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class PlatformScaling : CustomMonoBehaviour
    {
        GameManager _gameManager => GameManager.Instance;

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<SetPositionAndScaleEvent>((e) => SetPositionAndScale());
        }

        private void SetPositionAndScale()
        {
            float position = (_gameManager.GameSize - 1f) / 2f;
            float scale = _gameManager.GameSize / 10f;

            transform.localPosition = new Vector3(position, transform.localPosition.y, position);
            transform.localScale = new Vector3(scale, transform.localScale.y, scale);
        }

        public class SetPositionAndScaleEvent { }
    }
}

