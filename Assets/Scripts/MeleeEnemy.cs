using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    [SerializeField] private int damage;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] LayerMask playerlayer;
    [SerializeField] float range;
    [SerializeField] float colliderDistance;
    private float coolDownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;

        if (playerInSight())
        {
            if (coolDownTimer >= attackCoolDown)
            {
                coolDownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }


    }


    private bool playerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range,boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,Vector2.left, 0, playerlayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (playerInSight())
            playerHealth.TakeDamage(damage);
    }
}
