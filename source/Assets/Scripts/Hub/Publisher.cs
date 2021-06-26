using System;

public static class Publisher
{
    public static event EventHandler<SubjectArg> RequestPublished;
    public static bool IsHubAwake { get; set; }

    public static void PublishRequest<T>(SubjectKey key,
        T payload)
    {
        var arg = SubjectArg.Factory(key.SubjectName, payload);
        RequestPublished?.Invoke(null, arg);
    }
}