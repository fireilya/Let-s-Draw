using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawController : MonoBehaviour
{
    public class DrawControllerState
    {
        public bool isCollisionEnabled;
        public bool isEraseEnabled;
        public float drawingTimeLimit;
        public float currentDrawingTime;
        public bool isDrawTimeLimitEnabled;
        public bool isMoveSignalEnabled;
        public bool isDrawingEnabled;
    };

    [SerializeField]
    private LineRenderer linePrefab;

    [SerializeField]
    private Color lineColor;

    [SerializeField]
    [Range(1e-5f, 1)]
    private float lineWidth;

    [SerializeField, Range(1e-5f, 5f)]
    private float lineSimplifying;

    [SerializeField]
    [Range(0f, 15f)]
    private float drawingTimeLimit;

    [SerializeField]
    private bool isLineDissapear;

    [SerializeField]
    private bool isLineWithColider;

    [SerializeField]
    private bool isDrawTimeLimit;

    [SerializeField]
    private bool isMoveSignalEnabled;

    [SerializeField]
    private Sprite lineTexture;

    [SerializeField]
    private GraphicRaycaster UIRaycaster;

    private LineRenderer currentLine;
    private float currentLineZIndex = 0;
    private EdgeCollider2D currentLineCollider;
    private List<Vector2> currentLinePositions = new List<Vector2>();
    private Camera mainCamera;

    public UnityEvent<List<Vector2>> OnLineFinished = new UnityEvent<List<Vector2>>();
    public DrawControllerState State { get; set; } = new DrawControllerState();

    void Awake()
    {
        State.isCollisionEnabled = isLineWithColider;
        State.isEraseEnabled = !isLineDissapear;
        State.drawingTimeLimit = drawingTimeLimit;
        State.currentDrawingTime = 0;
        State.isDrawTimeLimitEnabled = isDrawTimeLimit;
        State.isMoveSignalEnabled = isMoveSignalEnabled;
        State.isDrawingEnabled = true;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var graphicsRaycastResults = new List<RaycastResult>();
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (UIRaycaster != null)
        {
            var pointerEventData = new PointerEventData(FindObjectOfType<EventSystem>());
            pointerEventData.position = Input.mousePosition;
            UIRaycaster.Raycast(pointerEventData, graphicsRaycastResults);
        }

        if (Physics2D.Raycast(mouseWorldPosition, Vector2.zero) || graphicsRaycastResults.Count != 0)
        {
            //Debug.Log("here");
        }
        if (!State.isDrawingEnabled) { return; }
        if (Input.GetMouseButtonDown(0))        
            InitLine();       

        if(Input.GetMouseButton(0) && State.currentDrawingTime<=drawingTimeLimit)        
            Draw();

        if (Input.GetMouseButtonUp(0))
            EndDrawing();
    }

    public void clearAllLines()
    {
        foreach(var line in FindObjectsOfType<LineRenderer>())
        {
            Destroy(line.gameObject);
        }
    }

    public void UpdateLinesColliders()
    {
        foreach (var line in FindObjectsOfType<LineRenderer>())
        {
            line.GetComponent<EdgeCollider2D>().enabled = State.isCollisionEnabled;
        }
    }

    public void DeleteCurrentLine() { Destroy(currentLine.gameObject); }

    public List<Vector2> GetCurrentLine() { return currentLinePositions; }

    private void Draw() 
    {
        currentLine.positionCount++;
        var cursorPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentLine.SetPosition(currentLine.positionCount - 1, new Vector3(cursorPosition.x, cursorPosition.y, currentLineZIndex));
        currentLinePositions.Add(new Vector2(cursorPosition.x, cursorPosition.y));

        if (State.isDrawTimeLimitEnabled)
            State.currentDrawingTime += Time.deltaTime;
        
        if (State.isCollisionEnabled)
            currentLineCollider.SetPoints(currentLinePositions);
        
    }

    private void InitLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLine.widthCurve = new AnimationCurve(new Keyframe(0, lineWidth), new Keyframe(1, lineWidth));
        Material lineMaterial = currentLine.material;

        if (lineTexture == null)
            lineMaterial.color = lineColor;
        else
            lineMaterial.mainTexture = lineTexture.texture;

        currentLine.textureScale = new Vector2(1f / lineWidth, -1);
        currentLineZIndex -= 1e-3f;
        if (State.isCollisionEnabled)
        {
            currentLineCollider = currentLine.GetComponent<EdgeCollider2D>();
            currentLineCollider.enabled = true;
        }
        currentLinePositions.Clear();
    }

    private void EndDrawing()
    {
        State.currentDrawingTime = 0;
        if (currentLine == null) return;
        currentLine.Simplify(lineSimplifying);
        var newLinePositions = new Vector3[currentLine.positionCount];
        currentLine.GetPositions(newLinePositions);
        OnLineFinished.Invoke(newLinePositions.Select(x => new Vector2(x.x, x.y)).ToList());
        if (isLineDissapear)
            Destroy(currentLine.gameObject);
    }
}
