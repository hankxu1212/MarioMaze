using System;
using Cinemachine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public static Player Instance { get; private set; }
    private readonly Quaternion rightRotation = Quaternion.Euler(new Vector3(0, 90, 0));
    
    [Header("Player Controller")]
    [SerializeField] private float moveSpeed = 12f;
    private CharacterController controller;
    private Vector3 moveDir;

    public event Action OnCoinCollected;
    public event Action OnFinish;
    
    public Coroutine shift = null;


    private void Awake()
    {
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
            Destroy(hit.gameObject);
            OnCoinCollected?.Invoke();
        }
        else if (hit.gameObject.CompareTag("Finish"))
        {
            OnFinish?.Invoke();
        }
        else if (hit.gameObject.CompareTag("Portal"))
        {
            var portal = hit.gameObject.GetComponent<Portal>();
            CameraManager.Instance.SetY(portal.GetY());
            transform.position = portal.GetPlayerCoords();
        }
    }

    // move towards `moveDir` with speed
    private void Move(float speed)
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
            return;
        
        // calculates orthographic camera angle
        Vector3 forward = virtualCamera.transform.forward;
        forward.y = 0;
        Vector3 right = rightRotation * forward;
        forward *= inputVector.z;
        right *= inputVector.x;
        moveDir = (forward + right).normalized;
        
        // move
        Move(moveSpeed);
    }
}
