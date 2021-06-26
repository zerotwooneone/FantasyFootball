using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubBehavior : MonoBehaviour
{
    private readonly Dictionary<SubjectKey, Subject> Subjects = new Dictionary<SubjectKey, Subject>();

    private void Awake()
    {
        foreach (var key in SubjectKeys.GetAllKeys())
        {
            if (!Subjects.ContainsKey(key))
            {
                var subject = Subject.Factory(key.SubjectName, key.Type);
                Subjects.Add(key,subject);
            }
            else
            {
                Debug.LogWarning($"tried to create existing subject during awake {key.SubjectName} {key.Type}");
            }
        }

        Subscriber.GetSubject = key => Subjects[key];
        Publisher.RequestPublished += OnRequestPublished;
        Publisher.IsHubAwake = true;
        OnRequestPublished(this, SubjectArg.Factory(SubjectKeys.HubAwake));
    }

    private void OnRequestPublished(object sender, SubjectArg arg)
    {
        if (!Subjects.TryGetValue(arg.SubjectKey, out var subject))
        {
            subject = Subject.Factory(arg.SubjectKey.SubjectName, arg.SubjectKey.Type);
            Subjects.Add(arg.SubjectKey, subject);
            Debug.LogWarning($"had to create subject during publish  {arg.SubjectKey.SubjectName} {arg.SubjectKey.Type}");
        }

        subject.Publish(arg.Payload);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Publisher.RequestPublished -= OnRequestPublished;
    }
}

public static class Publisher
{
    public static event EventHandler<SubjectArg> RequestPublished;
    public static bool IsHubAwake { get; set; }

    public static void PublishRequest(SubjectArg e)
    {
        RequestPublished?.Invoke(null, e);
    }
}

public static class Subscriber
{
    public static Func<SubjectKey,Subject> GetSubject;
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
            throw new Exception("subscriber is not set up yet");
        }

        var subject = GetSubject(key);
        EventHandler<SubjectArg> handler = (s,o)=>
        {
            if (o.SubjectKey.Equals(key))
            {
                callback((T)o.Payload);    
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
}

public class Subscription : IDisposable
{
    private readonly Action _action;

    public Subscription(Action action)
    {
        _action = action;
    }

    public void Dispose()
    {
        _action();
    }
}

public class Subject
{
    public string Name { get; }
    public string Type { get; }
    public event EventHandler<SubjectArg> Event;
    protected Subject(string name, string type)
    {
        Name = name;
        Type = type;
    }

    public static Subject Factory(string name, string type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Subject must have a name", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException($"Subject must have a type {name}", nameof(type));
        }
        return new Subject(name, type);
    }

    /*public void Publish(SubjectArg arg)
    {
        if (arg.SubjectName != Name)
        {
            throw new ArgumentException("subject name must match", nameof(arg));
        }

        if (arg.Type != Type)
        {
            throw new ArgumentException("type must match", nameof(arg));
        }
        OnEvent(arg);
    }*/

    public void Publish<T>(T payload)
    {
        if (payload != null && payload.GetType().FullName != Type)
        {
            throw new ArgumentException($"type of payload must match expected:{Type} actual:{typeof(T).FullName}", nameof(payload));
        }
        OnEvent(SubjectArg.Factory(Name, payload));
    }

    protected virtual void OnEvent(SubjectArg e)
    {
        Event?.Invoke(this, e);
    }
}
    
public class SubjectArg
{
    public SubjectKey SubjectKey { get; }
    public object Payload { get; }
    
    protected SubjectArg(SubjectKey subjectKey,
        object payload = null)
    {
        SubjectKey = subjectKey;
        Payload = payload;
    }

    private static SubjectArg InnerFactory(SubjectKey subjectKey,
        object payload = null)
    {
        if (subjectKey == null)
        {
            throw new ArgumentNullException(nameof(subjectKey), "subject must have a key");
        }

        if (payload != null &&
            payload.GetType().FullName != subjectKey.Type)
        {
            throw new ArgumentException("payload must match key type", nameof(payload));
        }
        return new SubjectArg(subjectKey,
            payload);
    }

    public static SubjectArg Factory(SubjectKey key)
    {
        return InnerFactory(key);
    }
    public static SubjectArg Factory(string subjectName)
    {
        return Factory<NoPayload>(subjectName, null);
    }

    public static SubjectArg Factory<T>(string subjectName,
        T payload)
    {
        var typeName = payload == null
        ? typeof(NoPayload).FullName
        : payload.GetType().FullName;
        var subjectKey = SubjectKey.Factory(subjectName,
            typeName);
        return InnerFactory(subjectKey,
            payload);
    }
}

public class SubjectKey
{
    public string SubjectName { get; }
    public string Type { get; }

    protected SubjectKey(string subjectName,
        string type)
    {
        SubjectName = subjectName;
        Type = type;
    }

    public override int GetHashCode()
    {
        const int hashPrime = 17;
        int result = hashPrime;
        unchecked
        {
            result = result * hashPrime + SubjectName.GetHashCode();
            result = result * hashPrime + Type.GetHashCode();
        }

        return result;
    }

    public override bool Equals(object obj)
    {
        var other = obj as SubjectKey;
        if (obj is null || other is null)
        {
            return false;
        }

        if (object.ReferenceEquals(this, obj))
        {
            return true;
        }

        if (this.GetType() != obj.GetType())
        {
            return false;
        }

        return SubjectName == other.SubjectName && Type == other.Type;
    }

    public override string ToString()
    {
        return $"{SubjectName}:{Type}";
    }

    public static SubjectKey Factory(string subjectName,
        string type)
    {
        if (string.IsNullOrWhiteSpace(subjectName))
        {
            throw new ArgumentException("key must have a subject name", nameof(subjectName));
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("key must have a type", nameof(type));
        }

        return new SubjectKey(subjectName, type);
    }
}

public static class SubjectKeys
{
    private static IDictionary<string, SubjectKey> KeysByName = new Dictionary<string, SubjectKey>();
    public static SubjectKey Test = SubjectKeyFactory<TestArg>();
    public static SubjectKey HubAwake = SubjectKeyFactoryNoPayload("HubAwake");

    public static IEnumerable<SubjectKey> GetAllKeys()
    {
        return KeysByName.Values;
    }
    private static SubjectKey SubjectKeyFactory<T>()
    {
        var typeName = typeof(T).FullName;
        if (KeysByName.TryGetValue(typeName, out var existing))
        {
            return existing;
        }

        var newKey = SubjectKey.Factory(typeName, typeName);
        KeysByName.Add(typeName, newKey);
        return newKey;
    }
    private static SubjectKey SubjectKeyFactoryNoPayload(string subjectName)
    {
        var typeName = typeof(NoPayload).FullName;
        if (KeysByName.TryGetValue(typeName, out var existing))
        {
            return existing;
        }

        var newKey = SubjectKey.Factory(subjectName, typeName);
        KeysByName.Add(typeName, newKey);
        return newKey;
    }
}

public class TestArg
{

}

public sealed class NoPayload
{
    
}
