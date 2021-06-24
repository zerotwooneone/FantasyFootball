using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeBehavior : MonoBehaviour
{
    public AoeRadiusBehavior RadiusBehavior;
    
    public Transform Transform;
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
        var scaleFactor = RadiusBehavior.GetRadius();
        var localScale = transform.localScale;
        Transform.localScale = new Vector3(scaleFactor * 3/2,
            scaleFactor * 2/3, localScale.z);
    }
}
