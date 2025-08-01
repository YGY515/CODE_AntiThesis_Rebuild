using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private BossHealth BossStatus;
    [SerializeField] private Slider healthBar;

    private void OnEnable()
    {
        if (BossStatus == null)
        {
            BossStatus = FindObjectOfType<BossHealth>();
        }
        BossStatus.BossHealthChange += UpdateHealth;

        healthBar.maxValue = BossStatus.MaxHealth;
    }

    void OnDisable()
    {
        BossStatus.BossHealthChange -= UpdateHealth;
    }

    void Start()
    {
        healthBar.value = BossStatus.CurrentHealth;

    }

    void UpdateHealth(int currentHealth)
    {
        healthBar.value = currentHealth;
        Debug.Log($"HP UI updated: {currentHealth}");
    }
}
