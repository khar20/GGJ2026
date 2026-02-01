using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player; // Drag the Player object here in the Inspector

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 5, -10); // Adjust to your liking
    public float smoothSpeed = 0.125f; // How "snappy" the camera is

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Define where the camera wants to be
        Vector3 desiredPosition = player.position + offset;

        // 2. Smoothly move from current position to desired position
        // We use Lerp to make lane switching look smooth for the camera too
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. Apply the position
        transform.position = smoothedPosition;

        // 4. Always look at the player slightly ahead
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}