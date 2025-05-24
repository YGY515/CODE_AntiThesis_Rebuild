using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Description : MonoBehaviour
{
    public TMP_Text descriptionText;

    public string[] descriptions;
    public string[] dialogues;
    public int talkNum = 0;

    public AudioSource audioSource;
    public AudioClip typingClip;

    public GameObject descriptionPanel;
    private bool waitingForClick = false;


    void Start()
    {
        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);

        StartTalk(descriptions);
    }

    public void StartTalk(string[] talks)
    {
        dialogues = talks;
        talkNum = 0;

        if (descriptionPanel != null)
            descriptionPanel.SetActive(true);

        if (dialogues != null && dialogues.Length > 0)
            StartCoroutine(Typing(dialogues[talkNum]));
    }

    public void NextTalk()
    {
        if (talkNum > 1)
        {
            // 마지막 글자만 남기고, 그 이전은 초기화
            descriptionText.text = "";
        }
        
        talkNum++;

        if (talkNum == dialogues.Length)
        {
            StartCoroutine(WaitForClickToEnd());
            return;
        }

        StartCoroutine(Typing(dialogues[talkNum]));
    }

    public void EndTalk()
    {
        talkNum = 0;

        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        descriptionText.text = "";
    }

    IEnumerator Typing(string text)
    {
        descriptionText.text = "";

        // > 표시로 줄바꿈
        if (text.Contains(">")) text = text.Replace(">", "\n");

        // 두 글자마다 타이핑 소리 재생
        int soundInterval = 2;
        for (int i = 0; i < text.Length; i++)
        {
            descriptionText.text += text[i];

            if (typingClip != null && audioSource != null && i % soundInterval == 0)
                audioSource.PlayOneShot(typingClip);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
        NextTalk();
    }

    IEnumerator WaitForClickToEnd()
    {
        waitingForClick = true;

        while (!Input.GetMouseButtonDown(0))
            yield return null;

        waitingForClick = false;
        EndTalk();
    }
}