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
    // Start is called before the first frame update
    void Start()
    {
        if (GrowFactor < 0)
        {
            _size = MinSize;
        }
        else
        {
            _size = MaxSize;
        }

        _growSpeed = (MaxSize - MinSize) / 100;
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
            newSize = MinSize;
        }
        if(newSize < MinSize)
        {
            newSize = MaxSize;
        }

        _size = newSize;
    }

    public override float GetRadius()
    {
        return _size;
    }
}