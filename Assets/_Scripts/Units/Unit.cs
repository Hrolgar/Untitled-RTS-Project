using System;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private float _moveSpeed = 0;
    [SerializeField] private float _acceleration = 0;
    [SerializeField] private float _stoppingDistance = 0;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;
        _agent.acceleration = _acceleration;
        _agent.stoppingDistance = _stoppingDistance;
    }

    public void MoveSelectedUnit(Vector3 target)
    {
        _agent.SetDestination(target);

    }
}