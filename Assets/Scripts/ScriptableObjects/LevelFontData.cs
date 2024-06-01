using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "new level font data", menuName = "Scriptable objects/UI/Level font data")]
public class LevelFontData : ScriptableObject
{
    public TMP_FontAsset mainHeaderFont;
    public float mainHeaderFontSize;
    public float mainHeaderFontLineSpacing;
    public TMP_FontAsset mainDescriptionFont;
    public float mainDescriptionFontSize;
    public float mainDescriptionFontLineSpacing;
    public TMP_FontAsset mainButtonsFont;
    public float mainButtonsFontSize;
    public TMP_FontAsset scrollMenuHeadersFont;
    public float scrollMenuHeadersFontSize;
    public TMP_FontAsset scrollMenuDescriptionFont;
    public float scrollMenuDescriptionFontSize;
    public TMP_FontAsset scrollMenuButtonsFont;
    public float scrollMenuButtonsFontSize;
}
