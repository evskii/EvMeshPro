using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    //Text
    [SerializeField] private TMP_Text dialogueText;
    [HideInInspector] public string dialogue;
    
    //Character Name
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image nameTextImage;

    //Character Sprite
    [SerializeField] private Image characterSpriteBackground; 
    [SerializeField] private Image characterSpriteImage; 
    
    private CharacterProfile myCharater;
    
    public void InitializeTextbox(string dialogue) {
        this.dialogue = dialogue;

        nameText.text = "";
        nameTextImage.enabled = false;
        
        characterSpriteBackground.gameObject.SetActive(false);
    }

    public void InitializeTextbox(string dialogue, CharacterProfile myCharacter) {
        this.myCharater = myCharacter;
        
        this.dialogue = dialogue;

        nameText.text = this.myCharater.characterName;
        nameTextImage.enabled = true;
        nameTextImage.color = this.myCharater.characterColor;

        characterSpriteBackground.gameObject.SetActive(true);
        characterSpriteBackground.color = this.myCharater.characterColor;

        characterSpriteImage.sprite = this.myCharater.characterSprite;
    }

    public void DisplayText() {
        dialogueText.text = dialogue;
    }

    public void DisplayText(float typeSpeed) {
        dialogueText.text = "";
        StartCoroutine(OneLetterAtAtime(typeSpeed));
    }

    private IEnumerator OneLetterAtAtime(float typeSpeed) {
        foreach (char letter in dialogue) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

}
