using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLearningWinLink : MonoBehaviour
{
    [SerializeField]
    private LevelUI levelUI;

    public void Win() { levelUI.DelayShowResult(0.75f); }
}
