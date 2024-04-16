using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartToDraw : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private float accuracyCoeff;

    private PolygonCollider2D poligonCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        poligonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        drawController.OnLineFinished.AddListener(VerifyStroke);
    }

    void VerifyStroke(List<Vector2> linePointPositions)
    {
        var deviation = poligonCollider.points
            .Select(coliderOutlinePoint => 
            linePointPositions
                .Select(linePoint => Vector2.Distance(transform.TransformPoint(coliderOutlinePoint), linePoint))
                .Min())
            .Average();

        if (deviation<accuracyCoeff)       
            spriteRenderer.enabled = true;
    }
}
