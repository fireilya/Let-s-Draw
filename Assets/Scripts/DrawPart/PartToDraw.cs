using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartToDraw : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private float maxDeviation;

    private PolygonCollider2D poligonCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        poligonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        drawController.OnLineFinished.AddListener(VerifyStroke);
    }

    private void VerifyStroke(List<Vector2> linePointPositions)
    {
        var deviation = linePointPositions
            .Select(linePoint =>
            poligonCollider.points
                .Select(coliderPoint => Vector2.Distance(transform.TransformPoint(coliderPoint), linePoint))
                .Min())
            .Average();

        if (deviation<maxDeviation)       
            spriteRenderer.enabled = true;
    }
}
