using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TMP_Text gameTime;
    public float duration;
    private float remainingTime;
    private bool isRunning = false;
    public GameObject bossShield;
    public GameObject SupportAttackManager; 

    public AudioSource audioSource;
    public AudioClip hitClip;

    public string[] startAttack = { "³×ÀÌÁ© ¾¾! Áö±ÝÀÌ¿¡¿ä!" };
    public TMP_Text timerTextUI;
    public GameObject timerDialoguePanel;


    void OnEnable()
    {
        if (BossPhaseManager.Instance.currentPhase == 1)
        {
            duration = 10f; 
        }
        else if (BossPhaseManager.Instance.currentPhase == 2)
        {
            duration = 15f; 
        }
        else if (BossPhaseManager.Instance.currentPhase == 3)
        {
            duration = 25f;
        }
        StartTimer(duration);
    }

    void Update()
    {
        
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            isRunning = false;
            gameTime.text = "";
            bossShield.SetActive(false);

            audioSource.PlayOneShot(hitClip);
            StartCoroutine(ShowAttackDialogueRoutine());
        }
        else
        {
            int min = Mathf.FloorToInt(remainingTime / 60f);
            int sec = Mathf.FloorToInt(remainingTime % 60f);
            gameTime.text = string.Format("{0:D2}:{1:D2}", min, sec);
        }

    }

    void StartTimer(float seconds)
    {
        duration = seconds;
        remainingTime = duration;
        isRunning = true;
        gameObject.SetActive(true);
    }

    IEnumerator ShowAttackDialogueRoutine()
    {
        yield return new WaitForSeconds(0.1f); 
        timerDialoguePanel.SetActive(true);

        DialogueManager.Instance.ShowDialogue(startAttack, timerTextUI, () =>
        {
            
        });

        yield return new WaitForSeconds(2f);
        timerDialoguePanel.SetActive(false);
        gameObject.SetActive(false);
        SupportAttackManager.SetActive(true);
    }
}
