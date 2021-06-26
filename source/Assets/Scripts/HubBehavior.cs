using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubBehavior : MonoBehaviour
{
    private readonly Dictionary<SubjectKey, Subject> Subjects = new Dictionary<SubjectKey, Subject>();

    private void Awake()
    {
        Debug.Log($"{nameof(HubBehavior)} awake");
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
        Subscriber.DrainPending();
        Publisher.RequestPublished += OnRequestPublished;
        
        Publish(SubjectArg.Factory(SubjectKeys.HubAwake));
        var pendingPublished = Publisher.Drain();
        foreach (var pending in pendingPublished)
        {
            Publish(pending);
        }
    }

    private void OnRequestPublished(object sender, SubjectArg arg)
    {
        Publish(arg);
    }

    private void Publish(SubjectArg arg)
    {
        if (!Subjects.TryGetValue(arg.SubjectKey, out var subject))
        {
            subject = Subject.Factory(arg.SubjectKey.SubjectName, arg.SubjectKey.Type);
            Subjects.Add(arg.SubjectKey, subject);
            Debug.LogWarning($"had to create subject during publish  {arg.SubjectKey.SubjectName} {arg.SubjectKey.Type}");
        }

        subject.Publish(arg.Payload);
    }

    private void OnDestroy()
    {
        Publisher.RequestPublished -= OnRequestPublished;
    }
}