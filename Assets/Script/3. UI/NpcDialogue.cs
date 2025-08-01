using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
    public Animator npcAnimation;
    public GameObject descriptionPanel;
    public GameObject timer;
    public EnemySpawner enemySpawner;
    public PlayerHealth playerHealth;

    [SerializeField] private TMP_Text npcTextUI;
    [SerializeField] private string[] npcLines;


    void OnEnable()
    {
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);

        string[] npcLines = new string[] {
        "제가 잠시 해킹할" +
        "\n시간을 벌어주세요!",
        "그리고 기계팔의" +
        "\n방어막도 꺼볼게요!",
        "해킹이 성공하면 해저드에게" +
        "\n유효타를 날릴 수 있어요!"
    };

        string[] npcLines2 = new string[]
        {
        "먹혔나요?" +
        "\n다행이다!",
        "한번 더 기다려주세요." +
        "\n다시 해볼게요!"
        };

        string[] npcLines3 = new string[]
        {
        "세상에 네이젤 씨," +
        "\n엄청 다치셨네요...!",
        "제가 가진 간이붕대를" +
        "\n감아드릴게요!"
        };

        string[] linesToUse;

        if (BossPhaseManager.Instance.currentPhase == 1)
            linesToUse = npcLines;
        else if (BossPhaseManager.Instance.currentPhase == 2)
            linesToUse = npcLines2;
        else
            linesToUse = npcLines3;

        npcAnimation.SetFloat("Looking", 0.66f); // 대화 출력 시 우측 바라보기


        DialogueManager.Instance.ShowDialogue(linesToUse, npcTextUI, () =>
        {
            if (playerHealth.CurrentHealth == 1)
            {
                Debug.Log("NPC가 추가 대사를 아직 출력 안함");
                DialogueManager.Instance.ShowDialogue(npcLines3, npcTextUI, () =>
                {
                    Debug.Log("NPC가 추가 대사를 출력했음");
                    playerHealth.PlayerHeal(1);
                    Debug.Log($"주인공 체력 회복함. 이제 체력이 {playerHealth.CurrentHealth}임");

                    // 추가 대사 끝난 뒤에 후처리
                    timer.SetActive(true);
                    npcAnimation.SetFloat("Looking", 0.33f);
                    enemySpawner.isSpawning = false; // 잡몹 스폰 허용
                    descriptionPanel.SetActive(false);
                });
            }
            else
            {
                // 추가 대사가 필요 없을 때만 바로 후처리
                timer.SetActive(true);
                npcAnimation.SetFloat("Looking", 0.33f);
                descriptionPanel.SetActive(false);
            }
        });
    }


}