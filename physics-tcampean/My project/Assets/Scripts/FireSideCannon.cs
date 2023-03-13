using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireSideCannon : MonoBehaviour
{
    [SerializeField] public Rigidbody projectile;
    public Transform target;
    private long currentTime;
    private long startTime;
    // Start is called before the first frame update
    private void Start()
    {
        startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 1000;
        currentTime = startTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentTime >= startTime)
        {
            FireAtTarget();
            startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 1000;
            currentTime = startTime - 1000;
        }
        else
        {
            currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }



    private void FireAtTarget()
    {
        var velocity = (target.position - transform.position) * 7;
        projectile.transform.position = transform.position;
        projectile.velocity = velocity;
    }
}
