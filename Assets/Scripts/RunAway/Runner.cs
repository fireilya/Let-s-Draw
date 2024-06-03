using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Runner : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private LevelUI levelUI;

    [SerializeField, Range(0.2f, 0.6f)]
    private float accuracityCoeff = 0f;

    [SerializeField]
    private float moveVelocity;

    [SerializeField]
    private Transform finish;

    private List<Vector2> path = null;
    private int currentIndex = 0;
    private Vector2 currentTarget;
    private Vector2 currentPosition;

    public UnityEvent OnRunStarted = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish") { levelUI.ShowLevelResult(); return; }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Start()
    {
        drawController.OnLineFinished.AddListener(VerifyLine);
    }

    // Update is called once per frame
    void Update()
    {
        if (path != null) 
        {
            var nextJumpLength = moveVelocity * Time.deltaTime;
            while (Vector2.Distance(currentPosition, currentTarget) < nextJumpLength && currentIndex != path.Count-1)
            { 
                currentTarget = path[++currentIndex];
            }
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, moveVelocity * Time.deltaTime);
            currentPosition = transform.position;
        }
    }

    public void VerifyLine(List<Vector2> linePath)
    {
        if (Vector3.Distance(transform.position, linePath[0]) > accuracityCoeff ||
            Vector3.Distance(finish.position, linePath[^1]) > accuracityCoeff)
        {
            drawController.DeleteCurrentLine();
            return;
        }
        path = linePath.ToList();
        currentPosition = transform.position;
        currentTarget = linePath[0];
        drawController.State.isDrawingEnabled = false;
        OnRunStarted.Invoke();
    }
}
