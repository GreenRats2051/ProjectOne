using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject DialogueMenu;
    public TMP_Text DialogueText;
    private int CurrentDialogue;
    public string[] Dialogues;
    private bool IAlreadyTalked;

    void Update()
    {
        if (CurrentDialogue != Dialogues.Length - 1 && IAlreadyTalked && Input.GetKeyDown(KeyCode.Return))
        {
            CurrentDialogue += 1;
        }
    }

    void OnTriggerStay(Collider Collider)
    {
        DialogueText.text = Dialogues[CurrentDialogue];
        DialogueMenu.SetActive(CurrentDialogue != Dialogues.Length);
        IAlreadyTalked = true;
    }

    void OnTriggerExit(Collider Collider)
    {
        DialogueMenu.SetActive(false);
        DialogueText.text = "<color=green>[Player]<color=white>: Test";
        IAlreadyTalked = false;
    }
}
