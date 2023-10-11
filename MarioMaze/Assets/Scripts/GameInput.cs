using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    
    // list of events
    public event Action<bool> OnCameraTurn; // 1 for right. 0 for left
    private GameInputActions gameInputActions;

    private void Awake()
    {
        gameInputActions = new GameInputActions();
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // subscribe listeners
    private void Start()
    {
        // enable all maps
        gameInputActions.Player.Enable();
        gameInputActions.Player.CameraRight.performed += CameraRight_performed;
        gameInputActions.Player.CameraLeft.performed += CameraLeft_performed;
    }
    
    private void OnDisable()
    {
        gameInputActions.Player.CameraRight.performed -= CameraRight_performed;
        gameInputActions.Player.CameraLeft.performed -= CameraLeft_performed;
        gameInputActions.Dispose();
    }
    
    /********************** TOGGLE GAME STATE FUNCTIONS **********************/
    
    private void CameraLeft_performed(InputAction.CallbackContext obj)
    {
        OnCameraTurn?.Invoke(true);
    }

    private void CameraRight_performed(InputAction.CallbackContext obj)
    {
        OnCameraTurn?.Invoke(false);
    }
    
    public float GetZoomVal()
    {
        return gameInputActions.Player.CameraZoom.ReadValue<float>();
    }

    public Vector3 GetMovementVectorNormalized()
    {
        Vector2 readVal = gameInputActions.Player.Move.ReadValue<Vector2>();
        return new Vector3(readVal.x, 0, readVal.y);
    }
}