using UnityEngine;

public class Selection_Component : MonoBehaviour
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
