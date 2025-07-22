using System.Collections.Generic;

public enum EventName
{
    OnPlayerResetMovement
}

public static class EventManager
{
    public delegate void EventReceiver(params object[] parameter);

    private static Dictionary<EventName, EventReceiver> _events = new();
    
    public static void Subscribe(EventName eventType, EventReceiver method)
    {
        if (!_events.TryAdd(eventType, method))
            _events[eventType] += method;
    }

    public static void UnSubscribe(EventName eventType, EventReceiver method)
    {
        if (!_events.ContainsKey(eventType)) return;
        _events[eventType] -= method;

        if (_events[eventType] == null)
            _events.Remove(eventType);
    }
    
    public static void Trigger(EventName eventType, params object[] parameters)
    {
        if (_events.ContainsKey(eventType))
            _events[eventType](parameters);
    }

    public static void ResetEventDictionary()
    {
        _events = new Dictionary<EventName, EventReceiver>();
    }
}