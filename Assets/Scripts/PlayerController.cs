using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private bool grounded;
    private int jumpsLeft;
    private BoxCollider2D boxCollider;
    public LayerMask groundLayer;
    private float attackCooldownTimer = 0f;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        jumpsLeft = maxJumps;
    }

    private void Update() {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        grounded = IsGrounded();
        Debug.Log(grounded);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
            Attack();

        attackCooldownTimer -= Time.deltaTime;

        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("verticalSpeed", body.velocity.y);
    }

    private void Jump() {
        if (!grounded) return;
        if (jumpSound != null)
            SoundManager.instance.PlaySound(jumpSound);

        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0, Vector2.down, 0.02f, groundLayer);
        return raycastHit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawCube(boxCollider.bounds.size, boxCollider.bounds.size);
    }

    private void Attack() {
        if (anim == null) {
            Debug.LogError("Animator is null");
            return;
        }

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound(attackSound);

        attackCooldownTimer = attackCooldown;
        anim.SetTrigger("attack");
    }
}