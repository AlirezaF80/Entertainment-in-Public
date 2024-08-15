
using System.Collections;
using System;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab; // Reference to the obstacle prefab
    [SerializeField] private float spawnInterval = 3f; // Time between spawns (in seconds)
    [SerializeField] private float minAngle, maxAngle; // Range for random rotation angles

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // Generate random rotation factor for smoother interpolation
            float randomRotationFactor = UnityEngine.Random.Range(0f, 1f);

            // Calculate random angle using Lerp
            float randomAngle = Mathf.Lerp(minAngle, maxAngle, randomRotationFactor);

            // Create a quaternion with random rotation
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);

            // Create a new obstacle instance with random rotation
            GameObject obstacle = Instantiate(obstaclePrefab, transform.position, randomRotation);

            yield return new WaitForSeconds(spawnInterval); // Wait before spawning again
        }
    }
}

