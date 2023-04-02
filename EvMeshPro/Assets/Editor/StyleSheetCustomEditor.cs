using System;
using System.Management.Instrumentation;

using Codice.CM.SEIDInfo;

using TMPro;

using Unity.Mathematics;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SO_TextStyleList))]
public class StyleSheetCustomEditor : Editor
{

   private GameObject previewGameObject;
   
   public override void OnInspectorGUI() {
      
      if (GUILayout.Button("Preview In Scene")) {
        PreviewInScene();
      }
      base.OnInspectorGUI();
   }

   private void OnDisable() {
      if (previewGameObject) {
         DestroyImmediate(previewGameObject);
      }
   }

   public void PreviewInScene() {
      SO_TextStyleList previewList = (SO_TextStyleList)target;

      if (previewGameObject) { DestroyImmediate(previewGameObject); }

      GameObject previewObj = Resources.Load<GameObject>("Canvas_PreviewExample");
      previewGameObject = Instantiate(previewObj, Vector3.zero, quaternion.identity);
      TMP_Text textAsset = previewGameObject.GetComponentInChildren<TMP_Text>();

      string textToPreview = "[TEST]" + previewList.previewSampleText + "[/TEST]";

      DialogueController previewDialogueController = previewGameObject.AddComponent<DialogueController>();
      previewDialogueController.textStyleList = previewList;
      string parsedText = previewDialogueController.ParseDialogueCustomStyle(textToPreview);

      textAsset.text = parsedText;
   }
}
