using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UIElements;

using ColorUtility = UnityEngine.ColorUtility;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class TextAnimations : MonoBehaviour
{
    //Mesh References

    [Header("Settings")]
    [SerializeField] private TMP_Text textMesh;
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

   

    private List<TextAnimationInfo> animationSettings = new List<TextAnimationInfo>();
    

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
        animationSettings.Add(animationInfo);
    }

    private void Update() {
        AnimateMesh();
    }

    public void AnimateMesh() {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;
        colors = mesh.colors;
        

        foreach (TextAnimationInfo animationSettings in animationSettings) {
            
            for (int i = animationSettings.animStartIndex; i < textMesh.textInfo.characterInfo.Length; i++) {
                // Debug.Log(i);
                TMP_CharacterInfo character = textMesh.textInfo.characterInfo[i];

                if (character.isVisible && (i >= animationSettings.animStartIndex && i <= animationSettings.animEndIndex)) {
                    int startIndex = character.vertexIndex;
                    //Scale Effect
                    if (animationSettings.useScaleEffect) {
                        float halfRange = (animationSettings.scaleMax - animationSettings.scaleMin) / 2;
                        float scaleAmount = animationSettings.scaleMin + halfRange + Mathf.Sin(Time.time * animationSettings.scaleRate) * halfRange;
                        Vector3 scaleVector = new Vector3(scaleAmount, scaleAmount, 1);

                        vertices[startIndex].Scale(scaleVector);
                        vertices[startIndex + 1].Scale(scaleVector);
                        vertices[startIndex + 2].Scale(scaleVector);
                        vertices[startIndex + 3].Scale(scaleVector);
                    }

                    //Bounce Effect
                    if (animationSettings.useBounceEffect) {
                        Vector3 offset = new Vector3(0, Mathf.Sin((Time.time + i * animationSettings.bounceFrequency)) * animationSettings.bounceScale, 0);
                    
                        vertices[startIndex] += offset;
                        vertices[startIndex + 1] += offset;
                        vertices[startIndex + 2] += offset;
                        vertices[startIndex + 3] += offset;
                    }
                    
                    //Rainbow Effect
                    if (animationSettings.useRainbowEffect) {
                        switch (animationSettings.rainbowDirection) {
                            case TextAnimationInfo.RainbowDirection.Horizontal:
                                colors[startIndex] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex].x * animationSettings.rainbowWidth, 1f));
                                colors[startIndex + 1] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 1].x * animationSettings.rainbowWidth, 1f));
                                colors[startIndex + 2] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 2].x * animationSettings.rainbowWidth, 1f));
                                colors[startIndex + 3] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 3].x * animationSettings.rainbowWidth, 1f));
                                break;
                            case TextAnimationInfo.RainbowDirection.Vertical:
                                colors[startIndex] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex].y * animationSettings.rainbowWidth, 1f));
                                colors[startIndex + 1] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 1].y * animationSettings.rainbowWidth, 1f));
                                colors[startIndex + 2] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 2].y * animationSettings.rainbowWidth, 1f));
                                colors[startIndex + 3] = animationSettings.rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[startIndex + 3].y * animationSettings.rainbowWidth, 1f));
                                break;
                        }
                    }

                    //Rotation Effect
                    if (animationSettings.useRotateEffect) {
                        Vector3 center = vertices[startIndex];
                        Quaternion newRotation = new Quaternion();
                        var pivotAmount = animationSettings.pingPongRotation ? Mathf.Sin(Time.time + i * animationSettings.rotateFrequency) * animationSettings.rotateAngle : (Time.time + i * animationSettings.rotateFrequency) * animationSettings.rotateAngle;
                        newRotation.eulerAngles = animationSettings.rotationAxis * pivotAmount;
                        
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

[Serializable]
public class TextAnimationInfo
{
    [HideInInspector] public int animStartIndex;
    [HideInInspector] public int animEndIndex;
    
    [Header("Wave")]
    public bool useBounceEffect;
    public float bounceScale = 1f;
    public float bounceFrequency = 1f;

    [Header("Scale")]
    public bool useScaleEffect;
    public float scaleRate;
    public float scaleMax;
    public float scaleMin;

    [Header("Rainbow")]
    public bool useRainbowEffect;
    [Range(0.001f, 0.01f)] public float rainbowWidth;
    public enum RainbowDirection
    {
        Horizontal,
        Vertical
    }
    public RainbowDirection rainbowDirection;
    public  Gradient rainbow;
    
    
    [Header("Rotate")]
    public bool useRotateEffect;
    public bool pingPongRotation;
    public float rotateAngle;
    public float rotateFrequency;
    public Vector3 rotationAxis;


    public TextAnimationInfo(int startIndex, int endIndex) {
        animStartIndex = startIndex;
        animEndIndex = endIndex;
    }

    public TextAnimationInfo(int textStartIndex, int textEndIndex, string animationSeed) {
        animStartIndex = textStartIndex;
        animEndIndex = textEndIndex;
        var settingsChunks = animationSeed.Split('}'); //Split settings up to {NAME,X,Y,Z
        foreach (string chunk in settingsChunks) {
            var values = chunk.Replace("{", "").Split(',');
            if (!string.IsNullOrEmpty(values[0])) {
                switch (values[0]) {
                case "WAVE":
                    useBounceEffect = values[1] == "1";
                    bounceScale = float.Parse(values[2]);
                    bounceFrequency = float.Parse(values[3]);
                    break;
                case "SCALE":
                    useScaleEffect = values[1] == "1";
                    scaleRate = float.Parse(values[2]);
                    scaleMax = float.Parse(values[3]);
                    scaleMin = float.Parse(values[4]);
                    break;
                case "RAINBOW":
                    useRainbowEffect = values[1] == "1";
                    rainbowWidth = float.Parse(values[2]);
                    rainbowDirection = (RainbowDirection)Int32.Parse(values[3]);

                    Gradient gradient = new Gradient();
                    GradientColorKey[] colorKeys = new GradientColorKey[(values.Length - 4) / 2];
                    int colorKeyIndex = 0;
                    for (int i = 4; i < values.Length; i+=2) {
                        Color newColor = new Color();
                        if (ColorUtility.TryParseHtmlString("#" + values[i], out newColor)) {
                                colorKeys[colorKeyIndex].color = newColor;
                                colorKeys[colorKeyIndex].time = float.Parse(values[i + 1]);
                            } else {
                            Debug.Log("<color=cyan>Could not parse #" + values[i] +" to a color..</color>");
                        }
                        colorKeyIndex++;
                    }
                    
                    gradient.colorKeys = colorKeys;
                    rainbow = gradient;


                    break;
                case "ROTATE":
                    useRotateEffect = values[1] == "1";
                    pingPongRotation = values[2] == "1";
                    rotateAngle = float.Parse(values[3]);
                    rotateFrequency = float.Parse(values[4]);
                    rotationAxis = new Vector3(float.Parse(values[5]), float.Parse(values[6]), float.Parse(values[7]));
                    break;
                default:
                    Debug.Log("<color=cyan>TextAnimationInfo does not recognize [" + values[0] + "] as an animation state.</color>");
                    break;
            }
            }
            
        }
        
        
    }

    public string GetSettingsSeed() {
        string seed = "";

        //Wave
        string waveSettings = "{WAVE,";
        waveSettings += useBounceEffect ? "1," : "0,";
        waveSettings += bounceScale.ToString() + ",";
        waveSettings += bounceFrequency.ToString() + "}";
        seed += waveSettings;

        //Scale
        string scaleSettings = "{SCALE,";
        scaleSettings += useScaleEffect ? "1," : "0,";
        scaleSettings += scaleRate.ToString() + ",";
        scaleSettings += scaleMax.ToString() + ",";
        scaleSettings += scaleMin.ToString() + "}";
        seed += scaleSettings;

        //Rainbow
        string rainbowSettings = "{RAINBOW,";
        rainbowSettings += useRainbowEffect ? "1," : "0,";
        rainbowSettings += rainbowWidth.ToString() + ",";
        rainbowSettings += (int)rainbowDirection + ",";
        foreach (var colorKey in rainbow.colorKeys) {
            rainbowSettings += ColorUtility.ToHtmlStringRGB(colorKey.color) + ",";
            rainbowSettings += colorKey.time.ToString() + ",";
        }
        rainbowSettings = rainbowSettings.Remove(rainbowSettings.Length - 1, 1);
        rainbowSettings += "}";
        seed += rainbowSettings;

        //Rotate
        string rotateSettings = "{ROTATE,";
        rotateSettings += useRotateEffect ? "1," : "0,";
        rotateSettings += pingPongRotation ? "1," : "0,";
        rotateSettings += rotateAngle.ToString() + ",";
        rotateSettings += rotateFrequency.ToString() + ",";
        rotateSettings += rotationAxis.x + "," + rotationAxis.y + "," + rotationAxis.z + "}";
        seed += rotateSettings;
        
        return seed;
    }
}
