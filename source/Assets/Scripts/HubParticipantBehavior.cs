using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HubParticipantBehavior : MonoBehaviour
{
    private List<IDisposable> Subscriptions = new List<IDisposable>();

    /// <summary>
    /// Sends a message through the specified subject.
    /// Publish should not be used before MonoBehavior.Start
    /// </summary>
    /// <param name="key">the subjects unique key</param>
    /// <param name="payload">the message to send</param>
    /// <typeparam name="T"></typeparam>
    protected void Publish<T>(SubjectKey key,
        T payload)
    {
        Publisher.PublishRequest(key, payload);
    }

    /// <summary>
    /// Listens for messages on the specified subject.
    /// Subscriptions should be performed as early in the lifecycle as possible, such as in MonoBehaviour.Awake
    /// </summary>
    /// <param name="key">the subjects unique key</param>
    /// <param name="handler">a callback to handle messages</param>
    /// <typeparam name="T"></typeparam>
    protected void Subscribe<T>(SubjectKey key,
        Action<T> handler) where T : class
    {
        var subscription = Subscriber.Subscribe(key, handler);
        Subscriptions.Add(subscription);
    }
    
    /// <summary>
    /// Subscriptions should be performed as early in the lifecycle as possible, such as in <see cref="Awake"/>
    /// </summary>
    protected void Subscribe(SubjectKey key,
        Action handler)
    {
        var subscription = Subscriber.Subscribe(key, handler);
        Subscriptions.Add(subscription);
    }

    protected void OnDestroy()
    {
        foreach (var subscription in Subscriptions)
        {
            try
            {
                subscription.Dispose();
            }
            catch
            {
                Debug.LogWarning("error trying to dispose subscription");
            }
        }
    }
}
