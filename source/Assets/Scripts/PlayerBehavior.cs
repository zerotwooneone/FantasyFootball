using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : HubParticipantBehavior
{
    private IEnumerator DelayPublish()
    {
        yield return new WaitForSeconds(2);
        
        Debug.Log($"{nameof(PlayerBehavior)} publishing after delay");
        Publish(SubjectKeys.Test, new TestArg());
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{nameof(PlayerBehavior)} start");
        
        Debug.Log($"{nameof(PlayerBehavior)} publishing first");
        Publish(SubjectKeys.Test, new TestArg());

        StartCoroutine(DelayPublish());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
