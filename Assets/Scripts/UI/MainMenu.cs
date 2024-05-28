using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text header;

    [SerializeField] 
    private TMP_Text description;

    [SerializeField] 
    private Image levelPreview;

    [SerializeField]
    private List<LevelCard> levelCards;

    [SerializeField]
    private List<Button> buttons;

    private Button currentDisableButton;
    private int sceneIndexToLoad;

    void Start()
    {
        header.text = levelCards[0].header;
        description.text = levelCards[0].description;
        levelPreview.sprite = levelCards[0].levelPreview;
        sceneIndexToLoad = levelCards[0].levelSceneIndex;
        buttons[0].interactable = false;
        currentDisableButton = buttons[0];
        sceneIndexToLoad = levelCards[0].levelSceneIndex;
    }

    public void Quit() { Application.Quit(); }
    

    public void ChangeLevelCard(int levelIndex)
    {
        currentDisableButton.interactable = true;
        buttons[levelIndex].interactable = false;
        currentDisableButton = buttons[levelIndex];
        header.text = levelCards[levelIndex].header;
        description.text = levelCards[levelIndex].description;
        levelPreview.sprite = levelCards[levelIndex].levelPreview;
        sceneIndexToLoad = levelCards[levelIndex].levelSceneIndex;
    }

    public void LoadLevelScene()
    {
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
