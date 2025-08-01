using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    public AudioSource Boss_audioSource;
    public AudioClip Boss_hitClip;
    public SpriteRenderer Boss_spriteRenderer;
    public Color Boss_hitColor = Color.red;
    public float Boss_hitColorDuration = 1.0f;

    public event Action<int> BossHealthChange;
    public event Action BossHealthPhase;

    private int maxHealth = 500;
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    private Coroutine hitCoroutine;

    public void BossTakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (Boss_audioSource != null && Boss_hitClip != null)
            Boss_audioSource.PlayOneShot(Boss_hitClip);


        // 스프라이트 깜빡이기 중복 방지
        if (Boss_spriteRenderer != null)
        {
            if (hitCoroutine != null)
                StopCoroutine(hitCoroutine);

            hitCoroutine = StartCoroutine(BossHitEffect());
        }

        BossHealthChange?.Invoke(currentHealth);

        if (currentHealth <= maxHealth / 3)
        {
            BossHealthPhase?.Invoke();
        }

    }

    private IEnumerator BossHitEffect()
    {
        Color originalColor = Boss_spriteRenderer.color;
        Boss_spriteRenderer.color = Boss_hitColor;

        float elapsed = 0f;
        while (elapsed < Boss_hitColorDuration)
        {
            Boss_spriteRenderer.color = Color.Lerp(Boss_hitColor, originalColor, elapsed / Boss_hitColorDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Boss_spriteRenderer.color = originalColor;
    }

}