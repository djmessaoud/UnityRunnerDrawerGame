using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Dreamteck.Splines;

public class DrawingManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public LineRenderer lineRenderer;
    private List<Vector3> points;
    public SplineComputer dynamicSplineComputer;
    public Camera uiCamera;
    private RunnerSpawner runnerSpawner;
    private RectTransform _thisRectTransform;

    void Start()
    {
        points = new List<Vector3>();
        runnerSpawner = FindObjectOfType<RunnerSpawner>();
        _thisRectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        points.Clear();
        AddPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsPointerWithinPanel(eventData.position))
        {
            AddPoint(eventData.position);
            CreateDynamicSpline();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsPointerWithinPanel(eventData.position))
        {
            AddPoint(eventData.position);
        }
        else
        {
            CreateDynamicSpline();
        }
    }

    private void AddPoint(Vector2 position)
    {
        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(), position, uiCamera, out worldPosition);
        points.Add(worldPosition);
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void CreateDynamicSpline()
    {
        if (dynamicSplineComputer == null) return;

        int numberOfRunners = runnerSpawner.GetRunnerCount();
        if (numberOfRunners == 0) return;
        Vector3[] xPoints = new Vector3[points.Count];
        xPoints = points.ToArray();
        List<SplinePoint> splinePoints = new List<SplinePoint>();
        for (int i = 0; i < xPoints.Length; i++)
        {
            xPoints[i].z += 95;
            xPoints[i].y = 0;
            splinePoints.Add(new SplinePoint(xPoints[i]));
        }
        dynamicSplineComputer.SetPoints(splinePoints.ToArray());
        dynamicSplineComputer.type = Spline.Type.Bezier;
        
        dynamicSplineComputer.RebuildImmediate();

        // Update all runners to follow the new dynamic spline
        runnerSpawner.UpdateRunnersSpline(dynamicSplineComputer, 0.4f);
    }
    private bool IsPointerWithinPanel(Vector2 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_thisRectTransform, position, uiCamera);
    }
}
