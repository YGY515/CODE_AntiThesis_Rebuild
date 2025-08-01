using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    public int Damage => 0; 
    public Transform gunTransform;
    public AudioClip shootClip;
    public LayerMask BossLayer;
    public float bulletRange = 5f;
    public float bulletAngle = 40f; // 총의 부채꼴 각도

    public Animator playerAnimator;
    public AudioSource audioSource;
    public BossHealth bossHealth;
    public GameObject boss;


    void OnDrawGizmosSelected()
    {
        // 씬 뷰에서 기즈모로 총 범위 보이게
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); 
        Gizmos.DrawWireSphere(transform.position, bulletRange);

        Vector3 origin = transform.position;

        int dir = 0;
        if (playerAnimator != null)
        {
            float looking = playerAnimator.GetFloat("Looking");
            if (Mathf.Approximately(looking, 0.00f)) dir = 0; 
            else if (Mathf.Approximately(looking, 1.00f)) dir = 1; 
            else if (Mathf.Approximately(looking, 0.33f)) dir = 2; 
            else if (Mathf.Approximately(looking, 0.66f)) dir = 3; 
        }

        Vector3 forward = Vector3.down;
        switch (dir)
        {
            case 0: forward = Vector3.down; break;
            case 1: forward = Vector3.up; break;
            case 2: forward = Vector3.left; break;
            case 3: forward = Vector3.right; break;
        }

        int segments = 30;
        float halfAngle = bulletAngle * 0.5f;
        float startAngle = -halfAngle;
        float deltaAngle = bulletAngle / segments;

        Vector3 prevPoint = origin + Quaternion.Euler(0, 0, startAngle) * forward * bulletRange;
        for (int i = 1; i <= segments; i++)
        {
            float currAngle = startAngle + deltaAngle * i;
            Vector3 nextPoint = origin + Quaternion.Euler(0, 0, currAngle) * forward * bulletRange;
            Gizmos.color = new Color(0f, 0.7f, 1f, 0.7f); 
            Gizmos.DrawLine(origin, nextPoint);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
    public void Attack()
    {
        if (playerAnimator == null)
        {
            Debug.LogError("playerAnimator가 할당 필요");
            return;
        }

        float looking = playerAnimator.GetFloat("Looking");
        int dir = 0;
        if (Mathf.Approximately(looking, 0.00f)) dir = 0; 
        else if (Mathf.Approximately(looking, 1.00f)) dir = 1; 
        else if (Mathf.Approximately(looking, 0.33f)) dir = 2; 
        else if (Mathf.Approximately(looking, 0.66f)) dir = 3; 

        Vector3[] positions = {
        new Vector3(0, -0.4f, 0),    
        new Vector3(0, 0.4f, 0),     
        new Vector3(-0.3f, -0.1f, 0),
        new Vector3(0.3f, -0.1f, 0)  
    };

        float[] startZ = { -90f, 90f, 180f, 0f }; 

        gunTransform.gameObject.SetActive(true);
        gunTransform.localPosition = positions[dir];
        gunTransform.localEulerAngles = Vector3.zero;

        Quaternion startRot = Quaternion.Euler(0, 0, startZ[dir] - 15f);
        Quaternion endRot = Quaternion.Euler(0, 0, startZ[dir] + 15f);

        bool isLeft = dir == 2;

        if (isLeft)
        {
            gunTransform.localScale = new Vector3(-0.7f, 0.7f, 0.7f); 
            startRot = Quaternion.Euler(0, 0, -startZ[3] + 15f);
            endRot = Quaternion.Euler(0, 0, -startZ[3] - 15f);
        }
        else
        {
            gunTransform.localScale = new Vector3(0.7f, 0.7f, 0); 
            startRot = Quaternion.Euler(0, 0, startZ[dir] - 15f);
            endRot = Quaternion.Euler(0, 0, startZ[dir] + 15f);
            }

            playerAnimator.gameObject.GetComponent<MonoBehaviour>()
            .StartCoroutine(GunRecoilRoutine(gunTransform, startRot, endRot, 0.03f, dir));
    }

    IEnumerator GunRecoilRoutine(Transform target, Quaternion from, Quaternion to, float duration, int dir)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.localRotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localRotation = to;
        if (shootClip != null && audioSource != null)
            audioSource.PlayOneShot(shootClip);


        BulletAttack(dir);

        yield return new WaitForSeconds(0.05f);
        target.gameObject.SetActive(false);
    }

    void BulletAttack(int dir)
    {
        Vector2 fireDir = Vector2.down;
        switch (dir)
        {
            case 0: fireDir = Vector2.down; break;
            case 1: fireDir = Vector2.up; break;
            case 2: fireDir = Vector2.left; break;
            case 3: fireDir = Vector2.right; break;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bulletRange, BossLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(fireDir, toTarget);

            if (angle <= bulletAngle / 2f)
            {
                bool isCritical = Random.value < 0.3f; // 치명타 30퍼
                int Damage = isCritical ? 30 : Random.Range(20, 29); // 치명타면 공격력 30, 아니면 20~29 사이

                Debug.Log("보스가 맞는중");
                bossHealth.BossTakeDamage(Damage);

                DamageTextManager.Instance.ShowDamage(
                    boss.transform.position, Damage, isCritical

                );
            }
        }
    }
}

  
