using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartToDraw : MonoBehaviour
{
    [SerializeField]
    private DrawController drawController;

    private PolygonCollider2D poligonCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        poligonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void VerifyStroke(List<Vector2> linePointPositiions)
    {

    }
}