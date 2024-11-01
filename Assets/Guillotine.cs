using System.Collections;
using UnityEngine;

public class Guillotine : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private float minAngle, maxAngle;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float playerDetectionRadius;

    [SerializeField] private Transform[] hoveringParts;
    [SerializeField] private float hoverRotationRange;
    [SerializeField] private float hoverFullRotationDuration;
    [SerializeField] private float hoverRotationOffset;

    private PlayerController player;

    private bool isDismantled;

    private void Awake() {
        player = FindObjectOfType<PlayerController>();
    }

    public void SpawnObstacle() {
        if (isDismantled) return;
        if (!IsPlayerNearby()) return;
        var randomRotationFactor = Random.Range(0f, 1f);
        var randomAngle = Mathf.Lerp(minAngle, maxAngle, randomRotationFactor);
        var randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        var prefabToSpawn = Random.Range(0, obstaclePrefabs.Length);
        var obstacle = Instantiate(obstaclePrefabs[prefabToSpawn], spawnPoint);
        obstacle.transform.rotation = randomRotation;
    }

    private bool IsPlayerNearby() {
        return Mathf.Abs(player.transform.position.x - transform.position.x) < playerDetectionRadius;
    }

    public void Dismantle() {
        isDismantled = true;
        animator.SetTrigger("Dismantle");
        StartCoroutine(AnimatePartsHovering());
    }

    private IEnumerator AnimatePartsHovering() {
        // They should rotate from hoverRotationRange.x to hoverRotationRange.y, and back using a sine wave
        var timeElapsed = 0f;
        while (true) {
            for (var i = 0; i < hoveringParts.Length; i++) {
                var part = hoveringParts[i];
                part.rotation = Quaternion.Euler(0, 0,
                    Mathf.Sin(timeElapsed * 2 * Mathf.PI / hoverFullRotationDuration + hoverRotationOffset * i) * hoverRotationRange);
                yield return null;
                timeElapsed += Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmos() {
        // Show detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}