using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new level button data", menuName = "Scriptable objects/UI/Level button data")]
public class LevelButtonData : ScriptableObject
{
    public Sprite commonScrollViewButtonSprite;
    public Sprite chosenScrollViewButtonSprite;
    public Color commonScrollViewButtonTextColor;
    public Color chosenScrollViewButtonTextColor;
    public Sprite mainButtonSprite;
}
