using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SearchStrategy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract Vector2? FindTarget();
}
