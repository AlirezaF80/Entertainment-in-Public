using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [FormerlySerializedAs("playerHealth")] [SerializeField] private Health health;
    [SerializeField] private Image totalHealthBar;

    private void Start() {
        health.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth) {
        totalHealthBar.fillAmount = currentHealth / 10;
    }
}