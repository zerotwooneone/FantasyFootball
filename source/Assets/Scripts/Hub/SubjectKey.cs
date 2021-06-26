using System;

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