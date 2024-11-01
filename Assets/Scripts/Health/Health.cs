using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {
    [SerializeField] private float startingHealth;
    [SerializeField] private Animator anim;
    [SerializeField] private UnityEvent onDeath;

    private bool isAlive = true;
    private bool isInvulnerable;
    private float invulnerabilityDuration = 2f;

    public float CurrentHealth { get; private set; }

    public event Action<float> OnHealthChanged;

    private void Awake() {
        CurrentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage) {
        if (isInvulnerable) return;
        CurrentHealth -= damage;

        if (CurrentHealth > 0) {
            StartCoroutine(InvulnerabilityTimer());
        } else if (isAlive) {
            isAlive = false;
            onDeath?.Invoke();
        }

        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void AddHealth(float value) {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, startingHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void Respawn() {
        isAlive = true;
        CurrentHealth = startingHealth;
    }

    IEnumerator InvulnerabilityTimer() {
        isInvulnerable = true; // Player becomes invulnerable

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false; // Invulnerability wears off
    }
}