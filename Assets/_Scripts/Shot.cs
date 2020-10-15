using System;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private int _damage = 10;
    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        var hit = other.gameObject.GetComponent<IDamagable>();
        if (hit != null)
        {
            hit.Damage(_damage);
            Destroy(gameObject);
        }
    }
}
