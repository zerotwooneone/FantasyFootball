using System;
using System.Collections.Generic;

public static class Publisher
{
    public static event EventHandler<SubjectArg> RequestPublished;
    private static List<SubjectArg> _pending = new List<SubjectArg>();
    public static void PublishRequest<T>(SubjectKey key,
        T payload)
    {
        var arg = SubjectArg.Factory(key.SubjectName, payload);
        if (RequestPublished == null)
        {
            _pending.Add(arg);
        }
        else
        {
            RequestPublished.Invoke(null, arg);    
        }
    }

    public static IEnumerable<SubjectArg> Drain()
    {
        if (_pending == null)
        {
            return new SubjectArg[] { };
        }

        var result = _pending;
        _pending = null;
        return result;
    }
}