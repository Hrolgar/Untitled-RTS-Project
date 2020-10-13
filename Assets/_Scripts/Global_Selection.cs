using System;
using UnityEngine;

public class Global_Selection : MonoBehaviour
{
    private Selected_Dictionary _selectedTable;
    private RaycastHit _hit;

    private bool _dragSelect = false;
    [SerializeField] private LayerMask _groundLayer = default;

    // Collider Variables
    private MeshCollider _selectionBox;
    private Mesh _selectionMesh;
    
    // 2D Selection Box corners
    private Vector2[] _corners;
    
    // MeshCollider Vertices
    private Vector3[] _verts;
    private Vector3[] _vectors;
    
    private Vector3 _point1, _point2;

    // TODO - Personalize this script further!
    // TODO - Add Left Control modifier to remove from selection
    // TODO - Fix being able to select ground - limit selection in general
    // TODO - Repeated single clicks not responding as expected
    
    void Start()
    {
        _selectedTable = GetComponent<Selected_Dictionary>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _point1 = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if ((_point1 - Input.mousePosition).magnitude > 40)
            {
                _dragSelect = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!_dragSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(_point1);

                if (Physics.Raycast(ray, out _hit, 5000))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) // Inclusive select
                    {
                        Debug.Log("Leftshift");
                        _selectedTable.AddSelected(_hit.transform.gameObject);
                    }
                    else if (Input.GetKey(KeyCode.LeftControl))
                    {
                        _selectedTable.Deselect(_hit.transform.gameObject.GetInstanceID());
                    }
                    else // Exclusive select
                    {
                        _selectedTable.DeselectAll();
                        _selectedTable.AddSelected(_hit.transform.gameObject);
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //Do nothing
                    }
                    else if (Input.GetKey(KeyCode.LeftControl))
                    {
                        _selectedTable.Deselect(_hit.transform.gameObject.GetInstanceID());
                    }
                    else
                    {
                        _selectedTable.DeselectAll();
                    }
                }
            }
            else
            {
                _verts = new Vector3[4];
                _vectors = new Vector3[4];
                
                int i = 0;
                _point2 = Input.mousePosition;
                _corners = GetBoundingBox(_point1, _point2);

                foreach (var corner in _corners)
                {
                    //_layerMask = ~_layerMask;
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out _hit, 5000f, _groundLayer))
                    {
                        _verts[i] = new Vector3(_hit.point.x, _hit.point.y, _hit.point.z);
                        _vectors[i] = ray.origin - _hit.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), _hit.point, Color.red, 1.0f);
                    }

                    i++;
                }

                _selectionMesh = GenerateSelectionMesh(_verts, _vectors);

                _selectionBox = gameObject.AddComponent<MeshCollider>();
                _selectionBox.sharedMesh = _selectionMesh;
                _selectionBox.convex = true;
                _selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    _selectedTable.DeselectAll();
                }
                
                Destroy(_selectionBox, 0.02f);
            }
            
            UnitManager.Instance.SetUnitSelection(_selectedTable);
            _dragSelect = false;
        }

    }

    private void OnGUI()
    {
        if (_dragSelect)
        {
            var rect = Utils.GetScreenRect(_point1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(.8f, .8f, .95f, .25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(.8f, .8f, .95f, .25f));
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _selectedTable.AddSelected(other.gameObject);
    }

    // Logic to create Selection Box
    private Vector2[] GetBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if _point1 is to the left of _point2
        {
            if (p1.y > p2.y) // if _point1 is above _point2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if _point1 is below _point2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if _point1 is to the right of _point2
        {
            if (p1.y > p2.y) // if _point1 is above _point2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if _point1 is below _point2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;
    }
    
    // Generate a mesh from the 4 bottom points
    private Mesh GenerateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for(int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for(int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh {vertices = verts, triangles = tris};

        return selectionMesh;
    }

}
