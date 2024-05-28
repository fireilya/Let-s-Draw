using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void GoToMainMenu() { SceneManager.LoadScene(0); }
    public void Resume() { Time.timeScale = 1f; gameObject.SetActive(false); Debug.Log("sdf"); }
}
