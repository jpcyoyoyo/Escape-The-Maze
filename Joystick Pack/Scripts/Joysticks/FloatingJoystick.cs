using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();

        // Enable the InputAction for movement and subscribe to its callbacks
        if (moveAction != null)
        {
            moveAction.Enable();
            moveAction.performed += OnMoveInput;   // Called when there is movement input
            moveAction.canceled += OnMoveInput;    // Reset input when action is canceled
        }

        background.gameObject.SetActive(false);  // Hide background initially for floating joystick
    }

    private void OnDestroy()
    {
        // Clean up event subscription to avoid memory leaks
        if (moveAction != null)
        {
            moveAction.performed -= OnMoveInput;
            moveAction.canceled -= OnMoveInput;
            moveAction.Disable();
        }
    }

    // Override to move joystick background and show it when pointer is down
    public override void OnPointerDown(PointerEventData eventData)
    {
        background.position = eventData.position;  // Move joystick background to touch position
        background.gameObject.SetActive(true);     // Show background
        base.OnPointerDown(eventData);
    }

    // Override to hide joystick background when pointer is up
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        background.gameObject.SetActive(false);  // Hide background
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            input = context.ReadValue<Vector2>();  // Read the vector input from the Input System
            UpdateHandlePosition();

            // Display the joystick background only when there is input
            background.gameObject.SetActive(input != Vector2.zero);
        }
    }

    // Helper function to update the handle's position based on input
    private void UpdateHandlePosition()
    {
        Vector2 radius = background.sizeDelta / 2;
        handle.anchoredPosition = input * radius * handleRange;
    }
}
