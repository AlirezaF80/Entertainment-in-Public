using UnityEngine;

public class Guillotine : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private float minAngle, maxAngle;
    [SerializeField] private Transform spawnPoint;

    private bool isDismantled;

    public void SpawnObstacle() {
        if (isDismantled) return;
        var randomRotationFactor = Random.Range(0f, 1f);
        var randomAngle = Mathf.Lerp(minAngle, maxAngle, randomRotationFactor);
        var randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        var prefabToSpawn = Random.Range(0, obstaclePrefabs.Length);
        var obstacle = Instantiate(obstaclePrefabs[prefabToSpawn], spawnPoint);
        obstacle.transform.rotation = randomRotation;
    }

    public void Dismantle() {
        isDismantled = true;
        animator.SetTrigger("Dismantle");
    }
}