using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehavior : HubParticipantBehavior
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    private void Start()
    {
        Subscribe<PlayerHealthChangedArgs>(SubjectKeys.PlayerHealthChanged, OnPlayerHealthChanged);
        Debug.Log($"{nameof(HealthBarBehavior)} subscribed to {SubjectKeys.PlayerHealthChanged}");
    }

    private void OnPlayerHealthChanged(PlayerHealthChangedArgs args)
    {
        if (args == null)
        {
            Debug.LogWarning("player health changed arg was null");
            return;
        }

        Debug.Log($"{nameof(OnPlayerHealthChanged)} delta:{args.Delta}");
        
        if (args.MAX.HasValue)
        {
            slider.maxValue = args.MAX.Value;
        }

        if (args.Current.HasValue)
        {
            slider.value = args.Current.Value;
        }

        if (args.Delta.HasValue)
        {
            Debug.Log($"old {slider.value} new:{(args.Current ?? slider.value) + args.Delta.Value}");
            slider.value = (args.Current ?? slider.value) + args.Delta.Value;
        }
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
