using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float movementSpeed = 10;
    [SerializeField] float groundCheckDistance = 5;
    [SerializeField] float rotationSmoothingAmount = 0.01f;

    private Vector2 inputMovement;
    private Vector2 inputRot;

    [Header("Components")]
    [SerializeField] MeshRenderer mesh;
    private Rigidbody rb;

    [Header("Object reference")]
    [SerializeField] Gamemanager gameManager;

    private bool isGrounded;


    #region Initialize
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        GroundCheck();
    }

    private void HandleMovement()
    {
        Vector3 camForward = gameManager.cameraContainer.transform.forward;
        Vector3 camRight = gameManager.cameraContainer.transform.right;

        Vector3 DesiredPosition = camForward * inputMovement.y + camRight * inputMovement.x;

        //Move player's RigidBody
        if (isGrounded)
        {
            rb.velocity = new Vector3(DesiredPosition.x * movementSpeed, rb.velocity.y, DesiredPosition.z * movementSpeed);
        }
    }
    private void HandleRotation()
    {
        Vector3 camForward = gameManager.cameraContainer.transform.forward;
        Vector3 camRight = gameManager.cameraContainer.transform.right;

        Vector3 DesiredRotation = camForward * inputRot.y + camRight * inputRot.x;

        if (inputRot != Vector2.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(new Vector3(DesiredRotation.x, 0, DesiredRotation.z));
            mesh.transform.rotation = Quaternion.Slerp(desiredRotation, mesh.transform.rotation, rotationSmoothingAmount);
        }
    }
    private void GroundCheck()
    {
        Ray groundRay = new Ray(gameObject.transform.position, gameObject.transform.up * -1);
        isGrounded = Physics.Raycast(groundRay,groundCheckDistance);
    }


    //Input getting
    private void OnMovement(InputValue value) => inputMovement = value.Get<Vector2>();
    private void OnAiming(InputValue value) => inputRot = value.Get<Vector2>();


    private void OnDefense()
    {
        Debug.Log("Defense");
    }
    private void OnFire()
    {
        Debug.Log("Fire");
    }
}
