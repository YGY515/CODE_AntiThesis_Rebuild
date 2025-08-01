using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip typingClip;

    public float typingSpeed = 0.05f;
    public bool isTyping = false;
    private Coroutine typingCoroutine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ShowDialogue(string[] lines, TMP_Text targetText, System.Action onFinish = null)
    {
        isTyping = true;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLines(lines, targetText, onFinish));
    }

    private IEnumerator TypeLines(string[] lines, TMP_Text targetText, System.Action onFinish)
    {
        foreach (var raw in lines)
        {
            string text = raw.Replace(">", "\n");
            targetText.text = "";

            for (int i = 0; i < text.Length; i++)
            {
                targetText.text += text[i];

                if (i % 2 == 0 && typingClip != null)
                    audioSource.PlayOneShot(typingClip);

                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(2f);
        }

        onFinish?.Invoke();
        isTyping = false;
    }
}