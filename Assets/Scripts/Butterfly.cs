using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : FlyingShotScript
{
    public float MaxTime;
    public float Inertia;

    private EnemyManage EM;
    private Vector2 velocity;
    private float time;
    private AudioSource mAudioSrc;
    
    // Start is called before the first frame update
    void Start()
    {
        EM = GameObject.Find("GameManagement").GetComponent<EnemyManage>();
        time = MaxTime;
        velocity = Direction.normalized * Speed;
        mAudioSrc = GetComponent<AudioSource>();
        mAudioSrc.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            BlowUp();
        }

        if (Target == null || !Target.activeSelf)
        {
            Target = EM.GetClosestEnemyInRange(transform.position, Range * 2, EnemyTags);
            if (Target == null)
            {
                BlowUp();
                return;
            }
        }
        
        var direction = Target.transform.position - transform.position;
        var angle = MathHelpers.Angle(direction, transform.up) * Time.deltaTime * Inertia;

        velocity = velocity.Rotate(angle);
        transform.position += (Vector3)velocity * Time.deltaTime;
        transform.up = velocity;

        
    }
}
