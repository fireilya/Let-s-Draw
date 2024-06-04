using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject[] partsToDraw;

    [SerializeField]
    private DrawController drawController;

    [SerializeField]
    private LevelUI levelUI;

    [SerializeField]
    private GameObject marker;

    [SerializeField, Range(0, 1f)]
    private float firstMarkerOpacity;

    [SerializeField, Range(0, 2)]
    private float maxDeviation;

    [SerializeField]
    private int maxShownMarkersCount;

    private SpriteRenderer[] shownMarkersSpriteRenderers;
    private PolygonCollider2D[] colliders;
    private SpriteRenderer[] spriteRenderers;
    private int currentDrawObjectIndex = 0;
    private int nextPointIndex = 0;
    private bool isLineFinished = false;
    private float markerOpacityStep;

    void Start()
    {
        shownMarkersSpriteRenderers = new SpriteRenderer[maxShownMarkersCount];
        drawController.OnLineFinished.AddListener(FinishLine);
        colliders = partsToDraw.Select(x => { x.gameObject.SetActive(false); return x.GetComponent<PolygonCollider2D>(); }).ToArray();
        spriteRenderers = partsToDraw.Select(x => x.GetComponent<SpriteRenderer>()).ToArray();
        partsToDraw[currentDrawObjectIndex].gameObject.SetActive(true);
        markerOpacityStep = firstMarkerOpacity / maxShownMarkersCount;
        InitMarkersArray();  
    }

    // Update is called once per frame
    void Update()
    {
        var currentLine = drawController.GetCurrentLine();
        if (currentLine == null || currentLine.Count == 0) return;
        if (shownMarkersSpriteRenderers[nextPointIndex % maxShownMarkersCount] != null && 
            Vector2.Distance(
                currentLine[^1],
                shownMarkersSpriteRenderers[nextPointIndex % maxShownMarkersCount]
                    .gameObject
                    .transform
                    .position) < maxDeviation)
        {
            Destroy(shownMarkersSpriteRenderers[nextPointIndex % maxShownMarkersCount].gameObject);
            UpdateMarkersPath();
        }

        if (isLineFinished)
        {
            isLineFinished = false;
            if (currentDrawObjectIndex == partsToDraw.Length - 1) { levelUI.DelayShowResult(0.75f); return; }
            EnableNextObject();
        }
    }

    private void UpdateMarkersPath()
    {
        var currentDrawObjectColliderPoints = colliders[currentDrawObjectIndex].points;
        if (nextPointIndex < currentDrawObjectColliderPoints.Length)
        {
            shownMarkersSpriteRenderers[nextPointIndex % maxShownMarkersCount] =
                Instantiate(
                    marker.gameObject,
                    partsToDraw[currentDrawObjectIndex].transform.TransformPoint(currentDrawObjectColliderPoints[nextPointIndex]),
                    Quaternion.identity).GetComponent<SpriteRenderer>();

            var spriteColor = shownMarkersSpriteRenderers[nextPointIndex % maxShownMarkersCount].color;
            spriteColor.a = 0;
            shownMarkersSpriteRenderers[nextPointIndex % maxShownMarkersCount].color = spriteColor;
        }
        UpdateMarkersColor();
        ++nextPointIndex;
    }

    private void EnableNextObject()
    {
        ++currentDrawObjectIndex;
        nextPointIndex = 0;
        partsToDraw[currentDrawObjectIndex].gameObject.SetActive(true);
        ClearMarkersArray();
        InitMarkersArray();
    }

    private void FinishLine(List<Vector2> linePoints) 
    {
        isLineFinished = shownMarkersSpriteRenderers[nextPointIndex % maxShownMarkersCount] == null &&
            VerifyStroke(linePoints);
        if (!isLineFinished)
        {
            nextPointIndex = 0;
            ClearMarkersArray();
            InitMarkersArray();
        }
        else spriteRenderers[currentDrawObjectIndex].enabled = true;
    }

    private bool VerifyStroke(List<Vector2> linePointPositions)
    {
        var deviation = linePointPositions
            .Select(linePoint =>
            colliders[currentDrawObjectIndex].points
                .Select(coliderPoint => Vector2.Distance(partsToDraw[currentDrawObjectIndex].transform.TransformPoint(coliderPoint), linePoint))
                .Min())
            .Average();

        return deviation < maxDeviation;
    }

    private void InitMarkersArray()
    {
        for (int i = 0; i < maxShownMarkersCount && i < colliders[currentDrawObjectIndex].points.Length; i++)
        {
            shownMarkersSpriteRenderers[i] = Instantiate(
                marker.gameObject,
                partsToDraw[currentDrawObjectIndex].transform.TransformPoint(colliders[currentDrawObjectIndex].points[nextPointIndex++]),
                Quaternion.identity).GetComponent<SpriteRenderer>();

            var spriteColor = shownMarkersSpriteRenderers[i].color;
            spriteColor.a = firstMarkerOpacity - markerOpacityStep * i;
            shownMarkersSpriteRenderers[i].color = spriteColor;
        }
    }

    private void ClearMarkersArray()
    {
        foreach (var spriteRenderer in shownMarkersSpriteRenderers)
        {
            if (spriteRenderer != null) Destroy(spriteRenderer.gameObject);
        }
    }

    private void UpdateMarkersColor()
    {
        for (int i = 0; i < maxShownMarkersCount; i++)
        {
            if (shownMarkersSpriteRenderers[i] != null)
            {
                var spriteColor = shownMarkersSpriteRenderers[i].color;
                spriteColor.a = spriteColor.a + markerOpacityStep;
                shownMarkersSpriteRenderers[i].color = spriteColor;
            }
        }
    }


}
