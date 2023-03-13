using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
        float time = 0f;
        while (time < 1)
        {
            Quaternion rotation = Quaternion.Slerp(transform.localRotation, lookRotation, time);
            target.localRotation = rotation;
            if ((target.eulerAngles.y <= 90 || target.eulerAngles.y >= 280))
            {
                transform.localRotation = rotation;
            }
            time += Time.deltaTime * 1f;
        }
    }
}
