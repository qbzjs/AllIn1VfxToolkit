using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Singleton<T>.Instance != null && Singleton<T>.Instance != this as T)
        {
            Destroy(this);
            return;
        }

        Instance = this as T;
    }
}