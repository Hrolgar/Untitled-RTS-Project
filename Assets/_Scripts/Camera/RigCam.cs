using System;
using UnityEngine;

public class RigCam : MonoBehaviour
{
    private Transform _cameraTransform = null;

    [Header("CameraOptions")]
    [Tooltip("Normal speed is when you don't hold down shift.")]
    [SerializeField] private float _normalSpeed = 0;
    [Tooltip("Fast speed is when you do hold down shift.")]
    [SerializeField] private float _fastSpeed = 0;
    [Tooltip("MovementSpeed is either Normal or Fast speed.")]
    [SerializeField] private float _movementSpeed = 0;
    [Tooltip("Time for your Camera to move")]
    [SerializeField] private float _movementTime = 0;
    [Tooltip("How much to rotate")]
    [SerializeField] private float _rotationAmount = 0;
    [Tooltip("How much to zoom on the y and z axis")]
    [SerializeField] private Vector3 _zoomAmount = Vector3.zero;

    [SerializeField] private float _minZoom = 0;
    [SerializeField] private float _maxZoom = 0;

    //Serialize for debug purposes 
    private Vector3 _newZoom = Vector3.zero;
    private Vector3 _newPosition = Vector3.zero;
    private Quaternion _newRotation = Quaternion.identity;


    private void Awake()
    {
        if (!(Camera.main is null)) _cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        _newPosition = transform.position;
        _newRotation = transform.rotation;
        _newZoom = _cameraTransform.localPosition;
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        // Camera move quicker, this might be a little useful addition.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movementSpeed = _fastSpeed;
        }
        else
        {
            _movementSpeed = _normalSpeed;
        }

        //Movement with keys and arrows.
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            _newPosition += (transform.forward * _movementSpeed);
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            _newPosition += (transform.forward * -_movementSpeed);
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _newPosition += (transform.right * _movementSpeed);
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _newPosition += (transform.right * -_movementSpeed);
        }
        // Rotate
        if (Input.GetKey(KeyCode.Q))
        {
            _newRotation *= Quaternion.Euler(Vector3.up * _rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            _newRotation *= Quaternion.Euler(Vector3.up * -_rotationAmount);
        }

        // Zooming
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0 && _newZoom.z < _maxZoom)
            {
                _newZoom += Input.mouseScrollDelta.y * _zoomAmount;
            }

            if (Input.mouseScrollDelta.y < 0 && _newZoom.z > _minZoom)
            {
                _newZoom += Input.mouseScrollDelta.y * _zoomAmount;
            }
        }

        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _newZoom, Time.deltaTime * _movementTime);

        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * _movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * _movementTime);
    }


}