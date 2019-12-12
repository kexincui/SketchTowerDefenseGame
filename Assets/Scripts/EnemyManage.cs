using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManage : MonoBehaviour
{

    private class EnemyDistancePair
    {
        public GameObject Enemy;
        public float Distance;

        public EnemyDistancePair(GameObject enemy, float distance)
        {
            Enemy = enemy;
            Distance = distance;
        }
    }

    private Dictionary<GameObject, EnemyDistancePair> enemies = new Dictionary<GameObject, EnemyDistancePair>();


    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy, new EnemyDistancePair(enemy, 0));
    }

    public void DeleteEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void UpdateEnemy(GameObject enemy, float distance)
    {
        enemies[enemy].Distance = distance;
    }

    public GameObject GetEnemyInRange(Vector2 position, float range, IEnumerable<string> enemyTags)
    {
        return enemies.Values
            .Where(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude < range * range && enemyTags.Any(t => e.Enemy.CompareTag(t)))
            .OrderBy(e => e.Distance)
            .Select(e => e.Enemy)
            .FirstOrDefault();
    }

    public GameObject GetClosestEnemyInRange(Vector2 position, float range, IEnumerable<string> enemyTags)
    {
        return enemies.Values
            .Where(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude < range * range && enemyTags.Any(t => e.Enemy.CompareTag(t)))
            .OrderBy(e => ((Vector2)e.Enemy.transform.position - position).sqrMagnitude)
            .Select(e => e.Enemy)
            .FirstOrDefault();
    }
}
