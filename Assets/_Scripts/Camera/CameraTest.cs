using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraTest : MonoBehaviour
{
    [SerializeField] private float _speed = 0.01f;
    [SerializeField] private float _rotateSpeed = 0.1f, _zoomSpeed = 10;
    [SerializeField] private float _maxHeight = 40, _minHeight = 4;

    private Vector2 _p1, _p2;
    private void Update()
    {
        var horizontal = transform.position.y * _speed * Input.GetAxisRaw("Horizontal");
        var vertical = transform.position.y * _speed * Input.GetAxisRaw("Vertical");
        var scrollSp = Mathf.Log(transform.position.y) * _zoomSpeed * -Input.GetAxisRaw("Mouse ScrollWheel");

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
        if (Input.GetMouseButtonDown(2))
        {
            _p1 = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            _p2 = Input.mousePosition;
            
            var dx = (_p2 - _p1).x * _rotateSpeed;
            var dy = (_p2 - _p1).y * _rotateSpeed;
            transform.rotation *= Quaternion.Euler(new Vector3(0,dx,0));
            // Tilting of the camera up and down, do we like it or not? 
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy,0,0));
            _p1 = _p2;
        }
        
        

    }
}