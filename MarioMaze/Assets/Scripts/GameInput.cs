using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    
    // list of events
    private GameInputActions gameInputActions;

    private void Awake()
    {
        gameInputActions = new GameInputActions();
        
        Instance = this;
    }

    // subscribe listeners
    private void Start()
    {
        // enable all maps
        gameInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        gameInputActions.Dispose();
    }
    
    /********************** TOGGLE GAME STATE FUNCTIONS **********************/
    public Vector3 GetMovementVectorNormalized()
    {
        Vector2 readVal = gameInputActions.Player.Move.ReadValue<Vector2>();
        return new Vector3(readVal.x, 0, readVal.y);
    }
}