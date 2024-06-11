using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPartWinLink : MonoBehaviour
{
    [SerializeField]
    private LevelUI levelUI;

    [SerializeField]
    private SpriteRenderer drawnPartSpriteRenderer;
    public void Win()
    {
        drawnPartSpriteRenderer.enabled = true;
        levelUI.DelayShowResult(0.75f);
    }
}
