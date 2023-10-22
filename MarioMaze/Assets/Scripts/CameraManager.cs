using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    // here orthographic camera is used so FOV is actually m_Lens.OrthographicSize
    [SerializeField] private float FOVmax;
    [SerializeField] private float FOVmin;
    [SerializeField] private float FOVDefault;
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float shiftCameraPerspectiveSpeed = 20f;
    [SerializeField] private float cameraTurnAngle;

    private const float EPS = .1f;

    private float targetFOV;
    private bool isMoving;
    private Quaternion targetCameraRotation;

    private void Awake()
    {
        Instance = this;
        virtualCamera.m_Lens.OrthographicSize = FOVDefault;
        targetFOV = virtualCamera.m_Lens.OrthographicSize;
        targetCameraRotation = transform.rotation;
        isMoving = false;
    }

    public void SetY(float y)
    {
        virtualCamera.transform.position = new Vector3(
            virtualCamera.transform.position.x,
            y,
            virtualCamera.transform.position.z
        );
    }

    private void Start()
    {
        GameInput.Instance.OnCameraTurn += GameInput_OnCameraTurn;
        virtualCamera.m_Lens.OrthographicSize = FOVDefault;
    }

    private void GameInput_OnCameraTurn(bool turnDir)
    {
        isMoving = true;
        Vector3 offset = new Vector3(0, turnDir ? cameraTurnAngle : -cameraTurnAngle, 0);
        targetCameraRotation = Quaternion.Euler(targetCameraRotation.eulerAngles + offset);
    }

    private void Update()
    {
        HandleCameraAngle();
        HandleCameraZoom();
    }

    private void HandleCameraAngle()
    {
        if (isMoving)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetCameraRotation, Time.deltaTime * shiftCameraPerspectiveSpeed);
            if (Mathf.Abs(transform.eulerAngles[1] - targetCameraRotation.eulerAngles[1]) < EPS)
            {
                transform.rotation = targetCameraRotation;
                isMoving = false;
            }
        }
    }

    private void HandleCameraZoom() {
        float zoomVal = GameInput.Instance.GetZoomVal();
        if (zoomVal > 0)
            targetFOV -= zoomSpeed;
        if (zoomVal < 0)
            targetFOV += zoomSpeed;

        targetFOV = Mathf.Clamp(targetFOV, FOVmin, FOVmax);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetFOV, Time.deltaTime * 10);
    }

    public void ShiftUp()
    {
        var cur_y = virtualCamera.transform.position.y;

        var new_y = Mathf.Clamp(cur_y + 250, 0, 500);
        
        if (new_y != cur_y)
        {
            StopAllCoroutines();
            StartCoroutine(CameraShiftCoroutine(new_y));
        }
    }
    
    public void ShiftDown()
    {
        var cur_y = virtualCamera.transform.position.y;

        var new_y = Mathf.Clamp(cur_y - 250, 0, 500);

        if (new_y != cur_y)
        {
            StopAllCoroutines();
            StartCoroutine(CameraShiftCoroutine(new_y));
        }
    }

    public IEnumerator CameraShiftCoroutine(float y)
    {
        float time = 0;
        while (time < 1)
        {
            var yval = Mathf.Lerp(
                virtualCamera.transform.position.y,
                y,
                time
            );

            virtualCamera.transform.position = new Vector3(
                virtualCamera.transform.position.x,
                yval,
                virtualCamera.transform.position.z
            );

            time += Time.deltaTime;

            yield return null;
        }
        
        virtualCamera.transform.position = new Vector3(
            virtualCamera.transform.position.x,
            y,
            virtualCamera.transform.position.z
        );

        Player.Instance.shift = null;
    }
}
