using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new level card", menuName = "Scriptable objects/UI/Level card")]
public class LevelCard : ScriptableObject
{
    [TextArea(1, 20)]
    public string header;
    [TextArea(5, 20)]
    public string description;
    public Sprite levelPreview;

}
