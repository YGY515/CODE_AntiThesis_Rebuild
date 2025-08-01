using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public Animator anim;
    public Transform player; 
    public float interactDistance = 2f;

    public EnemyController[] enemies;
    public GameObject bossShield;
    public GameObject NpcDialogue;


    void Start()
    {
        
        if (anim != null)
            anim.SetFloat("Looking", 0.33f);    // 시작 시 좌측 바라보기
    }

    void Update()
    {
        Interaction();
    }

    void Interaction()
    {
        if (player != null && bossShield != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= interactDistance && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("NPC와 상호작용 시작");
                anim.SetFloat("Looking", 0.66f);
                NpcDialogue.SetActive(true);
                OnPhaseChanged(BossPhaseManager.Instance.currentPhase);

            }
        }
    }

    void OnPhaseChanged(int phase)
    {
        NpcDialogue.SetActive(true);
        EnableEnemyDamage();

        if (phase == 1) { }
        else if (phase == 2)
        {
            
        }
    }

    void EnableEnemyDamage()
    {
        EnemyPoolManager.Instance.EnemyCanBeDamaged = true;

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.canBeDamaged = true;
            }
        }
    }



}