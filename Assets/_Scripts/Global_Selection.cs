using System;
using System.Collections;
using UnityEngine;

public class Global_Selection : MonoBehaviour
{
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
    // TODO - Fix last selection taking move commands while no (visibly) selected units - happens on single select only
    // TODO - Repeated single clicks not responding as expected - wait a frame or so?

    private void Start()
    {
        if (transform.position != Vector3.zero)
        {
            transform.position = Vector3.zero;
        }
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

                if (Physics.Raycast(ray, out _hit, 5000, ~_groundLayer))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) // Inclusive select
                    {
                        SelectionManager.Instance.AddSelected(_hit.transform.gameObject);
                    }
                    else if (Input.GetKey(KeyCode.LeftControl)) // Deselect
                    {
                        SelectionManager.Instance.Deselect(_hit.transform.gameObject.GetInstanceID());
                    }
                    else // Exclusive select
                    {
                        /*SelectionManager.Instance.DeselectAll();
                        SelectionManager.Instance.AddSelected(_hit.transform.gameObject);*/
                        
                        StartCoroutine(ClearAndReselect(_hit.transform.gameObject)); // Possible other alternative: Check if same - ignore if
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //Do nothing
                    }
                    else
                    {
                        SelectionManager.Instance.DeselectAll();
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

                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    SelectionManager.Instance.DeselectAll();
                }

                Destroy(_selectionBox, 0.02f); // TODO: Replace and reset with setActive variant?
            }
            
            UnitManager.Instance.SetUnitSelection();
            _dragSelect = false;
        }
    }

    IEnumerator ClearAndReselect(GameObject go)
    {
        SelectionManager.Instance.DeselectAll();
        yield return null;
        SelectionManager.Instance.AddSelected(go);
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            SelectionManager.Instance.Deselect(other.gameObject.GetInstanceID());
        }
        else
        {
            SelectionManager.Instance.AddSelected(other.gameObject);
        }
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
