using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Runner runner;
    private NavMeshAgent agent;
    private bool isRunStarted;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(transform.position);
        runner.OnRunStarted.AddListener(() => isRunStarted = true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunStarted) { agent.SetDestination(runner.gameObject.transform.position); }
    }
}
