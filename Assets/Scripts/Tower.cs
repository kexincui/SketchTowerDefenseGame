using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tower : MonoBehaviour
{

    public GameObject BulletPrototype;
    public float ShootingPeriod;
    public float Range;
    public float BulletSpeed;
    public float Damage;
    public float RotationSpeed;
    public List<GameObject> Enemies;

    private EnemyManage EM;

    private float timeToShoot = 0.0f;
    private List<string> enemyTags;

    // Start is called before the first frame update
    void Start()
    {
        EM = GameObject.Find("GameManagement").GetComponent<EnemyManage>();

        enemyTags = Enemies.Select(e => e.tag).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        var enemy = EM.GetEnemyInRange(transform.position, Range, enemyTags);

        if (enemy != null)
        {
            // TurnToEnemy(enemy.transform.position + enemy.transform.right * 32);

            if (timeToShoot < 0)
            {
                GameObject bullet = Instantiate(BulletPrototype, transform);
                //bullet.transform.position = transform.position;
                //bullet.transform.rotation = transform.rotation;

                var bulletScript = bullet.GetComponent<FlyingShotScript>();
                bulletScript.Speed = BulletSpeed;
                bulletScript.Range = Range;
                bulletScript.Direction = EnemyDirection(enemy.transform.position);
                bulletScript.Damage = Damage;
                bulletScript.Target = enemy;
                bulletScript.EnemyTags = enemyTags;
                bulletScript.Turret = transform;

                //bullet.SetActive(true);

                timeToShoot = ShootingPeriod;
                return;
            }
        }
        else
        {
            var closestEnemy = EM.GetClosestEnemyInRange(transform.position, Range * 2, enemyTags);
            //if (closestEnemy != null) TurnToEnemy(closestEnemy.transform.position + closestEnemy.transform.right * 32);
        }

        timeToShoot -= Time.deltaTime;
    }

    private Vector3 EnemyDirection(Vector2 position)
    {
        var direction = (position - (Vector2)transform.position);
        direction.Normalize();
        return direction;
    }
}
