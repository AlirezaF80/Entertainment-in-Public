using UnityEngine;

[RequireComponent(typeof(Health), typeof(Collider2D))]
public class WeakSpot : MonoBehaviour {
    [SerializeField] private float damageAmount;

    private Health health;

    private void Awake() {
        health = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.GetComponent<PlayerController>()) return;

        // if the player is coming from above, take damage
        if (other.transform.position.y > transform.position.y) {
            health.TakeDamage(damageAmount);
        }
    }
}