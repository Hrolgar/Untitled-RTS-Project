using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private Dictionary<int, GameObject> _selectedUnits;

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
        //DontDestroyOnLoad(gameObject);

    }

    void Update()
    {
        
    }

    public void SetUnitSelection()
    {
        _selectedUnits = SelectionManager.Instance.selectedTable;
    }

    public void MoveUnits(Vector3 target)
    {
        if (_selectedUnits.Count <= 0) return;
        foreach (var unit in SelectionManager.Instance.selectedTable)
        {
            unit.Value.GetComponent<Unit>().MoveSelectedUnit(target);
        }
    }
}
