using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour, IDamagable
{
    // Should be "converted" into being an Inherited class
    // Dynamic naming/tag assignment?
    private NavMeshAgent _agent;
    [SerializeField] private float _moveSpeed = 0;
    [SerializeField] private float _acceleration = 0;
    [SerializeField] private float _stoppingDistance = 0;

    [SerializeField] private GameObject _healthBar = null;
    
    private bool _lookForTargetActive = true;
    [SerializeField] private GameObject _shotPrefab = null;

    [SerializeField] private int _maxHealth = 100; 
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public string _material;

    private void Start()
    {
        MaxHealth = _maxHealth;
        CurrentHealth = MaxHealth;
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
        _healthBar.SetActive(CurrentHealth != MaxHealth);
        if (!_lookForTargetActive) return;
        if (LocateTarget())
        {
            StartCoroutine(BangBang());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Damage(10);
        }
    }

    public void Damage(int damageAmount)
    {
        if (CurrentHealth - damageAmount <= 0)
        {
            CurrentHealth = 0;
            Destroy(gameObject);
        }
        CurrentHealth -= damageAmount;
    }

    public void DisplayHealth(bool showHealthBar)
    {
        _healthBar.SetActive(showHealthBar);
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
        for (int i = 0; i <= 10; i++)
        {
            Instantiate(_shotPrefab, transform.localPosition + new Vector3(0,0, 0.75f), transform.localRotation);
            yield return new WaitForSeconds(0.5f);
        }
        
        _lookForTargetActive = false;
    }
}