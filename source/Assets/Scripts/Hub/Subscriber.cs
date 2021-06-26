using System;
using System.Collections.Generic;
using UnityEngine;

public static class Subscriber
{
    public static Func<SubjectKey,Subject> GetSubject;
    private static readonly List<Action> Pending = new List<Action>();

    public static IDisposable Subscribe(SubjectKey key, Action callback)
    {
        if (key == null)
        {
            throw new ArgumentNullException("key cannot be null", nameof(key));
        }
        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback), "callback cannot be null");
        }
        if (GetSubject == null)
        {
            throw new Exception("subscriber is not set up yet");
        }

        var subject = GetSubject(key);
        EventHandler<SubjectArg> handler = (s,o)=>
        {
            if (o.SubjectKey.Equals(key))
            {
                callback();    
            }
            else
            {
                Debug.LogWarning($"event {o?.SubjectKey} does not match subscription {key}");
            }
            
        };
        subject.Event += handler;
        var subscription = new Subscription(() => subject.Event -= handler);
        return subscription;
    }
    
    public static IDisposable Subscribe<T>(SubjectKey key, Action<T> callback)
    {
        if (key == null)
        {
            throw new ArgumentNullException("key cannot be null", nameof(key));
        }

        if (callback == null)
        {
            throw new ArgumentNullException(nameof(callback), "callback cannot be null");
        }
        var typeName = typeof(T).FullName;
        if (typeName != key.Type)
        {
            throw new ArgumentException($"argument type does not match subject type {key.SubjectName} {key.Type}", nameof(T));
        }
        if (GetSubject == null)
        {
            var pendingSubscription = new PendingSubscription();
            Pending.Add(() =>
            {
                var subject = GetSubject(key);
                var subscription = InnerSubscribe(key, callback, subject);
                pendingSubscription.InnerSubscription = subscription;
            });
            return pendingSubscription;
        }
        
        var subject = GetSubject(key);
        var subscription = InnerSubscribe(key, callback, subject);
        return subscription;
    }

    private static Subscription InnerSubscribe<T>(SubjectKey key, Action<T> callback, Subject subject)
    {
        EventHandler<SubjectArg> handler = (s, o) =>
        {
            if (o.SubjectKey.Equals(key))
            {
                callback((T) o.Payload);
            }
            else
            {
                Debug.LogWarning($"event {o?.SubjectKey} does not match subscription {key}");
            }
        };
        subject.Event += handler;
        var subscription = new Subscription(() => subject.Event -= handler);
        return subscription;
    }

    public static void DrainPending()
    {
        foreach (var action in Pending)
        {
            action();
        }
    }
}

public class PendingSubscription : IDisposable
{
    public void Dispose()
    {
        if (InnerSubscription != null)
        {
            InnerSubscription.Dispose();
        }
    }

    public IDisposable InnerSubscription { get; set; }
}