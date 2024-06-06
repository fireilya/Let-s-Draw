using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMenu : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    private static float currentAudioSourceTime = 0;
    public void GoToMainMenu() { Time.timeScale = 1; SceneManager.LoadScene(0); }
    public void Restart() { drawController.Restart(); }
}
