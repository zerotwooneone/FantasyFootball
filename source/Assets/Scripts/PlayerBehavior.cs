using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : HubParticipantBehavior
{
    protected override void Awake()
    {
        base.Awake();
        Debug.Log($"{nameof(PlayerBehavior)} awake");
        
        Debug.Log("publishing first");
        Publish(SubjectKeys.Test, new TestArg());

        StartCoroutine(DelayPublish());
    }

    private IEnumerator DelayPublish()
    {
        yield return new WaitForSeconds(2);
        
        Debug.Log("publishing after delay");
        Publish(SubjectKeys.Test, new TestArg());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
