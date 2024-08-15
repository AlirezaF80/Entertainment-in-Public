using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        jumpsLeft = maxJumps;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        grounded = IsGrounded();
        if (grounded)
            jumpsLeft = maxJumps;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpsLeft > 0)
                Jump();
            else if (!grounded)
                DoubleJump(); // Call DoubleJump when space is pressed and no regular jumps are left
        }

        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
            Attack();

        attackCooldownTimer -= Time.deltaTime;

        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("verticalSpeed", body.velocity.y);
    }

    private void Jump()
    {
        if (grounded && jumpsLeft > 0)
        {
            if (jumpSound != null)
            {
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            }

            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("jump");
            jumpsLeft--;
            Debug.LogError(jumpsLeft);
        }
    }

    private void DoubleJump()
    {
        if (jumpsLeft == 1) // Only allow double jump when there's one regular jump left
        {
            if (jumpSound != null)
            {
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            }

            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("jump");
            jumpsLeft--; // Decrement the regular jump count
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
        jumpsLeft = maxJumps;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
        if (raycastHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private void Attack()
    {
        if (SoundManager.instance == null)
        {
            Debug.LogError("SoundManager instance is null");
            return;
        }

        if (anim == null)
        {
            Debug.LogError("Animator is null");
            return;
        }
        if (attackSound != null)
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }

        // Proceed with attack logic
        attackCooldownTimer = attackCooldown;
        anim.SetTrigger("attack");
    }
}
