using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public Vector3 pointA, pointB;
    public float speed = 1.0f;

    void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}
