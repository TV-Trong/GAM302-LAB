using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public List<Transform> spawnerAreas = new List<Transform>();
    float timeBetweenSpawn => Random.Range(10, 20);
    float spawnTimer = 0;

    public override void Spawned()
    {
        spawnTimer = timeBetweenSpawn;
    }

    public override void FixedUpdateNetwork()
    {
        spawnTimer -= Runner.DeltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = timeBetweenSpawn;
        }
    }

    private void SpawnEnemy()
    {
        var enemies = FindObjectsByType<EnemyBehaviour>(FindObjectsSortMode.None);

        if (enemies.Length > 10)
        {
            Debug.Log("Enemy full!");
            return;
        }
        else
        {
            bool spawnSuccess = false;

            while (!spawnSuccess)
            {
                var area = spawnerAreas[Random.Range(0, spawnerAreas.Count)];

                if (Physics2D.OverlapCircle(area.position, 3f, LayerMask.GetMask("Player")) == null)
                {
                    float spawnRadius = Random.Range(-3, 3);
                    var spawnPosition = new Vector2(area.position.x + spawnRadius, area.position.y + spawnRadius);
                    var enemy = Runner.Spawn(enemyPrefab, spawnPosition, Quaternion.identity);
                    spawnSuccess = true;
                }
            }
        }
    }
}
