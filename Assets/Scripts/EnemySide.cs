using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySide : MonoBehaviour
{
    [SerializeField] private float damage;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
