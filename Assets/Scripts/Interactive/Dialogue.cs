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
            text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(dialogueSettings[indexDialogueValue].color)}>" + dialogueSettings[indexDialogueValue].nameCharacter + ": " + $"</color>" + dialogueSettings[indexDialogueValue].textCharacterDialogue;
        }
    }
    private int indexDialogueValue;
    private float timeDialogue;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            textObject.SetActive(true);
            text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(dialogueSettings[indexDialogueValue].color)}>" + dialogueSettings[indexDialogueValue].nameCharacter + ": " + $"</color>" + dialogueSettings[indexDialogueValue].textCharacterDialogue;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            timeDialogue += Time.deltaTime;
            if (timeDialogue >= dialogueSettings[indexDialogue].timeNextDialogue && indexDialogue >= dialogueSettings.Length)
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
    public Color color;
    public string nameCharacter;
    public string textCharacterDialogue;
    public float timeNextDialogue;
}
