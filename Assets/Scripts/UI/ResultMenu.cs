using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMenu : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    [SerializeField]
    private TMP_Text levelNameText;

    void Start()
    {
        levelNameText.text = levelName; 
    }
    public void GoToMainMenu() { Time.timeScale = 1; SceneManager.LoadScene(0); }
    public void Restart() { Time.timeScale = 1; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
