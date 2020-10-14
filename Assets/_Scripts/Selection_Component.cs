using UnityEngine;

public class Selection_Component : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private GameObject _selectedObj;
    void Start()
    {
        _selectedObj = new GameObject("Selected");
        _selectedObj.transform.parent = transform;
        _selectedObj.transform.localPosition = new Vector3(0, -0.5f, 0);
        _selectedObj.transform.localScale = Vector3.one * 0.75f;
        _selectedObj.transform.localEulerAngles = new Vector3(90, 0, 0);
        _spriteRenderer = _selectedObj.AddComponent<SpriteRenderer>();
        
        var selectionSprite = Resources.Load<Sprite>("Sprites/selected_marker");
        _spriteRenderer.sprite = selectionSprite;

    }

    void OnDestroy()
    {
        Destroy(_selectedObj);
    }
}
