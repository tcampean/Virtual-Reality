using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public Transform ground;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 floatPoint = new Vector3(transform.position.x, ground.position.y + 2f, transform.position.z);
        transform.position = floatPoint;
      
    }

    void OnCollisionEnter(Collision collision)
    {
        audio.Play();
    }
}
