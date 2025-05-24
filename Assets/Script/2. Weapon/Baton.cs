using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baton : MonoBehaviour, IWeapon
{
    //public KeyCode Key => KeyCode.A;
    public float Damage => 5f;
    public Animator playerAnimator;
    public Transform batonTransform;

    public AudioSource audioSource;
    public AudioClip swingClip;

    public void Attack()
    {
        Debug.Log("휘두르기");


        if (playerAnimator == null)
        {
            Debug.LogError("playerAnimator가 할당되지 않음");
            return;
        }

        /*
        int dir = playerAnimator.GetInteger("Looking");

        dir -= 1;   // 플레이어가 보는 방향 정수 1~4를 0~3으로 변환
        */


        // 블렌더 트리로 바꾸며 받아온 Looking 값
        float looking = playerAnimator.GetFloat("Looking");

        // float 값을 방향 인덱스로 변환
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


        // 오른쪽 방향만 예외처리 했던 것
        //Quaternion startRotation;
        //Quaternion endRotation;

        /*
        if (dir == 3) // 오른쪽
        {
            startRotation = Quaternion.Euler(0, 0, -20f);
            endRotation = Quaternion.Euler(0, 0, 100f);
        }
        else
        {
            startRotation = Quaternion.Euler(0, 0, startZ[dir]);
            endRotation = Quaternion.Euler(0, 0, startZ[dir] + 80f);
        }
        */
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

    public void PlayAnimaion()
    {
        Debug.Log("진압봉 애니메이션 재생");
    }
}
