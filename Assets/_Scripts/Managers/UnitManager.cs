using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private Dictionary<int, GameObject> _selectedUnits = new Dictionary<int, GameObject>();

    void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SetUnitSelection()
    {
        _selectedUnits = SelectionManager.Instance.GetCurrentSelection();
    }

    // TODO: Fix stopping issue
    public void MoveUnits(Vector3 target)
    {
        if (_selectedUnits.Count <= 0) return;
        foreach (var unit in _selectedUnits)
        {
            unit.Value.GetComponent<Unit>().MoveSelectedUnit(target);
        }
    }
}
