using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        Destroy(this.gameObject, 3f);
    }
}
