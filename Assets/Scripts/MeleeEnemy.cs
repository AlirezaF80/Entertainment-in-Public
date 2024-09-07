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
    private Health playerHealth;


    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        coolDownTimer += Time.deltaTime;

        if (IsPlayerClose()) {
            if (coolDownTimer >= attackCoolDown) {
                coolDownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }
    }

    public void DamagePlayer() {
        if (IsPlayerClose() && playerHealth != null) playerHealth.TakeDamage(damageAmount);
    }

    private bool IsPlayerClose() {
        var bounds = boxCollider.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(
            bounds.center + transform.right * (range * transform.localScale.x * colliderDistance),
            new Vector3(bounds.size.x * range, bounds.size.y, bounds.size.z),
            0, Vector2.left, 0, playerlayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}