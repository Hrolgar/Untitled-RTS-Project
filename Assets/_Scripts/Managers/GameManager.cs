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
    
    // Variables
    // private CameraController _cameraController;
    
    //Unit selection
    [SerializeField] private RectTransform _selectionBox = null;

    private Vector2 _initialClickPosition = Vector2.zero;
    private Vector2 _startPoint, _endPoint;
    
    private void Start()
    {
        if (!_instance) _instance = this;
        else Destroy(gameObject);

        // if (!(Camera.main is null)) _cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _initialClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _selectionBox.anchoredPosition = _initialClickPosition;
        }

        if (Input.GetMouseButton(0))
        {
            UnitSelectionBox();
        }
        
        // After we release the mouse button.
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 bounds = new Vector2(_startPoint.x, _endPoint.y);
            //Debug.Log("StartPos: " + _startPoint + "\n EndPos: " + _endPoint + "\n bounds: " + bounds);
            SelectUnits(bounds);
            _initialClickPosition = Vector2.zero;
            _selectionBox.anchoredPosition = Vector2.zero;
            _selectionBox.sizeDelta = Vector2.zero;
        }
        
    }

    private void UnitSelectionBox()
    {
        // Store the current mouse position in screen space.
        var currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        // How far have we moved the mouse?
        var difference = currentMousePosition - _initialClickPosition;
        
        var startPoint = _initialClickPosition;
        _startPoint = startPoint;
 
        // The following code accounts for dragging in various directions.
        if (difference.x < 0)
        {
            startPoint.x = currentMousePosition.x;
            difference.x = -difference.x;
        }
        if (difference.y < 0)
        {
            startPoint.y = currentMousePosition.y;
            difference.y = -difference.y;
        }
 
        // Set the anchor, width and height every frame.
        _selectionBox.anchoredPosition = startPoint;
        _selectionBox.sizeDelta = difference;

        _endPoint = difference;
        
    }

    private void SelectUnits(Vector2 boundsPos)
    {
        Collider newCollider = new Collider();
    }
}
