using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{

    public GameObject descriptionPanel;

    [SerializeField] private TMP_Text playerTextUI;
    [SerializeField] private string[] playerLines;


    void Start()
    {
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);


        string[] playerLines = new string[] {
            "해저드와 기계팔에 공격이" +
            "\n먹히지 않는 것 같아.",
            "아스톨포 씨가 나를 부르고 있어." +
            "\n어서 가보자!",
        };

        DialogueManager.Instance.ShowDialogue(playerLines, playerTextUI, () =>
        {
            descriptionPanel.SetActive(false);
        });
    }
}