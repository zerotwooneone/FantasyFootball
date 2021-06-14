using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform FirePoint;
    public Transform Target;

    public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var prefab = Instantiate(Prefab, FirePoint.position, FirePoint.rotation);
        var projectileBehavior = prefab.GetComponent<ProjectileBehavior>();
        projectileBehavior.Target = Target;
    }
}
