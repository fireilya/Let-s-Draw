using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PartToDraw : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private float maxDeviation;

    private PolygonCollider2D poligonCollider;

    public UnityEvent OnPartDrawed = new UnityEvent();

    void Start()
    {
        poligonCollider = GetComponent<PolygonCollider2D>();
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
            OnPartDrawed.Invoke();
    }
}
