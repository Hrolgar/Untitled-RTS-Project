using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraControls : MonoBehaviour
{
    private Camera _camera;
    
    [SerializeField] private float _speed = 0.01f;
    [SerializeField] private float _zoomSpeed = 10;
    [SerializeField] private float _maxHeight = 40, _minHeight = 4;

    [SerializeField] [Range(0.01f, 0.001f)]
    private float _rotateSpeed = 0.01f;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var horizontal = transform.position.y * _speed * Input.GetAxisRaw("Horizontal");
        var vertical = transform.position.y * _speed * Input.GetAxisRaw("Vertical");
        var scrollSp = Mathf.Log(transform.position.y) * _zoomSpeed * -Input.GetAxisRaw("Mouse ScrollWheel");

        // Scrolling Section
        // This if/ else is so that you will never be able to scroll so fast you go through the terrain.
        if ((transform.position.y >= _maxHeight) && (scrollSp > 0))
        {
            scrollSp = 0;
        }
        else if ((transform.position.y <= _minHeight) && (scrollSp < 0))
        {
            scrollSp = 0;
        }

        if ((transform.position.y + scrollSp) > _maxHeight)
        {
            scrollSp = _maxHeight - transform.position.y;
        }
        else if ((transform.position.y + scrollSp) < _minHeight)
        {
            scrollSp = _minHeight - transform.position.y;
        }


        // if (transform.position.y == _maxHeight)
        // {
        //     transform.rotation = Quaternion.Euler(10, 0, 0);
        // }
        // else if (transform.position.y == _minHeight)
        // {
        //     transform.rotation = Quaternion.Euler(-10, 0, 0);
        // }
        // else
        // {
        //     transform.rotation = Quaternion.Euler(0, 0, 0);
        // }


        var verticalMove = new Vector3(0, scrollSp, 0);
        var lateralMove = horizontal * transform.right;
        var forwardMove = transform.forward;

        // Stopping the camera to move down.
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vertical;

        var move = verticalMove + lateralMove + forwardMove;
        transform.position += move;

        GetCameraRotation();
    }

    private void GetCameraRotation()
    {
        if (_camera is null) return;
        var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        if (!Physics.Raycast(ray, out var hitInfo)) return;
        // Debug.DrawRay(ray.origin, ray.direction * 100, Color.black );
        var target = hitInfo.transform.position;
        bool rotateClockwise;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotateClockwise = true;
            StartCoroutine(Rotate(target, rotateClockwise));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            rotateClockwise = false;
            StartCoroutine(Rotate(target, rotateClockwise));
        }
    }

    private IEnumerator Rotate(Vector3 target, bool rotateClockwise)
    {
        for (var i = 1; i <= 90; i++)
        {
            transform.RotateAround(target, Vector3.up, rotateClockwise ? 1: -1);
            yield return new WaitForSeconds(_rotateSpeed);
        }
    }
}

//----------------------------------------
// old rotation with middle mouse and tilt
//----------------------------------------

// private Vector2 _p1, _p2;
// private float _rotateSpeed = 0.1f,


// if (Input.GetMouseButtonDown(2))
// {
//     _p1 = Input.mousePosition;
// }
//
// if (Input.GetMouseButton(2))
// {
//     _p2 = Input.mousePosition;
//     
//     var dx = (_p2 - _p1).x * _rotateSpeed;
//     var dy = (_p2 - _p1).y * _rotateSpeed;
//     transform.rotation *= Quaternion.Euler(new Vector3(0,dx,0));
//     // Tilting of the camera up and down, do we like it or not? 
//     transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy,0,0));
//     _p1 = _p2;
// }
//
// if (Input.GetMouseButtonUp(2))
// {
//     transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(60,0,0));
// }
//