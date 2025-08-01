using TMPro;
using UnityEngine;
using System.Collections;

public class SupportAttackManager : MonoBehaviour
{
    public BossHealth bossHealth;
    public float waitTime;

    public AudioSource audioSource;
    public AudioClip beforeAttack;
    public AudioClip afterAttack;

    public GameObject attackDialoguePanel;
    public GameObject bossSheild;
    public EnemyController enemyController;

    public TMP_Text attackTextUI;
    public string[] attackLines = { "선배님! 지원 사격 들어갑니다!" };

    void OnEnable()
    {
        StartSupportAttack();

        if (BossPhaseManager.Instance.currentPhase == 1)
        {
            waitTime = 5f; // 1페이즈
        }
        else if (BossPhaseManager.Instance.currentPhase == 2)
        {
            waitTime = 7f; // 2페이즈
        }
        else if (BossPhaseManager.Instance.currentPhase == 3)
        {
            waitTime = 15f; // 3페이즈
        }
    }

    public void StartSupportAttack()
    {
        
        StartCoroutine(DelayedSupport());
    }

    IEnumerator DelayedSupport()
    {
        yield return new WaitForSeconds(waitTime);

        if (audioSource && beforeAttack)
            audioSource.PlayOneShot(beforeAttack);

        DialogueManager.Instance.ShowDialogue(attackLines, attackTextUI, () =>
        {
            StartCoroutine(ExecuteAttack());
        });

        attackDialoguePanel.SetActive(true);
    }

    IEnumerator ExecuteAttack()
    {
        yield return null;
        attackDialoguePanel.SetActive(false);
        gameObject.SetActive(false);

        if (audioSource && afterAttack)
            audioSource.PlayOneShot(afterAttack);

        if (bossHealth)
            bossHealth.BossTakeDamage(80);

        if (bossHealth.CurrentHealth > 0) Invoke("ActiveBossSheild", 1f);
        BossPhaseManager.Instance.AdvancePhase();
        enemyController.canBeDamaged = false;

    }

    void ActiveBossSheild()
    {
        bossSheild.SetActive(true);
    }
}