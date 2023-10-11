using System;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Idle, 
    Walk
}

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public static Player Instance { get; private set; }
    private readonly Quaternion rightRotation = Quaternion.Euler(new Vector3(0, 90, 0));
    
    [Header("Player Controller")]
    [SerializeField] private float moveSpeed = 12f;
    private PlayerState state;
    private CharacterController controller;
    private Vector3 moveDir;

    public event Action OnCoinCollected;

    private int coins = 0;
    
    private void Awake()
    {
        state = PlayerState.Idle;
        Instance = this;
        
        controller = GetComponent<CharacterController>();
    }
    
    
    private void Update()
    {
        HandleMovement();
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Coin"))
        {
            coins++;
            Destroy(hit.gameObject);
            OnCoinCollected?.Invoke();
        }

        if (hit.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Finished! Congratulations");
        }
    }

    // move towards `moveDir` with speed
    public void Move(float speed)
    {
        Vector3 moveDist = speed * Time.deltaTime * moveDir;
        controller.Move(moveDist);
        
        // rotation
        transform.forward = new Vector3(moveDir.x, 0, moveDir.z);
    }

    // called when player is either moving or idle
    private void HandleMovement()
    {
        Vector3 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        if (inputVector == Vector3.zero)
        {
            state = PlayerState.Idle;
            return;
        }
        
        // calculates orthographic camera angle
        Vector3 forward = virtualCamera.transform.forward;
        forward.y = 0;
        Vector3 right = rightRotation * forward;
        forward *= inputVector.z;
        right *= inputVector.x;
        moveDir = (forward + right).normalized;
        
        // move
        state = PlayerState.Walk;
        Move(moveSpeed);
    }
    
    public bool IsWalking() => state == PlayerState.Walk;
    public bool IsIdle() => state == PlayerState.Idle;
}
