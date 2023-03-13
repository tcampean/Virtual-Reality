using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtBird : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0f;
        while (time < 1)
        {
            Quaternion rotation = Quaternion.Slerp(transform.localRotation, lookRotation, time);
            transform.localRotation = rotation;
            time += Time.deltaTime * 1f;
        }

    }
}
