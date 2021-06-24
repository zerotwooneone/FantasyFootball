using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AoeFactory : MonoBehaviour
{
    public GameObject Prefab;
    private GameObject _instance;

    public Transform SourceLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        const int rightMouseButton = 1;
        if (Input.GetMouseButton(rightMouseButton))
        {
            if (_instance == null)
            {
                _instance = Instantiate(Prefab, SourceLocation.position, SourceLocation.rotation);
            }
        }
        else
        {
            if (_instance != null)
            {
                Destroy(_instance);
                _instance = null;
            }
        }
    }
}
