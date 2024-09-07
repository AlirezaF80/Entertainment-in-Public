using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MeleeEnemy : MonoBehaviour {
    [SerializeField] private float attackCoolDown;
    [FormerlySerializedAs("damage")] [SerializeField] private int damageAmount;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] LayerMask playerlayer;
    [SerializeField] float range;
    [SerializeField] float colliderDistance;
    private float coolDownTimer = Mathf.Infinity;

    private Animator anim;
    private PlayerHealth playerHealth;


    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        coolDownTimer += Time.deltaTime;

        if (IsPlayerInSight()) {
            if (coolDownTimer >= attackCoolDown) {
                coolDownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }
    }

    public void DamagePlayer() {
        if (playerHealth != null) playerHealth.TakeDamage(damageAmount);
    }

    private bool IsPlayerInSight() {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerlayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}