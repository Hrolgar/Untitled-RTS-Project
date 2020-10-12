using UnityEngine;

public class selection_component : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnDestroy()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
