using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float laneDistance = 3.0f; // Distance between lanes
    public float laneSwitchSpeed = 10.0f;
    public float forwardSpeed = 5.0f;

    [Header("Audio")]
    public AK.Wwise.Event switchLaneSound;

    private int currentLane = 1; // 0: Left, 1: Middle, 2: Right
    private Vector3 targetPosition;

    void Update()
    {
        // 1. Move Forward constantly
        transform.Translate(forwardSpeed * Time.deltaTime * Vector3.forward);

        // 2. Calculate Target Lane Position
        // If lane is 0, x = -3. If lane is 1, x = 0. If lane is 2, x = 3.
        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // 3. Smoothly slide to the target lane
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneSwitchSpeed);
    }

    // These methods are called by the New Input System
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed && currentLane > 0)
        {
            currentLane--;
            switchLaneSound.Post(gameObject);
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed && currentLane < 2)
        {
            currentLane++;
            switchLaneSound.Post(gameObject);
        }
    }
}