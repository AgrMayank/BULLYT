using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    public Transform[] spawnPositions;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        yield return new WaitForSeconds(1f);

        while (!ScaleController.isGameOver)
        {
            // Instantiate prefab1 or prefab2 randomly
            GameObject prefabToInstantiate = Random.Range(0f, 1f) < 0.5f ? prefab1 : prefab2;

            // Get a random spawn position without repetition
            Transform spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition != null)
            {
                // Instantiate the prefab at the selected position
                GameObject spawnedObject = Instantiate(prefabToInstantiate, spawnPosition.position, Quaternion.identity);

                // Destroy the instantiated object after 2 seconds
                Destroy(spawnedObject, 2f);
            }

            // Wait for 1 second before the next instantiation
            yield return new WaitForSeconds(1f);
        }
    }

    Transform GetRandomSpawnPosition()
    {
        // Shuffle the spawnPositions array to avoid consecutive spawns at the same position
        System.Random rand = new System.Random();
        int n = spawnPositions.Length;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            Transform value = spawnPositions[k];
            spawnPositions[k] = spawnPositions[n];
            spawnPositions[n] = value;
        }

        // Find the first available position (not occupied by an object)
        foreach (Transform position in spawnPositions)
        {
            Collider2D colliders = Physics2D.OverlapCircle(position.position, 0.2f);
            if (colliders == null)
            {
                // Return the first available position
                return position;
            }
        }

        // Return null if all positions are occupied
        return null;
    }
}
