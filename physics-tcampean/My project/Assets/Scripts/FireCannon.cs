using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireCannon : MonoBehaviour
{
    public Rigidbody projectile;
    public Transform target;
    // Start is called before the first frame update
    private void Start()
    {
    
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("f"))
            FireAtTarget();
    }
          
      

    private void FireAtTarget()
    {
        var velocity = (target.position - transform.position) * 3;
        projectile.transform.position = transform.position;
        projectile.velocity = velocity;
    }
}
