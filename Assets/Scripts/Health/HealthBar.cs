using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthbBar;

    private void Start()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;  
    }

    private void Update()
    {
        currentHealthbBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
