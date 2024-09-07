using UnityEngine;

public class MeleeEnemy : MonoBehaviour {
    [SerializeField] private float attackCoolDown;
    [SerializeField] private int damageAmount;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float range;
    [SerializeField] float moveSpeed;

    private enum State {
        Idle,
        Chase,
        Attack
    }

    private State currentState = State.Idle;

    private static readonly int MovingAnimatorHash = Animator.StringToHash("moving");

    private float coolDownTimer = Mathf.Infinity;
    private Vector3 initialScale;
    private Animator anim;
    private Health playerHealth;
    private Transform player;

    private void Awake() {
        anim = GetComponent<Animator>();
        initialScale = transform.localScale;
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {
        FacePlayer();
        coolDownTimer += Time.deltaTime;

        var canAttackPlayer = CanAttackPlayer();
        anim.SetBool(MovingAnimatorHash, !canAttackPlayer);

        switch (currentState) {
            case State.Idle:
                anim.SetBool(MovingAnimatorHash, false);
                if (boxCollider.IsInCameraBounds()) {
                    anim.SetBool(MovingAnimatorHash, true);
                    currentState = State.Chase;
                }

                break;

            case State.Chase:
                if (canAttackPlayer) currentState = State.Attack;
                else ChasePlayer();
                break;

            case State.Attack:
                if (canAttackPlayer) {
                    var isAttackCooldownOver = coolDownTimer >= attackCoolDown;
                    if (isAttackCooldownOver) {
                        coolDownTimer = 0;
                        anim.SetTrigger("meleeAttack");
                    }
                } else {
                    currentState = State.Chase;
                }

                break;
        }
    }

    private void FacePlayer() {
        if (player == null) return;
        var direction = Mathf.Sign(player.position.x - transform.position.x);
        transform.localScale = initialScale.With(x: direction * initialScale.x);
    }

    // This method is called via Animation Event
    public void DamagePlayer() {
        if (CanAttackPlayer() && playerHealth != null) playerHealth.TakeDamage(damageAmount);
    }

    private bool CanAttackPlayer() {
        var bounds = boxCollider.bounds;
        RaycastHit2D hit = Physics2D.Raycast(
            bounds.center,
            (Vector2.right * transform.localScale.x).normalized,
            (bounds.size.x / 2 + range) * Mathf.Abs(transform.localScale.x),
            playerLayer);

        var hasSeenPlayer = hit.collider != null && hit.transform.CompareTag("Player");
        if (hasSeenPlayer)
            playerHealth = hit.transform.GetComponent<Health>();
        return hasSeenPlayer;
    }

    private void ChasePlayer() {
        if (player != null) {
            transform.position =
                Vector2.MoveTowards(transform.position, player.position.With(y: transform.position.y),
                    moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        var bounds = boxCollider.bounds;
        Gizmos.DrawRay(bounds.center, Vector2.right * transform.localScale.x * (bounds.size.x / 2 + range));
    }
}