using System;

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