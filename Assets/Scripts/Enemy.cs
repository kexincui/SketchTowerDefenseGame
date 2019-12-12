using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth;
    private Transform healthBarObj;
    private HealthBar healthBar;
    private GameManagement GM;
    private EnemyManage EM;

    private float health;
    private bool isAvailable;
    
    //private readonly float EPSILON = 1e-4f;

    // TODO: health and show health bar
    // TODO: collision with the attack and reduce the health, collision to the endpoint.

    // Start is called before the first frame update
    void OnEnable()
    {
        isAvailable = true;

        health = MaxHealth;
        healthBarObj = gameObject.transform.Find("HealthBar");
        healthBar = healthBarObj.GetComponent<HealthBar>();
        healthBarObj.gameObject.SetActive(false);

        GM = GameObject.Find("GameManagement").GetComponent<GameManagement>();
        EM = GameObject.Find("GameManagement").GetComponent<EnemyManage>();
    }

    //void Update()
    //{

    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!gameObject.activeSelf || !isAvailable) return;
        if (collision.CompareTag("Butterfly"))
        {
            var flyingShot = collision.gameObject.GetComponentInParent<FlyingShotScript>();
            var damage = flyingShot.Damage;
            health -= damage * Time.deltaTime;
            healthBarObj.gameObject.SetActive(true);
            healthBar.setSize(health / MaxHealth);
            if (health <= 0)
            {
                isAvailable = false;
                GM.EnemyKilled();

                EM.DeleteEnemy(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf || !isAvailable) return;
        if (collision.CompareTag("Finish"))
        {
            isAvailable = false;
            GM.EnemyEscaped();
        }
        else if (collision.CompareTag("Bullet") || (collision.CompareTag("Bee") && CompareTag("Ballon1")))
        {
            var flyingShot = collision.gameObject.GetComponentInParent<FlyingShotScript>();
            var damage = flyingShot.Damage;
            health -= damage;
            healthBarObj.gameObject.SetActive(true);
            healthBar.setSize(health / MaxHealth);
            //if (!collision.CompareTag("Butterfly"))
            flyingShot.BlowUp();

            if (health <= 0)
            {
                isAvailable = false;
                GM.EnemyKilled();
     
                EM.DeleteEnemy(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            EM.DeleteEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}