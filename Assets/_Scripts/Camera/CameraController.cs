using System;
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

    // Input
    private float _hInput;
    private float _vInput;
    private float _scroll;

    private void Start()
    {
        _camera = Camera.main;
        _debugMode = GameManager.Instance.IsDebugMode();
    }

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");
        _scroll = Input.GetAxis("Mouse ScrollWheel");
    }

    private void LateUpdate()
    {
        CameraPan();
        CameraZoom();
    }
    
    private void CameraPan()
    {
        var currentPos = transform.position;
        if (_vInput > 0 || !_debugMode && Input.mousePosition.y >= Screen.height - _borderRadius)
        {
            currentPos.z += _camSpeed * Time.deltaTime;
        }

        if (_vInput < 0|| Input.mousePosition.y <= _borderRadius && !_debugMode)
        {
            currentPos.z -= _camSpeed * Time.deltaTime;
        }

        if (_hInput > 0 || Input.mousePosition.x >= Screen.width - _borderRadius && !_debugMode)
        {
            currentPos.x += _camSpeed * Time.deltaTime;
        }

        if (_hInput < 0 || Input.mousePosition.x <= _borderRadius && !_debugMode)
        {
            currentPos.x -= _camSpeed * Time.deltaTime;
        }
        
        currentPos.x = Mathf.Clamp(currentPos.x, -_screenLimit.x, _screenLimit.x);
        currentPos.z = Mathf.Clamp(currentPos.z, -_screenLimit.y, _screenLimit.y);

        transform.position = currentPos;
    }
    

    // private void GetCameraRotation()
    // {
    //     if (_camera is null) return;
    //     var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
    //     if (!Physics.Raycast(ray, out var hitInfo)) return;
    //     // Debug.DrawRay(ray.origin, ray.direction * 100, Color.black );
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         var target = hitInfo.transform.position;
    //         transform.RotateAround(target, Vector3.up, 90);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         var target = hitInfo.transform.position;
    //         transform.RotateAround(target, Vector3.up, -90);
    //     }
    // }

    private void CameraZoom()
    {
        if (_scroll == 0) return;
        
        var currentPos = transform.position;
        currentPos.y -= _scroll * _scrollSpeed * 100f * Time.deltaTime;
        currentPos.y = Mathf.Clamp(currentPos.y, _minY, _maxY);
        transform.position = currentPos;
    }
    

    // Change Angle
    private void CameraRotation()
    {
        
    }
}