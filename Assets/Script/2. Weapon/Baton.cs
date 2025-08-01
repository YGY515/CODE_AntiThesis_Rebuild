using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baton : MonoBehaviour, IWeapon
{
    public int Damage => 0; 
    public Animator playerAnimator;
    public Transform batonTransform;
    public Collider2D Collider2D;

    public AudioSource audioSource;
    public AudioClip swingClip;
    public BossHealth bossHealth;
    public EnemyController enemy;
    public GameObject sheildObject;
    public GameObject playerDialogue;
    public GameObject boss;


    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Boss")) && (!sheildObject.activeSelf))
        {

            bool isCritical = Random.value < 0.3f; 
            int Damage = isCritical ? 25 : Random.Range(15, 24); // 치명타면 30 데미지, 아니면 일반

            Debug.Log("보스가 맞는중");
            bossHealth.BossTakeDamage(Damage);

            DamageTextManager.Instance.ShowDamage(
                boss.transform.position, Damage, isCritical

            );
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyController target = other.GetComponent<EnemyController>();
            if (target.canBeDamaged == true)
            {
                target.TakeDamage(1);
            }
            else
            {
                playerDialogue.SetActive(true);
            }
        }

    }


    public void Attack()
    {

        if (playerAnimator == null)
        {
            Debug.LogError("playerAnimator가 할당되지 않음");
            return;
        }


        float looking = playerAnimator.GetFloat("Looking");

        int dir = 0;
        if (Mathf.Approximately(looking, 0.00f))
            dir = 0; // 아래
        else if (Mathf.Approximately(looking, 1.00f))
            dir = 1; // 위
        else if (Mathf.Approximately(looking, 0.33f))
            dir = 2; // 왼쪽
        else if (Mathf.Approximately(looking, 0.66f))
            dir = 3; // 오른쪽
        else
            dir = 0; // 기본값(아래)


        Vector3[] positions = {
        new Vector3(0, -0.5f, 0),       // 아래
        new Vector3(0, 0.4f, 0),        // 위
        new Vector3(-0.2f, -0.3f, 0),   // 왼쪽
        new Vector3(0.3f, -0.2f, 0)     // 오른쪽
    };

        float[] startZ = { -100f, 100f, -180f, -20f }; // 아래, 위, 왼쪽, 오른쪽

        batonTransform.gameObject.SetActive(true);
        batonTransform.localPosition = positions[dir];
        batonTransform.localEulerAngles = Vector3.zero;

        Quaternion startRotation = Quaternion.Euler(0, 0, startZ[dir]);
        Quaternion endRotation = Quaternion.Euler(0, 0, startZ[dir] + 100f);
        
        playerAnimator.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(RotateBaton(batonTransform, startRotation, endRotation, 0.1f));
    }

    private IEnumerator RotateBaton(Transform target, Quaternion from, Quaternion to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.localRotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if(swingClip != null && audioSource != null)
            audioSource.PlayOneShot(swingClip);

        target.localRotation = to;
        target.gameObject.SetActive(false);
    }
}
