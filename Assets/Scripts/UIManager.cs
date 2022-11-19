using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SceneInit
{
    void Start()
    {
        CreateView();
    }

    protected override void SubscribeToMessageHubEvents()
    {
        throw new System.NotImplementedException();
    }
}
