using System;
using UnityEngine;
using UnityEngine.UI;

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
    private CameraController _cameraController;
    
    //Unit selection
    [SerializeField] private RectTransform _selectionBox;
    private Vector3 _startPos;
    private Vector3 _endPos;
    
    private void Start()
    {
        if (!_instance) _instance = this;
        else Destroy(gameObject);

        _cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = _cameraController.GetMousePosition();
        }

        if (Input.GetMouseButton(0))
        {
            SelectUnits();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Start: " + _startPos + " End: " + _endPos);
        }
    }

    //Select units
    private void SelectUnits()
    {
        _endPos = _cameraController.GetMousePosition();

        float width = _endPos.x - _startPos.x;
        float height = _endPos.z - _startPos.z;

        _selectionBox.sizeDelta = new Vector2(Mathf.Abs(width),Mathf.Abs(height));
        _selectionBox.anchoredPosition = _startPos + new Vector3(width / 2,0, height / 2);

    }
}
