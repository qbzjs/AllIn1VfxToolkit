using System;
using Easy.MessageHub;

public class MessageHubSingleton : Singleton<MessageHubSingleton>
{
    IMessageHub _messageHub = new MessageHub();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void Publish<T>(T eventClass)
    {
        _messageHub.Publish<T>(eventClass);
    }

    public Guid Subscribe<T>(Action<T> action)
    {
        return _messageHub.Subscribe<T>(action);
    }

    public void Unsubscribe(Guid guid)
    {
        _messageHub.Unsubscribe(guid);
    }
}