using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMenu : MonoBehaviour
{
    public void GoToMainMenu() { SceneManager.LoadScene(0); }
    public void Restart() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
