using System;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void MoveSelectedUnit(Vector3 target)
    {
        _agent.SetDestination(target);
    }
}