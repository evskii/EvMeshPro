using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    #region Singleton Creation
    //Create a singleton to call from anywhere (as long as prefab is in scene)
    public static DialogueController instance;
    private void Awake() {
        instance = this;
    }
    #endregion 

    [Header("Important References")]
    [SerializeField] private GameObject dialogueBoxPrefab;
    [SerializeField][Tooltip("Set this to the parent transform of where you want your dialogue boxes to spawn.")] private Transform dialogueBoxParent;
    
    [Header("Dialogue Settings")]
    [SerializeField][Tooltip("This dictates how long dialogue boxes stay on screen. Lower/Higher to make them last longer/shorter")] private int wpmReadingSpeed;
    [SerializeField] [Tooltip("Dictates how fast text appears in the box. Use 0 if you wish for it to appear immediately.")] private float textTypeSpeed = 0.1f;
    [SerializeField] private SO_CharacterList characterList;
    
    [Header("Textbox Lerp Settings")]
    [SerializeField][Tooltip("Lerp the texbox into place once it is called. This can be configured on the TextBox prefabs UiLerpElement component.")] 
    private bool lerpDialogueBoxesIn = false;
    [SerializeField][Tooltip("True: Lerp effect is only applied to the first box showing up in the que. \n" +
                            "False: Lerp effect is applied to every box in the que.")] 
    private bool onlyLerpFirstBoxInQue = false;
    
    
    private List<GameObject> dialogueInstanceQue = new List<GameObject>();
    private Coroutine queIterationCoroutine;
    private bool firstQueIndex = false; //Lets us know if this is the first textbox in the current que (IMPROVE THIS PLEASE)
    
    //Base method that only utilizes dialogue
    public void NewDialogueInstance(string dialogue) {
        GameObject newDialogueBox = Instantiate(dialogueBoxPrefab, dialogueBoxParent);
        newDialogueBox.GetComponent<Textbox>().InitializeTextbox(dialogue);
        newDialogueBox.SetActive(false);
        
        dialogueInstanceQue.Add(newDialogueBox);
        if (queIterationCoroutine == null) {
            firstQueIndex = true;
            queIterationCoroutine = StartCoroutine(IterateQue());
        }
    }
    
    public void NewDialogueInstance(string dialogue, string characterID) {
        if (characterList == null) {
            Debug.Log("<color=cyan>Trying to reference a characterID, however there is no CharacterList referenced in your DialogueController.</color>");
            return;
        }
        
        CharacterProfile characterProfile = characterList.GetCharacter(characterID);

        if (characterProfile.characterName == "NULL") {
            Debug.Log("<color=cyan>GetCharacter returned NULL. Not creating new dialogue instance.. Sorry </color>");
            return;
        }
        
        GameObject newDialogueBox = Instantiate(dialogueBoxPrefab, dialogueBoxParent);
        newDialogueBox.GetComponent<Textbox>().InitializeTextbox(dialogue, characterProfile);
        newDialogueBox.SetActive(false);
        
        dialogueInstanceQue.Add(newDialogueBox);
        if (queIterationCoroutine == null) {
            firstQueIndex = true;
            queIterationCoroutine = StartCoroutine(IterateQue());
        }
    }
    

    private IEnumerator IterateQue() {
        dialogueInstanceQue[0].SetActive(true);

        //If user wants dialogue to lerp in, then only call it on the first textbox in the que
        if (lerpDialogueBoxesIn) {
            if (dialogueInstanceQue[0].TryGetComponent(out UILerpElement lerpElement)) {
                if (onlyLerpFirstBoxInQue && firstQueIndex) {
                    lerpElement.StartLerp();
                }

                if (!onlyLerpFirstBoxInQue) {
                    lerpElement.StartLerp();
                }
                
            } else {
                Debug.Log("<color=cyan>Trying to lerp dialogue box, however we could not find UILerpElement.cs on it!</color>");
            }
        }

        //Get how long the dialogue box should appear for
        Textbox currentTextBox = dialogueInstanceQue[0].GetComponent<Textbox>();
        float displayLength = currentTextBox.dialogue.Split(' ').Length / (wpmReadingSpeed / 60);

        if (currentTextBox.dialogue.Length * textTypeSpeed >= displayLength) {
            Debug.LogWarning("<color=cyan>Your textTypeSpeed is too slow in comparison to your wpmReadingSpeed. Dialogue box will disappear before all text is shown.</color>");
        }
        
        currentTextBox.DisplayText(textTypeSpeed);

        yield return new WaitForSeconds(displayLength);
        
        var toDestroy = dialogueInstanceQue[0];
        dialogueInstanceQue.Remove(toDestroy);
        Destroy(toDestroy);

        firstQueIndex = false;
        
        if (dialogueInstanceQue.Count > 0) {
            queIterationCoroutine = StartCoroutine(IterateQue());
        } else {
            queIterationCoroutine = null;
        }
    }

}
