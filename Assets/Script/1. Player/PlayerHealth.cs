using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip hitClip;
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float hitColorDuration = 1.0f;

    public event Action<int> HealthChange;
    public event Action HealthWarning;

    private int maxHealth = 3;
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    private Coroutine hitCoroutine;

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
        hitCoroutine = null;
    }

    public void PlayerTakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (audioSource != null && hitClip != null)
            audioSource.PlayOneShot(hitClip);

        // 맞은 이펙트의 코루틴 중복 방지
        if (spriteRenderer != null)
        {
            if (hitCoroutine != null)
                StopCoroutine(hitCoroutine);

            hitCoroutine = StartCoroutine(HitEffect());
        }

        HealthChange?.Invoke(currentHealth);
        //Debug.Log($"플레이어 받은 데미지: {damage}, 현재 체력: {currentHealth}");

        if (currentHealth <= maxHealth / 3)
        {
            HealthWarning?.Invoke();
        }
    }

    public void PlayerHeal(int amount)
    {
        if (amount <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        HealthChange?.Invoke(currentHealth);
    }

}