using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;
    void Update() { if (Input.GetKeyDown(KeyCode.Escape)) Resume(); }
    public void GoToMainMenu() { Time.timeScale = 1; SceneManager.LoadScene(0); }
    public void Resume() { Time.timeScale = 1; drawController.State.isDrawingEnabled = true; gameObject.SetActive(false); }
}
