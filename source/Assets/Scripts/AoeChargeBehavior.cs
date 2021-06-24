using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeChargeBehavior : AoeRadiusBehavior
{
    public float GrowFactor = 1;
    public float MinSize = 1;
    public float MaxSize = 2;

    private float _size;
    private float _growSpeed;

    private void Awake()
    {
        if (GrowFactor < 0)
        {
            _size = MaxSize;
        }
        else
        {
            _size = MinSize;
        }

        _growSpeed = (MaxSize - MinSize) / 100;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var newSize = _size + (GrowFactor * _growSpeed);
        if (newSize > MaxSize)
        {
            newSize = MaxSize;
        }
        if(newSize < MinSize)
        {
            newSize = MinSize;
        }

        _size = newSize;
    }

    public override float GetRadius()
    {
        return _size;
    }
}