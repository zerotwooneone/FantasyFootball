using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HubParticipantBehavior : MonoBehaviour
{
    private List<IDisposable> Subscriptions = new List<IDisposable>();

    protected virtual void Awake()
    {
        Subscribe(SubjectKeys.HubAwake, ()=>Debug.Log($"Hub Awake {nameof(HubParticipantBehavior)}"));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Publish<T>(SubjectKey key,
        T payload)
    {
        Publisher.PublishRequest(key, payload);
    }

    protected void Subscribe<T>(SubjectKey key,
        Action<T> handler) where T : class
    {
        var subscription = Subscriber.Subscribe(key, handler);
        Subscriptions.Add(subscription);
    }
    
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
