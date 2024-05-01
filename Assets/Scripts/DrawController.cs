using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrawController : MonoBehaviour
{
    public class DrawControllerState
    {
        public bool isCollisionEnabled;
        public bool isEraseEnabled;
        public float drawingTimeLimit;
        public float currentDrawingTime;
    };

    [SerializeField]
    private LineRenderer linePrefab;

    [SerializeField]
    private Color lineColor;

    [SerializeField]
    [Range(1e-5f, 1)]
    private float lineWidth;

    [SerializeField]
    [Range(0f, float.MaxValue)]
    private float drawingTimeLimit;

    [SerializeField]
    private bool isLineDissapear;

    [SerializeField]
    private bool isLineWithColider;

    [SerializeField]
    private Sprite lineTexture;

    private LineRenderer currentLine;
    private float currentLineZIndex = 0;
    private EdgeCollider2D currentLineCollider;
    private List<Vector2> currentLinePositions = new List<Vector2>();

    public UnityEvent<List<Vector2>> OnLineFinished = new UnityEvent<List<Vector2>>();
    public DrawControllerState State { get; private set; }

    void Start()
    {
        State.isCollisionEnabled = isLineWithColider;
        State.isEraseEnabled = !isLineDissapear;
        State.drawingTimeLimit = drawingTimeLimit;
        State.currentDrawingTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))        
            InitLine();       

        if(Input.GetMouseButton(0) && State.currentDrawingTime<=drawingTimeLimit)        
            Draw();

        if (Input.GetKeyDown(KeyCode.C))       
            clearAllLines();

        if (Input.GetMouseButtonUp(0))
        {
            OnLineFinished.Invoke(currentLinePositions);
            if(isLineDissapear)
                Destroy(currentLine.gameObject);
        }
    }

    private void clearAllLines()
    {
        foreach(var line in FindObjectsOfType<LineRenderer>())
        {
            Destroy(line.gameObject);
        }
    }

    private void Draw() 
    {
        currentLine.positionCount++;
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLine.SetPosition(currentLine.positionCount - 1, new Vector3(cursorPosition.x, cursorPosition.y, currentLineZIndex));
        currentLinePositions.Add(new Vector2(cursorPosition.x, cursorPosition.y));
        State.currentDrawingTime += Time.deltaTime;
        if (isLineWithColider)
        {
            currentLineCollider.SetPoints(currentLinePositions);
        }
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
        State.currentDrawingTime = 0;
        if (isLineWithColider)
        {
            currentLineCollider = currentLine.GetComponent<EdgeCollider2D>();
            currentLineCollider.enabled = true;
        }
        currentLinePositions.Clear();
    }
}
