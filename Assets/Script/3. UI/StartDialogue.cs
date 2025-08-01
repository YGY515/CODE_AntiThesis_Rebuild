using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{

    public GameObject descriptionPanel;
    public GameObject bossHealth;
    public EnemySpawner enemySpawner;
    public bool startGame = false;

    [SerializeField] private TMP_Text startTextUI;
    [SerializeField] private string[] startLines;


    void Start()
    {
        startGame = false;
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);

        string[] startLines = new string[] {
            "* z키를 눌러 해저드를 제압해 주세요.\n" +
            "Shift 키를 눌러 날개를 펼쳐 빠르게 움직일 수 있습니다.",

            "* 또한 x키로 짧게 대쉬해서 피할 수 있으며,\n" +
            "Tab 키를 눌러 무기를 변경할 수 있습니다."
        };

        DialogueManager.Instance.ShowDialogue(startLines, startTextUI, () => {
            descriptionPanel.SetActive(false);
            bossHealth.SetActive(true);
            startGame = true;
        });
    }

}