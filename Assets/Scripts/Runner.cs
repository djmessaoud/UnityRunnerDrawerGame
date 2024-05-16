using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Runner : MonoBehaviour
{
    private RunnerSpawner spawner;
    private bool isMovingToNewPosition = false;
    public Transform target;
    public int _digit;
    void Start()
    {
        spawner = FindObjectOfType<RunnerSpawner>();
    }

    public void HandleCollision(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Die();
        }
        else if (other.CompareTag("Spirit"))
        {
            spawner.gameManager.addScore(19);
            CollectSpirit(other.gameObject);
        }
    }

    private void Die()
    {
        spawner.gameManager.addScore(-4);
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        Debug.Log("Runner is dying");
        yield return new WaitForSeconds(0.2f);

        spawner.RemoveRunner(gameObject);
        Destroy(gameObject);

        if (spawner.GetRunnerCount() == 0)
        {
            Debug.Log("Game Over");  // Game over logic
        }
        Debug.Log("Runner died");
    }

    private void CollectSpirit(GameObject spirit)
    {
        Debug.Log("Collecting Spirit");
        spawner.SpawnRunner();
        Destroy(spirit);
    }
    public void MoveToNewPosition(Vector3 targetPosition, float duration)
    {
        if (!isMovingToNewPosition)
        {
            StartCoroutine(MoveToPosition(targetPosition, duration));
        }
    }
    void LateUpdate()
    {
            Vector3 desiredPosition = spawner.dynamicSplineComputer.EvaluatePosition(_digit);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0f);
            transform.position = smoothedPosition;
    }
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        isMovingToNewPosition = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        transform.position = targetPosition;
        isMovingToNewPosition = false;
    }
}
