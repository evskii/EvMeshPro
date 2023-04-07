using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

using TMPro;

using UnityEditor;
using UnityEditor.Build;

using UnityEngine;
using UnityEngine.UIElements;

using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class TextAnimations : MonoBehaviour
{
    //Mesh References

    [Header("Settings")]
    [SerializeField] private TMP_Text textMesh;
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    [Header("Wave")]
    [SerializeField] private bool useBounceEffect;
    [SerializeField] private float bounceScale = 1f;
    [SerializeField] private float bounceFrequency = 1f;

    [Header("Scale")]
    [SerializeField] private bool useScaleEffect;
    [SerializeField] private float scaleRate;
    [SerializeField] private float scaleMax;
    [SerializeField] private float scaleMin;

    [Header("Rainbow")]
    [SerializeField] private bool useRainbowEffect;
    [SerializeField][Range(0.001f, 0.01f)] private float rainbowWidth;
    private enum RainbowDirection
    {
        Horizontal,
        Vertical
    }
    [SerializeField] private RainbowDirection rainbowDirection;
    [SerializeField] private  Gradient rainbow;
    
    
    [Header("Rotate")]
    [SerializeField] private bool useRotateEffect;
    [SerializeField] private bool pingPongRotation;
    [SerializeField] private float rotateAngle;
    [SerializeField] private float rotateFrequency;
    [SerializeField] private Vector3 rotationAxis;

    private List<TextAnimationInfo> animationInfos = new List<TextAnimationInfo>();
    

    // private void AnimateMesh() {
    //     textMesh.ForceMeshUpdate();
    //     mesh = textMesh.mesh;
    //     vertices = mesh.vertices;
    //     colors = mesh.colors;
    //
    //     for (int i = 0; i < textMesh.textInfo.characterCount; i++) {
    //         
    //         TMP_CharacterInfo character = textMesh.textInfo.characterInfo[i];
    //
    //         if (character.isVisible) {
    //             int startIndex = character.vertexIndex;
    //             //Scale Effect
    //             if (useScaleEffect) {
    //                 float halfRange = (scaleMax - scaleMin) / 2;
    //                 float scaleAmount = scaleMin + halfRange + Mathf.Sin(Time.time * scaleRate) * halfRange;
    //                 Vector3 scaleVector = new Vector3(scaleAmount, scaleAmount, 0);
    //
    //                 vertices[startIndex].Scale(scaleVector);
    //                 vertices[startIndex + 1].Scale(scaleVector);
    //                 vertices[startIndex + 2].Scale(scaleVector);
    //                 vertices[startIndex + 3].Scale(scaleVector);
    //             }
    //
    //             //Bounce Effect
    //             if (useBounceEffect) {
    //                 Vector3 offset = new Vector3(0, Mathf.Sin((Time.time + i * bounceFrequency)) * bounceScale, 0);
    //             
    //                 vertices[startIndex] += offset;
    //                 vertices[startIndex + 1] += offset;
    //                 vertices[startIndex + 2] += offset;
    //                 vertices[startIndex + 3] += offset;
    //             }
    //             
    //             //Rainbow Effect
    //             if (useRainbowEffect) {
    //                 switch (rainbowDirection) {
    //                     case RainbowDirection.Horizontal:
    //                         colors[startIndex] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex].x * rainbowWidth, 1f));
    //                         colors[startIndex + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 1].x * rainbowWidth, 1f));
    //                         colors[startIndex + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 2].x * rainbowWidth, 1f));
    //                         colors[startIndex + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 3].x * rainbowWidth, 1f));
    //                         break;
    //                     case RainbowDirection.Vertical:
    //                         colors[startIndex] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex].y * rainbowWidth, 1f));
    //                         colors[startIndex + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 1].y * rainbowWidth, 1f));
    //                         colors[startIndex + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 2].y * rainbowWidth, 1f));
    //                         colors[startIndex + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 3].y * rainbowWidth, 1f));
    //                         break;
    //                 }
    //             }
    //
    //             //Rotation Effect
    //             if (useRotateEffect) {
    //                 Vector3 center = vertices[startIndex];
    //                 Quaternion newRotation = new Quaternion();
    //                 var pivotAmount = pingPongRotation ? Mathf.Sin(Time.time + i * rotateFrequency) * rotateAngle : (Time.time + i * rotateFrequency) * rotateAngle;
    //                 newRotation.eulerAngles = rotationAxis * pivotAmount;
    //                 
    //                 vertices[startIndex] = newRotation * (vertices[startIndex] - center) + center;
    //                 vertices[startIndex + 1] = newRotation * (vertices[startIndex + 1] - center) + center;
    //                 vertices[startIndex + 2] = newRotation * (vertices[startIndex + 2] - center) + center;
    //                 vertices[startIndex + 3] = newRotation * (vertices[startIndex + 3] - center) + center;
    //                 
    //             }
    //         }
    //         
    //         
    //     }
    //
    //     mesh.vertices = vertices;
    //     mesh.colors = colors;
    //     textMesh.canvasRenderer.SetMesh(mesh);
    // }

    public void AddAnimationInfo(TextAnimationInfo animationInfo) {
        animationInfos.Add(animationInfo);
    }

    private void Update() {
        AnimateMesh();
    }

    public void AnimateMesh() {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        colors = mesh.colors;
        

        foreach (TextAnimationInfo animationInfo in animationInfos) {
            
            for (int i = animationInfo.animStartIndex; i < textMesh.textInfo.characterInfo.Length; i++) {
                // Debug.Log(i);
                TMP_CharacterInfo character = textMesh.textInfo.characterInfo[i];

                if (character.isVisible && (i >= animationInfo.animStartIndex && i <= animationInfo.animEndIndex)) {
                    int startIndex = character.vertexIndex;
                    //Scale Effect
                    if (useScaleEffect) {
                        float halfRange = (scaleMax - scaleMin) / 2;
                        float scaleAmount = scaleMin + halfRange + Mathf.Sin(Time.time * scaleRate) * halfRange;
                        Vector3 scaleVector = new Vector3(scaleAmount, scaleAmount, 0);

                        vertices[startIndex].Scale(scaleVector);
                        vertices[startIndex + 1].Scale(scaleVector);
                        vertices[startIndex + 2].Scale(scaleVector);
                        vertices[startIndex + 3].Scale(scaleVector);
                    }

                    //Bounce Effect
                    if (useBounceEffect) {
                        Vector3 offset = new Vector3(0, Mathf.Sin((Time.time + i * bounceFrequency)) * bounceScale, 0);
                    
                        vertices[startIndex] += offset;
                        vertices[startIndex + 1] += offset;
                        vertices[startIndex + 2] += offset;
                        vertices[startIndex + 3] += offset;
                    }
                    
                    //Rainbow Effect
                    if (useRainbowEffect) {
                        switch (rainbowDirection) {
                            case RainbowDirection.Horizontal:
                                colors[startIndex] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex].x * rainbowWidth, 1f));
                                colors[startIndex + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 1].x * rainbowWidth, 1f));
                                colors[startIndex + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 2].x * rainbowWidth, 1f));
                                colors[startIndex + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 3].x * rainbowWidth, 1f));
                                break;
                            case RainbowDirection.Vertical:
                                colors[startIndex] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex].y * rainbowWidth, 1f));
                                colors[startIndex + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 1].y * rainbowWidth, 1f));
                                colors[startIndex + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 2].y * rainbowWidth, 1f));
                                colors[startIndex + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 3].y * rainbowWidth, 1f));
                                break;
                        }
                    }

                    //Rotation Effect
                    if (useRotateEffect) {
                        Vector3 center = vertices[startIndex];
                        Quaternion newRotation = new Quaternion();
                        var pivotAmount = pingPongRotation ? Mathf.Sin(Time.time + i * rotateFrequency) * rotateAngle : (Time.time + i * rotateFrequency) * rotateAngle;
                        newRotation.eulerAngles = rotationAxis * pivotAmount;
                        
                        vertices[startIndex] = newRotation * (vertices[startIndex] - center) + center;
                        vertices[startIndex + 1] = newRotation * (vertices[startIndex + 1] - center) + center;
                        vertices[startIndex + 2] = newRotation * (vertices[startIndex + 2] - center) + center;
                        vertices[startIndex + 3] = newRotation * (vertices[startIndex + 3] - center) + center;
                        
                    }
                }

            }
        }
        
        

        mesh.vertices = vertices;
        mesh.colors = colors;
        textMesh.canvasRenderer.SetMesh(mesh);
    }
}

public class TextAnimationInfo
{
    public int animStartIndex;
    public int animEndIndex;


    public TextAnimationInfo(int startIndex, int endIndex) {
        animStartIndex = startIndex;
        animEndIndex = endIndex;
    }
}
