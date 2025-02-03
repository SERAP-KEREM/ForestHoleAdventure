using Cinemachine;
using UnityEngine;

namespace _Main.Hole
{


public class HoleCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;  // Cinemachine Virtual Camera
    [SerializeField] private Transform holeTransform;                // Hole nesnesinin Transform'u
    [SerializeField] private float maxDistance = 10f;                 // En uzak mesafe
    [SerializeField] private float minDistance = 5f;                  // En yak?n mesafe
    [SerializeField] private float growthFactor = 1f;                 // Hole b�y�d�k�e uzakla?ma miktar?
    [SerializeField] private float smoothTime = 0.5f;                 // Yumu?ak ge�i? s�resi

    private float currentDistance;
    private float velocity = 0f;  // Smooth ge�i? i�in kullan?lacak velocity

    private void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Cinemachine Virtual Camera atanmam??!");
            return;
        }

        if (holeTransform == null)
        {
            Debug.LogError("Hole Transform atanmam??!");
            return;
        }

        // Ba?lang?� mesafesini ayarl?yoruz
        currentDistance = minDistance;
    }

    private void Update()
    {
        AdjustCameraDistance(); // Hole b�y�d�k�e kamera uzakl???n? ayarlay?n
    }

    private void AdjustCameraDistance()
    {
        // Hole b�y�kl�?�n� al?yoruz
        float holeRadius = holeTransform.localScale.x;  // Hole'un b�y�kl�?�n� radius ile �l��yorduk, burada x eksenini kullan?yoruz

        // Hole b�y�d�k�e kamera uzakl???n? ayarl?yoruz
        float targetDistance = Mathf.Lerp(minDistance, maxDistance, holeRadius * growthFactor);

        // Kameray? yumu?ak bir ?ekilde uzakla?t?r?yoruz
        currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref velocity, smoothTime);

        // VirtualCamera Follow mesafesini ayarl?yoruz
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, currentDistance, -currentDistance);
    }
}
}   
