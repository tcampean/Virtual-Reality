using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveForearm : MonoBehaviour
{
    private float speed = 6.0f;
    private const float minBound = 0.0f;
    private const float maxBound = 8.0f;

    private float currentValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var x1 = currentValue + speed * Time.deltaTime;
        if (x1 > maxBound)
            speed = -1 * Math.Abs(speed);
        else if (x1 < minBound)
            speed = Math.Abs(speed);
        currentValue += speed * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(currentValue, Vector3.left);
    }
}