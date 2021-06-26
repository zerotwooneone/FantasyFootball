using System;

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