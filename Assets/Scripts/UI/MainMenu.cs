using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text mainHeader;

    [SerializeField] 
    private TMP_Text mainDescription;

    [SerializeField] 
    private Image levelPreview;

    [SerializeField]
    private List<LevelCard> levelCards;

    [SerializeField]
    private List<Button> scrollMenuButtons;

    [SerializeField]
    private List<Button> mainButtons;

    [SerializeField]
    private List<TMP_Text> scrollMenuHeaders;

    [SerializeField]
    private List<TMP_Text> scrollMenuDescriptions;

    private Sprite commonScrollViewButtonSprite;
    private Sprite chosenScrollViewButtonSprite;
    private Color commonScrollViewButtonTextColor;
    private Color chosenScrollViewButtonTextColor;
    private List<TMP_Text> scrollMenuButtonsText;
    private List<TMP_Text> mainButtonsText;
    private Button currentDisableButton;
    private TMP_Text currentDisableButtonText;
    private int sceneIndexToLoad;

    void Start()
    {
        scrollMenuButtonsText = scrollMenuButtons.Select(x=>x.GetComponentInChildren<TMP_Text>()).ToList();
        mainButtonsText = mainButtons.Select(x=>x.GetComponentInChildren<TMP_Text>()).ToList();
        ChangeLevelCard(0);
    }

    public void Quit() { Application.Quit(); }
    
    public void ChangeLevelCard(int levelIndex)
    {
        UpdateButtons(levelIndex);

        if (currentDisableButton != null)
        {
            currentDisableButton.interactable = true;
            currentDisableButton.image.sprite = commonScrollViewButtonSprite;
            currentDisableButtonText.color = commonScrollViewButtonTextColor;
        }

        scrollMenuButtons[levelIndex].interactable = false;
        scrollMenuButtons[levelIndex].image.sprite = chosenScrollViewButtonSprite;
        scrollMenuButtonsText[levelIndex].color = chosenScrollViewButtonTextColor;

        currentDisableButton = scrollMenuButtons[levelIndex];
        currentDisableButtonText= scrollMenuButtonsText[levelIndex];

        mainHeader.text = levelCards[levelIndex].header;
        mainDescription.text = levelCards[levelIndex].description;
        levelPreview.sprite = levelCards[levelIndex].levelPreview;
        sceneIndexToLoad = levelCards[levelIndex].levelSceneIndex;
        UpdateLevelFont(levelIndex);
    }

    public void LoadLevelScene()
    {
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    private void UpdateLevelFont(int levelIndex)
    {
        var levelFontData = levelCards[levelIndex].levelFontData;
        mainHeader.font = levelFontData.mainHeaderFont;
        mainHeader.fontSize = levelFontData.mainHeaderFontSize;
        mainHeader.lineSpacing = levelFontData.mainHeaderFontLineSpacing;

        mainDescription.font = levelFontData.mainDescriptionFont;
        mainDescription.fontSize = levelFontData.mainDescriptionFontSize;
        mainDescription.lineSpacing = levelFontData.mainDescriptionFontLineSpacing;

        foreach (var text in scrollMenuHeaders)
        {
            text.font = levelFontData.scrollMenuHeadersFont;
            text.fontSize = levelFontData.scrollMenuHeadersFontSize;
        }

        foreach (var text in scrollMenuDescriptions)
        {
            text.font = levelFontData.scrollMenuDescriptionFont;
            text.fontSize = levelFontData.scrollMenuDescriptionFontSize;
        }

        foreach (var text in scrollMenuButtonsText)
        {
            text.font = levelFontData.scrollMenuButtonsFont;
            text.fontSize = levelFontData.scrollMenuButtonsFontSize;
        }

        foreach (var text in mainButtonsText)
        {
            text.font = levelFontData.mainButtonsFont;
            text.fontSize = levelFontData.mainButtonsFontSize;
        }
    }

    private void UpdateButtons(int levelIndex)
    {
        commonScrollViewButtonSprite = levelCards[levelIndex].levelButtonData.commonScrollViewButtonSprite;
        chosenScrollViewButtonSprite = levelCards[levelIndex].levelButtonData.chosenScrollViewButtonSprite;
        commonScrollViewButtonTextColor = levelCards[levelIndex ].levelButtonData.commonScrollViewButtonTextColor;
        chosenScrollViewButtonTextColor = levelCards[levelIndex].levelButtonData.chosenScrollViewButtonTextColor;

        foreach (var button in scrollMenuButtons)
            button.image.sprite = button.interactable ? commonScrollViewButtonSprite : chosenScrollViewButtonSprite;

        foreach (var button in mainButtons)
            button.image.sprite = levelCards[levelIndex].levelButtonData.mainButtonSprite;
    }
}
