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

    // [Header("SelectUnits")] 
    
    
    // public RectTransform selectionBox = null;
    // private Vector2 boxStartPos;

    private void Start()
    {
        _camera = Camera.main;
        _unit = GameObject.Find("Unit01").GetComponent<Unit>();
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (_camera is null) return;
        Vector3 target = GetMousePosition();
        _unit.MoveSelectedUnit(target);
    }

    private void CameraMovement()
    {
        var currentPos = transform.position;
        if (Input.GetAxisRaw("Vertical") > 0 || Input.mousePosition.y >= Screen.height - _borderRadius)
        {
            currentPos.z += _camSpeed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Vertical") < 0|| Input.mousePosition.y <= _borderRadius)
        {
            currentPos.z -= _camSpeed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Horizontal") > 0 || Input.mousePosition.x >= Screen.width - _borderRadius)
        {
            currentPos.x += _camSpeed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Horizontal") < 0 || Input.mousePosition.x <= _borderRadius)
        {
            currentPos.x -= _camSpeed * Time.deltaTime;
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        currentPos.y -= scroll * _scrollSpeed * 100f * Time.deltaTime;
        currentPos.y = Mathf.Clamp(currentPos.y, _minY, _maxY);

        currentPos.x = Mathf.Clamp(currentPos.x, -_screenLimit.x, _screenLimit.x);
        currentPos.z = Mathf.Clamp(currentPos.z, -_screenLimit.y, _screenLimit.y);

        transform.position = currentPos;
    }

    public Vector3 GetMousePosition()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.point : Vector3.zero;
    }
}