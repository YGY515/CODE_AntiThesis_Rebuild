using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float timer = 0f;
    public int spawnCount = 0;
    public int maxSpawnCount;
    public bool isSpawning;

    public Transform boss; 
    public Camera mainCam;

    public float spawnRange = 1.0f; 
    public float camShakeAmount = 0.3f;
    public float camShakeDuration = 0.2f;
    
    public AudioSource audioSource;
    public AudioClip hitClip;
    public StartDialogue startDialogue;


    void Update()
    {
        if (startDialogue.startGame && !isSpawning)
        {
            Spawn();
        }

        maxSpawnCount = BossPhaseManager.Instance.currentPhase switch
        {
            1 => 3,
            2 => 5,
            3 => 7
        };
    }

    void Spawn()
    {
        if (spawnCount >= maxSpawnCount)
        {   
            isSpawning = true; 
            spawnCount = 0; 
            return; 
        }

        timer += Time.deltaTime;
        if (timer > 1f)
        {
            GameObject enemy = EnemyPoolManager.Instance.Get(0);
            if (enemy != null)
            {

                Vector3 offset = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), -1);
                enemy.transform.position = boss.position + (Vector3)offset;

                if (audioSource != null && hitClip != null)
                    audioSource.PlayOneShot(hitClip);

                spawnCount++;
                timer = 0f;


                StartCoroutine(ShakeCamera());
                StartCoroutine(ShakeEnemy(enemy));
            }
        }
    }

    IEnumerator ShakeEnemy(GameObject enemy)
    {
        float elapsed = 0f;
        Vector3 originalPos = enemy.transform.position;

        while (elapsed < camShakeDuration)
        {
            float offsetY = Random.Range(-camShakeAmount, camShakeAmount);
            Vector3 shake = new Vector3(0, offsetY, 0);

            float riseY = Mathf.Lerp(0f, 0.5f, elapsed / camShakeDuration);
            enemy.transform.position = originalPos + shake + new Vector3(0, riseY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 잡몹이 상승 후 원래 위치로 돌아오도록
        enemy.transform.position = originalPos + new Vector3(0, 0.5f, 0);
    }
    IEnumerator ShakeCamera()
    {
        float elapsed = 0f;
        while (elapsed < camShakeDuration)
        {
            float offsetY = Random.Range(-camShakeAmount, camShakeAmount);
            Vector3 shake = new Vector3(0, offsetY, 0);
            mainCam.GetComponent<PlayerCamera>().ApplyShake(shake);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCam.GetComponent<PlayerCamera>().ResetShake();
    }

}