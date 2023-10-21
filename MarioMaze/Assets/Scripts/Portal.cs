using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private float cameraShiftY;
    [SerializeField] private float X;
    [SerializeField] private float Z;

    public float GetY() => cameraShiftY;

    public Vector3 GetPlayerCoords()
    {
        return new Vector3(X, cameraShiftY + 0.08f, Z);
    }
}
