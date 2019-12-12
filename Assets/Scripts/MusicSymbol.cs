using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSymbol : FlyingShotScript
{

    private float distance;

    void Start()
    {
        distance = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var diff = Time.deltaTime * Speed;
        distance += diff;
        transform.position += (Vector3)Direction * diff;

        if (distance > Range)
        {
            Destroy(gameObject);
        }
    }
}
