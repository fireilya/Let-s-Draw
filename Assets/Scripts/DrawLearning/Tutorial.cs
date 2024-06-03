using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField, Range(0, 2)]
    private float maxDeviation;

    [SerializeField, Range(0, 20)]
    private int markerSteps;

    private PolygonCollider2D[] colliders;
    private SpriteRenderer[] spriteRenderers;
    private int currentDrawObjectIndex = 0;
    private int currentPointIndex = 0;
    private GameObject currentMarker;
    private bool cycleMarkerFlag = false;
    private bool isLineFinished = false;

    void Start()
    {
        drawController.OnLineFinished.AddListener(FinishLine);
        colliders = partsToDraw.Select(x => { x.gameObject.SetActive(false); return x.GetComponent<PolygonCollider2D>(); }).ToArray();
        spriteRenderers = partsToDraw.Select(x => x.GetComponent<SpriteRenderer>()).ToArray();
        partsToDraw[currentDrawObjectIndex].gameObject.SetActive(true);
        currentMarker = 
            Instantiate(
                marker.gameObject,
                partsToDraw[currentDrawObjectIndex].transform.TransformPoint(colliders[currentDrawObjectIndex].points[currentPointIndex]), 
                Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        var currentLine = drawController.GetCurrentLine();
        if (currentLine == null || currentLine.Count == 0) return;
        if (currentMarker != null && Vector2.Distance(currentLine[^1], currentMarker.transform.position) < maxDeviation)
        {
            Destroy(currentMarker.gameObject);
            if (!cycleMarkerFlag) MakeNewMarker();
        }

        if (isLineFinished)
        {
            isLineFinished = cycleMarkerFlag = false;
            if (currentDrawObjectIndex == partsToDraw.Length - 1) { levelUI.DelayShowResult(0.75f); return; }
            EnableNextObject();
        }
    }

    private void MakeNewMarker()
    {
        var currentDrawObjectColliderPoints = colliders[currentDrawObjectIndex].points;
        cycleMarkerFlag = currentPointIndex + markerSteps > currentDrawObjectColliderPoints.Length - 1;
        currentPointIndex = cycleMarkerFlag ? 0 : currentPointIndex + markerSteps;
        currentMarker =
            Instantiate(
                marker.gameObject,
                partsToDraw[currentDrawObjectIndex].transform.TransformPoint(currentDrawObjectColliderPoints[currentPointIndex]),
                Quaternion.identity);
    }

    private void EnableNextObject()
    {
        ++currentDrawObjectIndex;
        currentPointIndex = 0;
        partsToDraw[currentDrawObjectIndex].gameObject.SetActive(true);
        currentMarker =
            Instantiate(
                marker.gameObject,
                partsToDraw[currentDrawObjectIndex].transform.TransformPoint(colliders[currentDrawObjectIndex].points[currentPointIndex]),
                Quaternion.identity);
    }

    private void FinishLine(List<Vector2> linePoints) 
    {
        isLineFinished = currentMarker == null ? VerifyStroke(linePoints) : false;
        if (currentMarker != null) Destroy(currentMarker.gameObject);
        if (!isLineFinished)
        {
            currentPointIndex = 0;
            currentMarker =
                Instantiate(
                marker.gameObject,
                partsToDraw[currentDrawObjectIndex].transform.TransformPoint(colliders[currentDrawObjectIndex].points[currentPointIndex]),
                Quaternion.identity);
            cycleMarkerFlag = false;
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
}
