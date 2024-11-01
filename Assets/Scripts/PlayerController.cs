using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float coyoteTime;

    private Rigidbody2D body;
    private bool grounded;
    private BoxCollider2D boxCollider;
    public LayerMask groundLayer;
    private float attackCooldownTimer = 0f;
    private float lastGroundedTime;
    private Health health;
    private UIManager uiManager;

    private void Awake() {
        uiManager = FindObjectOfType<UIManager>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        health = GetComponent<Health>();
        health.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float currentHealth) {
        if (currentHealth > 0) //{
            anim.SetTrigger("damage");
        // } else {
            // anim.SetTrigger("death");
            // this.enabled = false;
        // }
    }

    private void Update() {
        if (uiManager.IsPaused()) return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        HandleHorizontalInput(horizontalInput);
        anim.SetBool("walk", horizontalInput != 0);

        grounded = IsGrounded();
        if (grounded)
            lastGroundedTime = Time.time;
        if (Input.GetButtonDown("Jump"))
            Jump();
        anim.SetBool("grounded", grounded);
        anim.SetFloat("verticalSpeed", body.velocity.y);

        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
            Attack();
        attackCooldownTimer -= Time.deltaTime;
    }

    private void HandleHorizontalInput(float horizontalInput) {
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Jump() {
        if (!grounded && !HasCoyoteTime()) return;
        if (jumpSound != null)
            SoundManager.instance.PlaySound(jumpSound);

        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    private bool HasCoyoteTime() {
        return Time.time - lastGroundedTime <= coyoteTime;
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0, Vector2.down, 0.02f, groundLayer);
        return raycastHit.collider != null;
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

    public void GameOver() {
        enabled = false;
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine() {
        while (!IsGrounded()) {
            yield return null;
        }

        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("death");
    }
}