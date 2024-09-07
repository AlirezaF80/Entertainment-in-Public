using System.Collections;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private float startingHealth;
    [SerializeField] private Animator anim;

    private bool isAlive = true;
    private bool isInvulnerable;
    private float invulnerabilityDuration = 2f;

    public float CurrentHealth { get; private set; }

    public event Action OnHealthChanged;

    private void Awake() {
        CurrentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage) {
        if (isInvulnerable) return;
        CurrentHealth -= damage;

        if (CurrentHealth > 0) {
            anim.SetTrigger("damage");
            StartCoroutine(InvulnerabilityTimer());
        } else if (isAlive) {
            isAlive = false;
            anim.SetTrigger("death");
            GetComponent<PlayerController>().enabled = false;
        }

        OnHealthChanged?.Invoke();
    }

    public void AddHealth(float value) {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, startingHealth);
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