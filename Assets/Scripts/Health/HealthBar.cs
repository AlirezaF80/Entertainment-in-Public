using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image totalHealthBar;

    private void Start() {
        playerHealth.OnHealthChanged += UpdatePlayerHealthBar;
    }

    private void UpdatePlayerHealthBar() {
        totalHealthBar.fillAmount = playerHealth.CurrentHealth / 10;
    }
}