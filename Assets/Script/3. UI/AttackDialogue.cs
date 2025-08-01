using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AttackDialogue : MonoBehaviour
{

    public GameObject descriptionPanel;

    [SerializeField] private TMP_Text attackTextUI;
    [SerializeField] private string[] attackLines;


    void Start()
    {
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);


        string[] attackLines = new string[] {
            "선배님! 지원 사격 들어갑니다!"

        };

        DialogueManager.Instance.ShowDialogue(attackLines, attackTextUI, () =>
        {
            descriptionPanel.SetActive(false);
        });
    }
}