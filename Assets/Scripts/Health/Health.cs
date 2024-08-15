using System.Collections;
using System;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private float startingHealth;
    public float currentHealth;
    [SerializeField] private Animator anim;

    private bool isAlive = true;
    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 2f;

    public event Action OnHealthChanged;

    private void Awake() {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage) {
        if (!isInvulnerable) {
            currentHealth = currentHealth - _damage;

            if (currentHealth > 0) {
                anim.SetTrigger("damage");
                StartCoroutine(InvulnerabilityTimer());
            } else if (isAlive) {
                isAlive = false;
                anim.SetTrigger("death");
                GetComponent<PlayerController>().enabled = false;
            }

            OnHealthChanged?.Invoke();
        }
    }

    public void AddHealth(float _value) {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        OnHealthChanged?.Invoke();
    }

    public void Respawn() {
        isAlive = true;
        AddHealth(startingHealth);
        anim.ResetTrigger("death");
        anim.Play("Idle");
        GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator InvulnerabilityTimer() {
        isInvulnerable = true; // Player becomes invulnerable

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false; // Invulnerability wears off
    }
}