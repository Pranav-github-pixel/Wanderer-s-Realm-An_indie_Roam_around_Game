using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementScript : MonoBehaviour
{
    public Interactable focus;
    private float _gravity = 9.81f;
    private float _velocity;
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float interactionRadius = 3f;
    private float verticalVelocity; // Added to store vertical velocity
    private float gravityMultiplier = 1.0f; // Adjust gravity if needed

    void Update()
    {
        // Handle horizontal and vertical input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Apply gravity before moving
            ApplyGravity();
            controller.Move(moveDir.normalized * speed * Time.deltaTime + new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void ApplyGravity()
    {
        // Apply gravity if the character is not grounded
        if (!controller.isGrounded)
        {
            verticalVelocity -= _gravity * gravityMultiplier * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -1f; // A small negative value to ensure the character stays grounded
        }
    }

    void Interact()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Interactable interactable = hitCollider.GetComponent<Interactable>();
            if (interactable != null)
            {
                // SetFocus(interactable);
                Debug.Log("Interacting with: " + interactable.gameObject.name);
            }
        }
    }
}
