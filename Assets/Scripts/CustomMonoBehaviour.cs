using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomMonoBehaviour : MonoBehaviour
{
    [Header("Custom MonoBehaviour")]
    [SerializeField] protected bool _enableDebugLogs = true;

    protected List<Guid> _tokenList = new List<Guid>();

    protected virtual void Start()
    {
        SubscribeToMessageHubEvents();
    }

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
    protected virtual void SubscribeToMessageHubEvents() { }

    protected void PublishMessageHubEvent<T>(T eventClass)
    {
        MessageHubSingleton.Instance.Publish<T>(eventClass);
    }

    protected void SubscribeToMessageHubEvent<T>(Action<T> action)
    {
        _tokenList.Add(MessageHubSingleton.Instance.Subscribe<T>(action));
    }

    protected void DebugLog(object message, bool forceLog = false)
    {
        if (_enableDebugLogs || forceLog) Debug.Log($"{GetType().ToString()}: {message.ToString()}");
    }

    protected void DebugLogError(object message, bool forceLog = false)
    {
        if (_enableDebugLogs || forceLog) Debug.LogError($"{GetType().ToString()}: {message.ToString()}");
    }
}