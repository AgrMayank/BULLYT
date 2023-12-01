using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPointHolder;
    public List<Transform> spawnPoints = new(); // Array of spawn points
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform enemiesParent; // Parent object for the spawned enemies
    public float spawnRadius = 10f; // Radius around spawn points
    public int maxEnemies = 5; // Maximum number of enemies to be active
    public float maxDistanceFromPlayer = 50f; // Maximum distance for an enemy from the player

    private Transform player; // Reference to the player
    private int currentEnemyCount = 0; // Current number of active enemies

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("SpawnEnemy", 0f, 2f); // Adjust the delay as needed

        foreach (Transform tr in spawnPointHolder)
        {
            spawnPoints.Add(tr);
        }
    }

    private void SpawnEnemy()
    {
        // if (HealthController.Instance.m_playerLost)
        //     return;

        currentEnemyCount = enemiesParent.childCount;

        if (currentEnemyCount < maxEnemies)
        {
            // Find the closest spawn point to the player
            Transform closestSpawnPoint = null;
            float closestDistance = float.MaxValue;

            foreach (Transform spawnPoint in spawnPoints)
            {
                float distanceToPlayer = Vector3.Distance(spawnPoint.position, player.position);

                // Check if any enemy is already near this spawn point
                bool isNearOtherEnemy = false;

                foreach (Transform enemyTransform in enemiesParent)
                {
                    float distanceToEnemy = Vector3.Distance(enemyTransform.position, spawnPoint.position);

                    if (distanceToEnemy < spawnRadius)
                    {
                        isNearOtherEnemy = true;
                        break;
                    }
                }

                if (distanceToPlayer < closestDistance && !isNearOtherEnemy)
                {
                    closestSpawnPoint = spawnPoint;
                    closestDistance = distanceToPlayer;
                }
            }

            if (closestSpawnPoint != null)
            {
                // Spawn the enemy at the closest spawn point
                Vector3 spawnPosition = closestSpawnPoint.position + Random.insideUnitSphere * spawnRadius;
                // spawnPosition.y = 0f; // Ensure enemies spawn at the same height as the player

                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                newEnemy.transform.SetParent(enemiesParent);
                currentEnemyCount++;
            }
        }
    }

    private void Update()
    {
        // Check the distance of each enemy from the player
        foreach (Transform enemyTransform in enemiesParent)
        {
            float distanceToPlayer = Vector3.Distance(enemyTransform.position, player.position);

            // If an enemy is too far from the player, destroy it and spawn a new one
            if (distanceToPlayer > maxDistanceFromPlayer)
            {
                Destroy(enemyTransform.gameObject);
                currentEnemyCount--;
                SpawnEnemy();
            }
        }
    }
}
