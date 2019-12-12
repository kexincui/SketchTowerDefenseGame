using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : FlyingShotScript
{
    public float Inertia;
    public float InitialAcceleration;

    //private float startTime;
    private EnemyManage EM;

    private Vector2 velocity;
    private float acceleration;
    private AudioSource mAudioSrc;

    // Start is called before the first frame update
    void Start()
    {
        acceleration = InitialAcceleration;
        EM = GameObject.Find("GameManagement").GetComponent<EnemyManage>();
        velocity = Direction.normalized * Speed;
        mAudioSrc = GetComponent<AudioSource>();
        mAudioSrc.Play();
        //startTime = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (Target == null || !Target.activeSelf)
        {
            Target = EM.GetClosestEnemyInRange(transform.position, float.PositiveInfinity, EnemyTags);
            if (Target == null)
            {

                BlowUp();
                return;
            }
        }

        var direction = Target.transform.position - transform.position;
        var angle = MathHelpers.Angle(direction, transform.up) * Time.deltaTime * Inertia;

        velocity = velocity.Rotate(angle);
        velocity += (Vector2)direction.normalized * acceleration * Time.deltaTime;

        transform.position += (Vector3)velocity * Time.deltaTime;
        transform.up = velocity;
    }
}
