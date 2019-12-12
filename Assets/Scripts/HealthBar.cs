using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        bar.localScale = new Vector3(1f, 1f);
    }

    public void setSize(float sizeNormalized)
    {
        if (bar != null) { bar.localScale = new Vector3(sizeNormalized, 1f); }
    }
}
