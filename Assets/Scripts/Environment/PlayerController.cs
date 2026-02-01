using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float laneDistance = 20.0f; // Distance between lanes
    public float laneSwitchSpeed = 10.0f;
    public float forwardSpeed = 5.0f;

    [Header("Jump Settings")]
    public float jumpHeight = 2.0f;
    public float jumpDuration = 0.8f;
    private float verticalY = 0; // Offset from ground

    [Header("Audio")]
    public AK.Wwise.Event switchLaneSound;

    private int currentLane = 1; // 0: Left, 1: Middle, 2: Right
    private Vector3 targetPosition;

    void Update()
    {
        // 1. Move Forward
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 2. Calculate Lane Position
        float targetX = (currentLane - 1) * laneDistance;

        // 3. Apply X and Y (Jump) changes
        // We add verticalY to the base height
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Override the Y of targetPosition with our jump height
        Vector3 finalPos = targetPosition;
        finalPos.y = 1.5f + verticalY; // 0.5f is the base height of the capsule

        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * laneSwitchSpeed);
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

    public void TriggerJump()
    {
        StartCoroutine(JumpRoutine());
    }

    IEnumerator JumpRoutine()
    {
        float elapsed = 0;
        while (elapsed < jumpDuration)
        {
            // Simple Parabola: Sin wave for smooth up and down
            float progress = elapsed / jumpDuration;
            verticalY = Mathf.Sin(progress * Mathf.PI) * jumpHeight;

            elapsed += Time.deltaTime;
            yield return null;
        }
        verticalY = 0;
    }
}