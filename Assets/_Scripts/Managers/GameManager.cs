using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private bool _debugMode = false;

    // Could have the GameManager set up the other managers?
    
    public bool IsDebugMode()
    {
        return _debugMode;
    } 
}
