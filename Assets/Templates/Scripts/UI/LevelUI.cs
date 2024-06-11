using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private Button collisionButton;

    [SerializeField]
    private Button timeButton;

    [SerializeField]
    private Button eraseButton;

    [SerializeField]
    private Button moveSignalButton;

    [SerializeField]
    private Image timeIndicator;

    [SerializeField]
    private GameObject TimeSection;

    [SerializeField]
    private PauseMenu pauseMenu;

    [SerializeField]
    private ResultMenu resultMenu;

    [SerializeField]
    private Color comonColor;

    [SerializeField]
    private Color enabledColor;

    void Start()
    {
        if (timeIndicator != null) timeIndicator.fillAmount = 1;

        if (TimeSection != null) TimeSection.SetActive(drawController.State.isDrawTimeLimitEnabled);

        if (collisionButton != null) 
            collisionButton.image.color = drawController.State.isCollisionEnabled || !collisionButton.interactable ? enabledColor : comonColor;

        if (timeButton != null) 
            timeButton.image.color = drawController.State.isDrawTimeLimitEnabled || !timeButton.interactable ? enabledColor : comonColor;

        if (eraseButton != null) 
            eraseButton.image.color = drawController.State.isEraseEnabled || !eraseButton.interactable ? enabledColor : comonColor;

        if (moveSignalButton != null)
            moveSignalButton.image.color = drawController.State.isMoveSignalEnabled || !moveSignalButton.interactable ? enabledColor : comonColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawController.State.isDrawTimeLimitEnabled)
        {
            var currentDrawingTime = drawController.State.currentDrawingTime;
            var drawingTimeLimit = drawController.State.drawingTimeLimit;
            if (timeIndicator != null) timeIndicator.fillAmount = (drawingTimeLimit - currentDrawingTime) / drawingTimeLimit;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void ShowLevelResult()
    {
        Time.timeScale = 0;
        drawController.State.isDrawingEnabled = false;
        resultMenu.gameObject.SetActive(true);
    }

    public void ToggleCollision()
    {
        drawController.State.isCollisionEnabled = !drawController.State.isCollisionEnabled;
        collisionButton.image.color = drawController.State.isCollisionEnabled ? enabledColor : comonColor;
        drawController.UpdateLinesColliders();
    }

    public void ToggleTimeLimit()
    {
        drawController.State.isDrawTimeLimitEnabled = !drawController.State.isDrawTimeLimitEnabled;
        timeButton.image.color = drawController.State.isDrawTimeLimitEnabled ? enabledColor : comonColor;
        TimeSection.SetActive(drawController.State.isDrawTimeLimitEnabled);
    }

    public void ToggleErase()
    {
        drawController.State.isEraseEnabled = !drawController.State.isEraseEnabled;
        eraseButton.image.color = drawController.State.isEraseEnabled ? enabledColor : comonColor;
    }

    public void ToggleMoveSignal()
    {
        drawController.State.isMoveSignalEnabled = !drawController.State.isMoveSignalEnabled;
        moveSignalButton.image.color = drawController.State.isMoveSignalEnabled ? enabledColor : comonColor;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        drawController.State.isDrawingEnabled = false;
        pauseMenu.gameObject.SetActive(true);
    }

    public void DelayShowResult(float delay) { StartCoroutine(DelayShowResultCoroutine(delay)); }

    private IEnumerator DelayShowResultCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowLevelResult();
    }
}
