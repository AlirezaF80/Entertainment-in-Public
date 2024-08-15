using UnityEngine;

public class EnemySide : MonoBehaviour {
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isSeen = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void Update() {
        if (isSeen || IsInCameraBounds()) {
            isSeen = true;
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    private bool IsInCameraBounds() {
        // check the bounds of the camera to see if the enemy's collider is in view, taking into account the collider's size
        Vector3 leftColliderPoint =
            new Vector3(coll.bounds.min.x, coll.bounds.center.y, coll.bounds.center.z);
        Vector3 rightColliderPoint =
            new Vector3(coll.bounds.max.x, coll.bounds.center.y, coll.bounds.center.z);
        Vector3 leftViewportPoint = Camera.main.WorldToViewportPoint(leftColliderPoint);
        Vector3 rightViewportPoint = Camera.main.WorldToViewportPoint(rightColliderPoint);
        return leftViewportPoint.x > 0 && rightViewportPoint.x < 1 &&
               leftViewportPoint.y > 0 && rightViewportPoint.y < 1;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Check if collision is with the player
        if (collision.gameObject.GetComponent<Health>()) {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}