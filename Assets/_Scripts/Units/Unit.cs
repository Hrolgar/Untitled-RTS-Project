using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IDamagable
{
    private NavMeshAgent _agent;
    [SerializeField] private float _moveSpeed = 0;
    [SerializeField] private float _acceleration = 0;
    [SerializeField] private float _stoppingDistance = 0;

    private bool _lookForTargetActive = true;
    [SerializeField] private GameObject _shotPrefab = null;

    public int Health { get; set; } = 100;

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

    private void Update()
    {
        if (!_lookForTargetActive) return;
        if (LocateTarget())
        {
            StartCoroutine(BangBang());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Damage();
        }
    }

    public void Damage()
    {
        Health -= 10;
    }

    private bool LocateTarget()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.magenta);

        if (Physics.Raycast(ray, out var hit,  5f, ~1<<8))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator BangBang()
    {
        _lookForTargetActive = false;
        for (int i = 0; i < 10; i++)
        {
            Instantiate(_shotPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}