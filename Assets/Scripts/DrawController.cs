using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pointerEventData = new PointerEventData(FindObjectOfType<EventSystem>());
        pointerEventData.position = Input.mousePosition;
        var graphicsRaycastResults = new List<RaycastResult>();
        UIRaycaster.Raycast(pointerEventData, graphicsRaycastResults);
        if (Physics2D.Raycast(mouseWorldPosition, Vector2.zero) || graphicsRaycastResults.Count != 0)
        {
            Debug.Log("here");
        }
        if (!State.isDrawingEnabled) { return; }
        if (Input.GetMouseButtonDown(0))        
            InitLine();       

        if(Input.GetMouseButton(0) && State.currentDrawingTime<=drawingTimeLimit)        
            Draw();

        if (Input.GetMouseButtonUp(0))
        {
            State.currentDrawingTime = 0;
            if (currentLine == null) return;
            OnLineFinished.Invoke(currentLinePositions);
            if(isLineDissapear)
                Destroy(currentLine.gameObject);
        }
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

    private void Draw() 
    {
        currentLine.positionCount++;
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
}
