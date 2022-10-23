using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomMonoBehaviour : MonoBehaviour
{
    protected List<Guid> _tokenList = new List<Guid>();

    protected virtual void OnDestroy()
    {
        foreach (Guid token in _tokenList)
        {
            MessageHubSingleton.Instance.Unsubscribe(token);
        }
    }

    /// <summary>
    /// Designed to hold multiple SubscribeToMessageHubEvent() method calls.
    /// <para> Generally called in the Start() method. </para>
    /// </summary>
    protected abstract void SubscribeToMessageHubEvents();

    protected void PublishMessageHubEvent<T>(T eventClass)
    {
        MessageHubSingleton.Instance.Publish<T>(eventClass);
    }

    protected void SubscribeToMessageHubEvent<T>(Action<T> action)
    {
        _tokenList.Add(MessageHubSingleton.Instance.Subscribe<T>(action));
    }
}