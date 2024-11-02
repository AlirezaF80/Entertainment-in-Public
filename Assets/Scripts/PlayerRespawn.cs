using System;
using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {
    [SerializeField] private float delayBeforeRespawn = 2f;
    [SerializeField] private Transform[] checkPoints;

    private float maxReachedX;

    private PlayerController playerController;
    private Health health;

    private void Awake() {
        health = GetComponent<Health>();
        // uiManager = FindObjectOfType<UIManager>();
        playerController = GetComponent<PlayerController>();
        // Sort checkpoints by x ascending
        Array.Sort(checkPoints, (a, b) => a.position.x.CompareTo(b.position.x));
    }

    public void Respawn() {
        playerController.enabled = true;

        transform.position = GetLastCheckpoint().position;
        health.Respawn();
    }

    private void FixedUpdate() {
        maxReachedX = Mathf.Max(maxReachedX, playerController.transform.position.x);
    }

    private Transform GetLastCheckpoint() {
        Transform lastCheckPoint = checkPoints[0];
        foreach (var checkPoint in checkPoints) {
            if (maxReachedX > checkPoint.position.x) {
                lastCheckPoint = checkPoint;
            }
        }

        return lastCheckPoint;
    }

    public float GetPlayerProgress() {
        var levelFullDistance = checkPoints[^1].position.x - checkPoints[0].position.x;
        return maxReachedX / levelFullDistance;
    }

    private void Die() {
        playerController.enabled = false; // Disable player controller
        Invoke(nameof(Respawn), delayBeforeRespawn);
    }
}