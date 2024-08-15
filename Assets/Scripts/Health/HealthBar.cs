using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;

    private void Start() {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar() {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}