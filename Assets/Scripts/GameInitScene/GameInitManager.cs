using System.Collections;
using UnityEngine;
using FairyGUIArchitecture;

namespace GameInitScene
{
    public class GameInitManager : SceneInit
    {
        private void Start()
        {
            CreateView();
        }

        protected override void SubscribeToMessageHubEvents() { }
    }
}
