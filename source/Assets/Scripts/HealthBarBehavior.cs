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
    
    private void Awake()
    {
        Subscribe<PlayerHealthChangedArgs>(SubjectKeys.PlayerHealthChanged, OnPlayerHealthChanged);
    }

    private void OnPlayerHealthChanged(PlayerHealthChangedArgs args)
    {
        if (args == null)
        {
            Debug.LogWarning("player health changed arge was null");
            return;
        }

        if (args.MAX.HasValue)
        {
            slider.maxValue = args.MAX.Value;
        }

        if (args.Current.HasValue)
        {
            slider.value = args.Current.Value;
        }
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
