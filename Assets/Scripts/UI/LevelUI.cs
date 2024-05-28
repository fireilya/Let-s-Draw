using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private Button timeButton;

    [SerializeField]
    private Button collisionButton;

    [SerializeField]
    private Button eraseButton;

    [SerializeField]
    private Button punchButton;

    [SerializeField]
    private Image timeIndicator;

    [SerializeField]
    private bool isPunchEnabled;

    [SerializeField]
    private GameObject TimeSection;

    [SerializeField]
    private PauseMenu pauseMenu;

    [SerializeField]
    private ResultMenu resultMenu;

    void Start()
    {
        timeButton.interactable = drawController.State.isDrawTimeLimitEnabled;
        TimeSection.SetActive(drawController.State.isDrawTimeLimitEnabled);
        collisionButton.interactable = drawController.State.isCollisionEnabled;
        eraseButton.interactable = drawController.State.isEraseEnabled;
        punchButton.interactable = isPunchEnabled;
        timeIndicator.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawController.State.isDrawTimeLimitEnabled)
        {
            var currentDrawingTime = drawController.State.currentDrawingTime;
            var drawingTimeLimit = drawController.State.drawingTimeLimit;
            timeIndicator.fillAmount = (drawingTimeLimit - currentDrawingTime) / drawingTimeLimit;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            drawController.IsDrawingEnabled = false;
            pauseMenu.gameObject.SetActive(true);
        }
    }

    public void ShowLevelResult()
    {
        Time.timeScale = 0;
        drawController.IsDrawingEnabled = false;
        resultMenu.gameObject.SetActive(true);
    }
}
