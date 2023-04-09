using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DEBUG_TestDialogue : MonoBehaviour
{
	private void Start() {
		Invoke("TestMulitBoxes", 2f);
		// TestMulitBoxes();
	}

	[ContextMenu("Test Multi Boxes")]
	public void TestMulitBoxes() {
		// DialogueController.instance.NewDialogueInstance("Hey! Hope you are doing well. This is a test dialogue instance for the new dialogue package EvMeshPro","character_leo");
		// DialogueController.instance.NewDialogueInstance("This is a [NAMES]comprehensive package[/NAMES] to give developers an easy to use dialogue system."); 
		// DialogueController.instance.NewDialogueInstance("[NAMES]This[/NAMES] is [EXAGGERATE]a[/EXAGGERATE] [NAMES]comprehensive[/NAMES] package to [EXAGGERATE]Punctuation Test. This is to test! To make sure, That - some of this[/EXAGGERATE] an easy to use dialogue system."); 
		// DialogueController.instance.NewDialogueInstance("This is a comprehensive package to give developers an easy to use dialogue system.");
		
		// DialogueController.instance.NewDialogueInstance("Oh my god [NAMES]Evan[/NAMES]... They are about to [EXAGGERATE]fight![/EXAGGERATE] Should we step in?");
		// DialogueController.instance.NewDialogueInstance("Good afternoon traveller. I require your services... There is a caravan of bandits making their way to our settlement, and I would" +
		//                                                 " be forever in your debt if you could take care of them.");
		
		// DialogueController.instance.NewDialogueInstance("xBot, [GREEN]reckon[/GREEN] you could [NAMES]make short[/NAMES] work of this encryption?");
		DialogueController.instance.NewDialogueInstance("Oh hot damn, those are some [GREEN]FUNKY[/GREEN] text animations. [NAMES]This shit's kinda nice[/NAMES]... Custom text styles now feature animated text options!");
		
		// DialogueController.instance.NewDialogueInstance("Hey! How are you doing? You look good!"); 
		
		// DialogueController.instance.NewDialogueInstance("I dont have anything to say but we needed to test another box...");
		// DialogueController.instance.NewDialogueInstance("I dont have anything to say but we needed to test another box...", "character_leo");
		// DialogueController.instance.NewDialogueInstance("I dont have anything to say but we needed to test another box...", "character_raph");
		// DialogueController.instance.NewDialogueInstance("I dont have anything to say but we needed to test another box...", "character_don");
		// DialogueController.instance.NewDialogueInstance("I dont have anything to say but we needed to test another box...", "character_mick");

	}
}
