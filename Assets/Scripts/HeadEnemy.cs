using UnityEngine;

public class HeadEnemy : MonoBehaviour {
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Collider2D coll;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        rb.simulated = false;
    }

    private void Update() {
        rb.simulated = coll.IsInCameraBounds();
        if (rb.simulated) {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Check if collision is with the player
        if (collision.gameObject.GetComponent<PlayerHealth>()) {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}