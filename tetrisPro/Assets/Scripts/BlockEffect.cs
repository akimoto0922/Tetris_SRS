using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : MonoBehaviour
{
    Rigidbody rb;
    Vector3 dir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MakeDirection();
        Destroy(this.gameObject,5.0f);
        rb.AddForce(dir * 100.0f);
    }

    void MakeDirection()
    {
        dir.x = 1.0f;
        dir.y = 1.0f;
        dir.z = -1.0f;
    }
}
