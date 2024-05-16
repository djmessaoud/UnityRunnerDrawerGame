using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private Runner _runner;
    // Start is called before the first frame update
    void Start()
    {
        _runner = GetComponentInParent<Runner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected");
        _runner.HandleCollision(other);
    }
}
