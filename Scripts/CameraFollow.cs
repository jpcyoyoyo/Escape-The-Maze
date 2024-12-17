using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;             // Reference to the player's transform
    public Vector2 offset = new Vector2(0, 5);  // Offset position of the camera relative to the player
    public float smoothSpeed = 0.125f;   // Adjust for smooth movement

    private Vector3 initialCameraZ;      // To retain the camera's Z position

    void Start()
    {
        // Store the camera's initial Z position to keep it constant in 2D
        initialCameraZ = new Vector3(0, 0, transform.position.z);
    }

    void FixedUpdate()
    {
        // Calculate the desired position as a Vector2 with the offset
        Vector2 desiredPosition = (Vector2)player.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector2 smoothedPosition = Vector2.Lerp((Vector2)transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position, keeping the initial Z position
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, initialCameraZ.z);
    }
}


