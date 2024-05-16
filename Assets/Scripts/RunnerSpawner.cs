using UnityEngine;
using System.Collections.Generic;
using Dreamteck.Splines;

public class RunnerSpawner : MonoBehaviour
{
    public GameObject runnerPrefab;
    public SplineComputer staticSplineComputer;
    public SplineComputer dynamicSplineComputer;
    public List<GameObject> runners;
    public Camera mainCamera;
    private int _initialRunners = 20;
    public GameManager gameManager;
    void Start()
    {
        runners = new List<GameObject>();
        SpawnInitialRunners(); // Spawn runners initially
    }

    public void SpawnInitialRunners()
    {
        for (int i=0; i<_initialRunners;i++)
        { 
            Vector3 spawnPosition = dynamicSplineComputer.EvaluatePosition(i);
            GameObject runner = Instantiate(runnerPrefab, spawnPosition, Quaternion.identity);
            runner.transform.parent = dynamicSplineComputer.transform;
          //  runner.GetComponent<SplineFollower>().spline = staticSplineComputer;
            runners.Add(runner);
        }
        UpdateCameraTarget();
    }

    public void SpawnRunner()
    {
        int randomIndex = Random.Range(0, dynamicSplineComputer.pointCount);
        Vector3 randomPointPosition = dynamicSplineComputer.GetPoint(randomIndex).position;
        Vector3 newPointPosition = randomPointPosition + new Vector3(-20, 0, 0);

        SplinePoint newSplinePoint = new SplinePoint(newPointPosition);
        List<SplinePoint> points = new List<SplinePoint>(dynamicSplineComputer.GetPoints());
        points.Insert(randomIndex, newSplinePoint);
        dynamicSplineComputer.SetPoints(points.ToArray());

        GameObject runner = Instantiate(runnerPrefab, newPointPosition, Quaternion.identity);
        runner.transform.parent = dynamicSplineComputer.transform;
        runners.Add(runner);
       // UpdateRunnersSpline(dynamicSplineComputer, 0.4f);
        UpdateCameraTarget();
    }

    public void RemoveRunner(GameObject runner)
    {
        runners.Remove(runner);
        if (runners.Count == 0)
        {
            gameManager.LoseGame();
        }
        UpdateCameraTarget();
    }

    public int GetRunnerCount()
    {
        return runners.Count;
    }

    public void UpdateRunnersSpline(SplineComputer newSpline, float duration)
    {
 
        for (int i=0; i<runners.Count; i++)
        {
            Vector3 targetPosition = newSpline.EvaluatePosition((double)i / (runners.Count - 1));

            runners[i].GetComponent<Runner>().MoveToNewPosition(targetPosition, duration);
        }
    }

    private void UpdateCameraTarget()
    {
        //if (mainCamera != null && runners.Count > 0)
        //{
        //    mainCamera.GetComponent<CameraFollow>().SetTarget(runners[0].transform);
        //}
    }
}
