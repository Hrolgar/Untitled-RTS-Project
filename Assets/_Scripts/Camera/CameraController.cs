using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Unit _unit;
    
    [Header("CameraOptions")]
    [SerializeField] private float _camSpeed = 20f;
    [SerializeField] private float _borderRadius = 10f;
    [SerializeField] private Vector2 _screenLimit = new Vector2(0,0);
    
    [SerializeField] private float _scrollSpeed = 2;
    [SerializeField] private float _minY = 20, _maxY = 120;
    private bool _debugMode;

    private void Start()
    {
        _camera = Camera.main;
        _debugMode = GameManager.Instance.IsDebugMode();
    }

    private void LateUpdate()
    {
        CameraPanning();
    }
    
    private void CameraPanning()
    {
        var currentPos = transform.position;
        if (Input.GetAxisRaw("Vertical") > 0 || !_debugMode && Input.mousePosition.y >= Screen.height - _borderRadius)
        {
            currentPos.z += _camSpeed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Vertical") < 0|| Input.mousePosition.y <= _borderRadius && !_debugMode)
        {
            currentPos.z -= _camSpeed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Horizontal") > 0 || Input.mousePosition.x >= Screen.width - _borderRadius && !_debugMode)
        {
            currentPos.x += _camSpeed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Horizontal") < 0 || Input.mousePosition.x <= _borderRadius && !_debugMode)
        {
            currentPos.x -= _camSpeed * Time.deltaTime;
        }

        currentPos.x = Mathf.Clamp(currentPos.x, -_screenLimit.x, _screenLimit.x);
        currentPos.z = Mathf.Clamp(currentPos.z, -_screenLimit.y, _screenLimit.y);

        transform.position = currentPos;
    }

    private void CameraZoom()
    {
        var currentPos = transform.position;
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        
        currentPos.y -= scroll * _scrollSpeed * 100f * Time.deltaTime;
        currentPos.y = Mathf.Clamp(currentPos.y, _minY, _maxY);
    }
    

    // Change Angle
    private void CameraRotation()
    {
        
    }
}