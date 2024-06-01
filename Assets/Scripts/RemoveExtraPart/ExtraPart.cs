using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

class BoundingBox
{
    public BoundingBox(Vector2 min, Vector2 max)
    {
        MinPoint = min; MaxPoint = max;
    }

    public BoundingBox()
    {
        MinPoint = new Vector2(float.MaxValue, float.MaxValue);
        MaxPoint = new Vector2(float.MinValue, float.MinValue);
    }

    public Vector2 MinPoint { get; set; }
    public Vector2 MaxPoint { get; set; }

    public float Area 
    { 
        get { return (MaxPoint.x - MinPoint.x) * (MaxPoint.y - MinPoint.y); } 
    }

    public BoundingBox FindIntersection(BoundingBox other)
    {
        var maxX = MaxPoint.x < other.MaxPoint.x ? MaxPoint.x : other.MaxPoint.x;
        var maxY = MaxPoint.y < other.MaxPoint.y ? MaxPoint.y : other.MaxPoint.y;
        var minX = MinPoint.x > other.MinPoint.x ? MinPoint.x : other.MinPoint.x; ;
        var minY = MinPoint.y > other.MinPoint.y ? MinPoint.y : other.MinPoint.y; ;

        return new BoundingBox(new Vector2(maxX, maxY), new Vector2(minX, minY));
    }
}

public class ExtraPart : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    [SerializeField, Range(0.0f, 1.0f)]
    private float linePointsInsideColiderRatioTreshold;

    [SerializeField, Range(0.0f, 1.0f)]
    private float lineCoverColiderRatioTreshold;

    private PolygonCollider2D polygonCollider;
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        drawController.OnLineFinished.AddListener(VerifyLineFilling);
    }

    void VerifyLineFilling(List<Vector2> linePoints)
    {
        var linePointsInsideColliderCount = 0;

        var maxX = float.MinValue;
        var maxY = maxX;
        var minX = float.MaxValue;
        var minY = minX;

        foreach (var point in linePoints)
        {
            if (polygonCollider.OverlapPoint(point))            
                linePointsInsideColliderCount++;

            maxX = point.x > maxX ? point.x : maxX;
            maxY = point.y > maxY ? point.y : maxY;
            minX = point.x < minX ? point.x : minX;
            minY = point.y < minY ? point.y : minY;
        }
        var lineBoundingBox = new BoundingBox(new Vector2(minX, minY), new Vector2(maxX, maxY));
        var colliderBoundingBox = new BoundingBox(polygonCollider.bounds.min, polygonCollider.bounds.max);
        var intersectionBoundingBox = colliderBoundingBox.FindIntersection(lineBoundingBox);

        if (intersectionBoundingBox.Area/colliderBoundingBox.Area > lineCoverColiderRatioTreshold &&
            linePointsInsideColliderCount/(float)linePoints.Count > linePointsInsideColiderRatioTreshold)
        { Destroy(gameObject); }
    }
}
