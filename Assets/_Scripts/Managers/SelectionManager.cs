using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoSingleton<SelectionManager>
{
    private Camera _mainCam;
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();
    
    protected override void Init()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (_mainCam is null) return;
        UnitManager.Instance.MoveUnits(GetMousePosition());
    }

    public Vector3 GetMousePosition()
    {
        var ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit.point : Vector3.zero;
    }

    public void AddSelected(GameObject go)
    {
        var id = go.GetInstanceID();
        if (!selectedTable.ContainsKey(id))
        {
            selectedTable.Add(id, go);
            go.AddComponent<Selection_Component>();
            //Debug.Log($"Added {id} to selected dict");
        }
    }

    public void Deselect(int id)
    {
        if (selectedTable.ContainsKey(id))
        {
            Destroy(selectedTable[id].GetComponent<Selection_Component>());
            selectedTable.Remove(id);
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in selectedTable)
        {
            if (unit.Value)
            {
                Destroy(selectedTable[unit.Key].GetComponent<Selection_Component>());
            }
        }
        
        selectedTable.Clear();
    }
}
