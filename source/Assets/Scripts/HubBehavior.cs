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

public class TestArg
{

}

public sealed class NoPayload
{
    
}
