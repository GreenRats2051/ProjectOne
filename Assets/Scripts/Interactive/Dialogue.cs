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
    private int indexDialogue;
    [SerializeField]
    private float timeDialogue;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            textObject.SetActive(true);
            text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(dialogueSettings[indexDialogue].color)}>" + dialogueSettings[indexDialogue].nameCharacter + ": " + $"</color>" + dialogueSettings[indexDialogue].textCharacterDialogue;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log(indexDialogue);
            if (indexDialogue < dialogueSettings.Length - 1)
            {
                timeDialogue += Time.deltaTime;
                if (timeDialogue >= dialogueSettings[indexDialogue].timeNextDialogue)
                {
                    indexDialogue++;
                    timeDialogue = 0;
                }
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
