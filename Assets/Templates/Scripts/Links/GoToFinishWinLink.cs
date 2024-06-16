using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToFinishWinLink : MonoBehaviour
{
    [SerializeField]
    private LevelUI levelUI;

    public void Win() { levelUI.ShowLevelResult(); }
}
