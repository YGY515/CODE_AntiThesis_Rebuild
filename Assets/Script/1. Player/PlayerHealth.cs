using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int> HealthChange;
    public event Action HealthWarning;

    // 체력 3칸
    private int maxHealth = 3;
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;


    [Header("Effect")]
    public AudioSource audioSource;
    public AudioClip hitClip;
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float hitColorDuration = 1.0f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        
        if (damage <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);    // 두 값 중에 최댓값 반환

        
        if (audioSource != null && hitClip != null)
            audioSource.PlayOneShot(hitClip);

        // 스프라이트 빨간색 효과
        if (spriteRenderer != null)
            StartCoroutine(HitEffect());

        HealthChange?.Invoke(currentHealth);


        if (currentHealth <= maxHealth / 3)
        {
            // 마지막 체력 경고
            HealthWarning?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);    // 두 값 중에 최솟값 반환

        HealthChange?.Invoke(currentHealth);
    }

    private IEnumerator HitEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = hitColor;

        float elapsed = 0f;
        while (elapsed < hitColorDuration)
        {
            spriteRenderer.color = Color.Lerp(hitColor, originalColor, elapsed / hitColorDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }
}