using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private static Button currentDisableButton;

    void Start()
    {
        header.text = levelCards[0].header;
        description.text = levelCards[0].description;
        levelPreview.sprite = levelCards[0].levelPreview;
        buttons[0].interactable = false;
        currentDisableButton = buttons[0];
    }
    
    public void ChangeLevelCard(int levelIndex)
    {
        currentDisableButton.interactable = true;
        buttons[levelIndex].interactable = false;
        currentDisableButton = buttons[levelIndex];
        header.text = levelCards[levelIndex].header;
        description.text = levelCards[levelIndex].description;
        levelPreview.sprite = levelCards[levelIndex].levelPreview;
    }
}