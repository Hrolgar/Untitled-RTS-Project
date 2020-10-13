using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private bool _debugMode = false;

    
    public bool IsDebugMode()
    {
        return _debugMode;
    }
}
