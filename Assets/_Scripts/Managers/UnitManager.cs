using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    private Selected_Dictionary _selectedUnits;

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

    public void SetUnitSelection(Selected_Dictionary selectedUnits)
    {
        _selectedUnits = selectedUnits;
    }

    public void MoveUnits(Vector3 target)
    {
        if (_selectedUnits)
        {
            foreach (var unit in _selectedUnits.selectedTable)
            {
                unit.Value.GetComponent<Unit>().MoveSelectedUnit(target);
            }
        }
    }
}
