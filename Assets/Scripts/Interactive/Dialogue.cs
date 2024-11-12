using System;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject textObject;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private DialogueSettings[] dialogueSettings;
    private int indexDialogue
    {
        get => indexDialogueValue;
        set
        {
            indexDialogueValue = value;
            text.text = dialogueSettings[indexDialogueValue].textDialogue;
        }
    }
    private int indexDialogueValue;
    private float timeDialogue;

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            textObject.SetActive(true);
            timeDialogue += Time.deltaTime;
            if (timeDialogue >= dialogueSettings[indexDialogue].timeNextDialogue)
            {
                indexDialogue++;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        textObject.SetActive(false);
        timeDialogue = 0;
    }
}

[Serializable]
public class DialogueSettings
{
    public string textDialogue;
    public float timeNextDialogue;
}
