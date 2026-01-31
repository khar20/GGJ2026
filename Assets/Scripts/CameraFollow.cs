using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 5, -10);

    void LateUpdate()
    {
        // Follow the player's Z position but stay at a fixed height/offset
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);
        transform.position = newPos;
    }
}