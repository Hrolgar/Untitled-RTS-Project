using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selected_Dictionary_youtube_commentersedition : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Selected_Dictionary_youtube_commentersedition _selected_Dictionary;
    [SerializeField]
    private RectTransform _uiSelectionBox;
    private Image _uiSelectionImage;
    [SerializeField]
    private LayerMask _groundLayerMask;                  //using a plane under the actual ground if the ground is not flat is seriously advisable as to avoid gameobjects going unselected because they get under the inclined mesh;

    private bool _isDragSelect;
    private Vector3 _uiClickStart;
    private Vector3 _uiClickEnd;
    private float _width, _height;
    private Vector3 p1, p2;
    [SerializeField]
    MeshCollider _meshCollider;

    private void Awake()
    {
        //gets the image so it can be enabled and disabled instead of destroyed for perforamnce reasons.
        _uiSelectionImage = _uiSelectionBox.GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _uiClickStart = Input.mousePosition;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                //_selected_Dictionary.RemoveSelections();
            }
        }
        if (Input.GetMouseButton(0))
        {
            _uiClickEnd = Input.mousePosition;

            UpdateSelectionBox();
            
            if (_isDragSelect)                         //this check serves to fix the odd case in which the quad is so small unity belives its coplanar
                                                       //Can be moved to OnMouseButtonUp if the performance cost of realtme unit selection is not worth it
            {
                CastSelectionArea();
            }



        }
        if (Input.GetMouseButtonUp(0))
        {
            DisableSelectionBox();
        }

    }
    void UpdateSelectionBox()
    {
        p1 = _uiClickStart;
        p2 = _uiClickEnd;
        if (Vector3.SqrMagnitude(p1 - p2) > 1f)
        {
            EnableSelectionBox();                                                 
            Vector3 swap = p1;                      //this function makes sure that p1 and p2 are always in the same relative position on our box.                    
            if (p1.x > p2.x)                        //  -----P2
            {                                       //  |     |
                p1.x = p2.x;                        //  |     |
                p2.x = swap.x;                      //  P1-----
            }
            if (p1.y > p2.y)
            {
                p1.y = p2.y;
                p2.y = swap.y;
            }
            _width = p2.x - p1.x;
            _height = p2.y - p1.y;
            _uiSelectionBox.position = p1;
            _uiSelectionBox.sizeDelta = (new Vector2(_width, _height));
        }
    }

    void EnableSelectionBox()
    {
        _isDragSelect = true;
        _uiSelectionImage.enabled = true;
        _meshCollider.enabled = true;
    }

    void DisableSelectionBox()
    {
        _isDragSelect = false;
        _uiSelectionImage.enabled = false;
        _meshCollider.enabled = false;
    }

    private void CastSelectionArea()
    {
        Vector3[] points= new Vector3[5];

        Ray[] rays ={                                                               //will always be cast in this order
            _camera.ScreenPointToRay(p1),                                           //1---------2
            _camera.ScreenPointToRay(new Vector3(p1.x,p1.y+_height,p1.z)),          //|         |
            _camera.ScreenPointToRay(p2),                                           //|         |
            _camera.ScreenPointToRay(new Vector3(p2.x,p2.y-_height,p2.z)),          //0---------3
        };
        int i = 0;
        foreach (Ray ray in rays)
        {
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1000f,_groundLayerMask);
            points.SetValue(hit.point, i);
            i++;
        }
        points.SetValue(points[3] + Vector3.up * 0.01f, 3);                       //this avoids the case of the ground being perfectly flat and generating a perfectly planar quad
        points.SetValue(_camera.transform.position,4);
        _meshCollider.sharedMesh= GenerateMesh(points);
        _meshCollider.convex = true;
        _meshCollider.enabled = true;


    }

    private Mesh GenerateMesh(Vector3[] vertices)
    {
        int[] trigOrder ={
            2,1,0,
            0,3,2,
            0,1,4,
            1,2,4,
            2,3,4,
            3,0,4

        };
        Mesh mesh= new Mesh();
        mesh.RecalculateNormals();
        mesh.vertices = vertices;
        mesh.triangles = trigOrder;
        return mesh;
    }
    private void OnTriggerEnter(Collider other)
    {
            //_selected_Dictionary.AddSelections(other.gameObject);
    }
}