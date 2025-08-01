using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmFadeOut : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    public BossHealth bossHealth; 

    void Update()
    {
        if (bossHealth != null && bossHealth.CurrentHealth <= 0)
        {
            StartCoroutine(FadeOutBGM());
        }
    }

    private IEnumerator FadeOutBGM()
    {

        float startVolume = bgmAudioSource.volume;
        float fadeDuration = 2.0f; 
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bgmAudioSource.volume = 0f; // º¸½º¸÷ »ç¸Á ½Ã ÃÖÁ¾ÀûÀ¸·Î º¼·ý 0À¸·Î
    }
}
