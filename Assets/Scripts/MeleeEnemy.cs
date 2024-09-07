using UnityEngine;

public class MeleeEnemy : MonoBehaviour {
    [SerializeField] private float attackCoolDown;
    [SerializeField] private int damageAmount;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float range;
    [SerializeField] float colliderDistance;
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

        switch (currentState) {
            case State.Idle:
                anim.SetBool(MovingAnimatorHash, false);
                if (boxCollider.IsInCameraBounds()) {
                    anim.SetBool(MovingAnimatorHash, true);
                    currentState = State.Chase;
                }

                break;

            case State.Chase:
                if (CanAttackPlayer()) {
                    currentState = State.Attack;
                } else {
                    ChasePlayer();
                    anim.SetBool(MovingAnimatorHash, true);
                }

                break;

            case State.Attack:
                if (CanAttackPlayer()) {
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
        RaycastHit2D hit = Physics2D.BoxCast(
            bounds.center + transform.right * (range * transform.localScale.x),
            new Vector3(bounds.size.x * range, bounds.size.y, bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
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
        // show the range of the enemy with a line
        Gizmos.DrawLine(
            bounds.center + transform.right * (range * transform.localScale.x * colliderDistance),
            bounds.center + transform.right * (range * transform.localScale.x * colliderDistance) +
            Vector3.left * bounds.size.x * range);
    }
}