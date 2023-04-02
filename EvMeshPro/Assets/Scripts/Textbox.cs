using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        //Regex patterns for detecting RichText tags in our string
        string styleTextPattern = @">[^<]+</"; //Regex pattern to find '> SOMETHING <' 
        string openTagPattern = @"(<[^>/]+>)+"; //Pattern to find <x><y><z>
        string closeTagPattern = @"(</[^>]+>)+"; //Pattern to fid </x></y></z>

        string cleanDialogue = dialogue;

        //Get our substrings that are styled
        List<StyleTextChunk> styleTextChunks = new List<StyleTextChunk>();
        var regexMatches = Regex.Matches(dialogue, styleTextPattern);
        for (int i = 0; i < regexMatches.Count; i++) {
            StyleTextChunk styleChunk = new StyleTextChunk();

            styleChunk.styledText = regexMatches[i].ToString().Replace(">", "").Replace("</", "");
            styleChunk.openTagString = Regex.Matches(dialogue, openTagPattern)[i].ToString();
            styleChunk.closeTagString = Regex.Matches(dialogue, closeTagPattern)[i].ToString();
            styleChunk.styledTextStartIndex = regexMatches[i].Index - styleChunk.openTagString.Length + 1;

            cleanDialogue = cleanDialogue.Replace(styleChunk.openTagString, "");
            cleanDialogue = cleanDialogue.Replace(styleChunk.closeTagString, "");

            
            
            styleTextChunks.Add(styleChunk);
        }
        


        int workingIndex = 0;
        string displayText = "";
        int styleChunkIndex = 0;
        
        
        foreach (char letter in cleanDialogue) {
            if (styleTextChunks.Count > 0) {
                StyleTextChunk currentStyleChunk = styleTextChunks[styleChunkIndex];

                if (workingIndex < currentStyleChunk.styledTextStartIndex) { 
                    // Debug.Log("Normal Letter: " + letter);
                    displayText += letter;
                }else if (workingIndex == currentStyleChunk.styledTextStartIndex) {
                    // Debug.Log("Starting Tags: " + letter);
                    displayText += currentStyleChunk.openTagString;
                    displayText += letter;
                    workingIndex = displayText.Length-1;
                    displayText += currentStyleChunk.closeTagString;
                }else if (workingIndex > currentStyleChunk.styledTextStartIndex && workingIndex < currentStyleChunk.styledTextStartIndex + currentStyleChunk.styledText.Length + currentStyleChunk.openTagString.Length) {
                    // Debug.Log("Inside Tags: " + letter);
                    displayText = displayText.Insert(workingIndex, letter.ToString());
                }

                if (workingIndex >= currentStyleChunk.styledTextStartIndex + currentStyleChunk.styledText.Length + currentStyleChunk.openTagString.Length) {
                    // Debug.Log("Exiting Style: " + letter);
                    workingIndex = displayText.Length ;
                    styleChunkIndex++;
                    if (styleChunkIndex >= styleTextChunks.Count - 1) {
                        styleChunkIndex = styleTextChunks.Count - 1;
                    }
                    displayText += letter;
                }
                
            } else {
                displayText += letter;
            }
            
            
            workingIndex++;

            dialogueText.text = displayText;
            
            yield return new WaitForSeconds(typeSpeed);
        }
    }
    
    struct StyleTextChunk
    {
        public string openTagString;
        public string closeTagString;

        public int styledTextStartIndex;
        public string styledText;

        public void PrintInfo() {
            Debug.Log("New Style Chunk: \n" +
                      "Index: " + styledTextStartIndex + "\n" +
                      "String: " +
                      styledText +
                      "\n" +
                      "Open Tag: " +
                      openTagString +
                      "\n" +
                      "Close Tag: " +
                      closeTagString);
        }
    }

}
