using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Mono Singleton
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }
    
    //
    private CameraController _cameraController;
    
    private void Start()
    {
        if (!_instance) _instance = this;
        else Destroy(gameObject);

        _cameraController = Camera.main.GetComponent<CameraController>();
    }
    
    //Move selected unit to position
}
