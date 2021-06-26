using System.Collections.Generic;

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