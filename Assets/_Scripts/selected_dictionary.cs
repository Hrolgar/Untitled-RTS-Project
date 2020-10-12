using System.Collections.Generic;
using UnityEngine;

public class Selected_Dictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();
        if (!selectedTable.ContainsKey(id))
        {
            selectedTable.Add(id, go);
            go.AddComponent<Selection_Component>();
            Debug.Log($"Added {id} to selected dict");
        }
    }

    public void Deselect(int id)
    {
        Destroy(selectedTable[id].GetComponent<Selection_Component>());
        selectedTable.Remove(id);
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
