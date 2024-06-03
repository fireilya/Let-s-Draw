using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartToDraw : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private LevelUI levelUI;

    [SerializeField]
    private float maxDeviation;

    [SerializeField]
    private GameObject drawnPart;

    private PolygonCollider2D poligonCollider;
    private SpriteRenderer drawnPartSpriteRenderer;

    void Start()
    {
        poligonCollider = GetComponent<PolygonCollider2D>();
        drawnPartSpriteRenderer = drawnPart.GetComponent<SpriteRenderer>();
        drawController.OnLineFinished.AddListener(VerifyStroke);
    }

    private void VerifyStroke(List<Vector2> linePointPositions)
    {
        var deviationByLine = linePointPositions
            .Select(linePoint =>
            poligonCollider.points
                .Select(colliderPoint => Vector2.Distance(transform.TransformPoint(colliderPoint), linePoint))
                .Min())
            .Average();

        var deviationByCollider = poligonCollider.points
            .Select(colliderPoint =>
             linePointPositions
                .Select(linePoint => Vector2.Distance(transform.TransformPoint(colliderPoint), linePoint))
                .Min())
            .Average();

        if ((deviationByLine + deviationByCollider) / 2f < maxDeviation) 
        { 
            drawnPartSpriteRenderer.enabled = true;
            levelUI.DelayShowResult(0.75f); 
        }
        
    }
}
