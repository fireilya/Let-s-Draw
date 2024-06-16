using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteExcessWinLink : MonoBehaviour
{
    [SerializeField]
    private LevelUI levelUI;

    public void Win() { levelUI.DelayShowResult(0.75f); }
}
