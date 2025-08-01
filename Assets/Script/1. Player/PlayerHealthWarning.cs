using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthWarning : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject warning;

    private Coroutine blinkCoroutine;

    void OnEnable()
    {
        playerHealth.HealthWarning += ShowWarning;
        playerHealth.HealthChange += OnHealthChanged;
    }

    void OnDisable()
    {
        playerHealth.HealthWarning -= ShowWarning;
        playerHealth.HealthChange -= OnHealthChanged;
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
    }

    void ShowWarning()
    {
        warning.SetActive(true);
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkWarning());
    }

    void OnHealthChanged(int currentHealth)
    {
        if (currentHealth > playerHealth.MaxHealth / 3)
        {
            // 체력이 회복되면 경고창 비활성화
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            warning.SetActive(false);
        }
    }

    private IEnumerator BlinkWarning()
    {
        Image panelImage = warning.GetComponent<Image>();
        if (panelImage == null)
            yield break;

        float blinkSpeed = 1.5f;
        float minAlpha = 0.0f;
        float maxAlpha = 1.0f;

        while (true)
        {
            float t = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);
            Color c = panelImage.color;
            c.a = alpha;
            panelImage.color = c;
            yield return null;
        }
    }
}