using UnityEngine;
using Cinemachine;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    // here orthographic camera is used so FOV is actually m_Lens.OrthographicSize
    [SerializeField] private float FOVmax;
    [SerializeField] private float FOVmin;
    [SerializeField] private float FOVDefault;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float shiftCameraPerspectiveSpeed = 20f;

    private const float EPS = .1f;

    private float targetFOV;
    private bool isMoving;
    private Quaternion targetCameraRotation;

    private void Awake()
    {
        virtualCamera.m_Lens.OrthographicSize = FOVDefault;
        targetFOV = virtualCamera.m_Lens.OrthographicSize;
        targetCameraRotation = transform.rotation;
        isMoving = false;
    }

    private void Start()
    {
        GameInput.Instance.OnCameraTurn += GameInput_OnCameraTurn;
        virtualCamera.m_Lens.OrthographicSize = FOVDefault;
    }

    private void GameInput_OnCameraTurn(bool turnDir)
    {
        isMoving = true;
        Vector3 offset = new Vector3(0, turnDir ? 45 : -45, 0);
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
            targetFOV -= .5f;
        if (zoomVal < 0)
            targetFOV += .5f;

        targetFOV = Mathf.Clamp(targetFOV, FOVmin, FOVmax);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetFOV, Time.deltaTime * zoomSpeed); ;
    }
}
