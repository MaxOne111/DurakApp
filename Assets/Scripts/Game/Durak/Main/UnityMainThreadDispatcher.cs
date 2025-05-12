using System;
using System.Collections.Concurrent;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
    
    private void Update()
    {
        lock (_actions)
        {
            while (_actions.Count > 0)
                if (_actions.TryDequeue(out var action))
                    action.Invoke();
        }
    }
    
    public static void Enqueue(Action action)
    {
        lock (_actions)
        {
            _actions.Enqueue(action);
        }
    }
}