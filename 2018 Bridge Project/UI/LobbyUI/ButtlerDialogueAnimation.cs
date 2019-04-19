using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtlerDialogueAnimation : MonoBehaviour
{
    [SerializeField]
    string dialogue;
    WaitForSeconds waitForSeconds;
    Text dialogueText;

    void Awake()
    {
        dialogueText = GetComponent<Text>();
        waitForSeconds = new WaitForSeconds(0.03f);
    }

    void OnEnable()
    {
        StartCoroutine(PlayDialogue());
    }

    void OnDisable()
    {
        dialogueText.text = "";
    }

    IEnumerator PlayDialogue()
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            yield return waitForSeconds;
            dialogueText.text += dialogue[i];
        }
    }
}
