using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : HubParticipantBehavior
{
    private const int DefaultMaxHp = 400;
    private const int DefaultHp = DefaultMaxHp;
    
    public int CurrentHp = DefaultHp;
    public int MaxHp = DefaultMaxHp;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{nameof(PlayerBehavior)} start");
        
        Debug.Log($"{nameof(PlayerBehavior)} publishing first");
        Publish(SubjectKeys.PlayerHealthChanged, PlayerHealthChangedArgs.Factory(current: CurrentHp,max: MaxHp));
        
        Publish(SubjectKeys.Test, new TestArg());

        StartCoroutine(DelayPublish());
    }
    
    private IEnumerator DelayPublish()
    {
        yield return new WaitForSeconds(2);
        
        Debug.Log($"{nameof(PlayerBehavior)} publishing after delay");
        Publish(SubjectKeys.Test, new TestArg());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
