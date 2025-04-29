using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Objek yang diikuti kamera
    public float smoothSpeed = 0.125f; // Kecepatan kamera mengikuti
    public Vector3 offset = new Vector3(0, 2, -10); // Posisi kamera relatif terhadap target (lebih ke atas)

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
