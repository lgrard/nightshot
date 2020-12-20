using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float runningSpeed = 10;
    [SerializeField] float attackingSpeed = 5;
    [SerializeField] float takingDamageSpeed = 5;
    [SerializeField] float groundCheckDistance = 5;
    [SerializeField] float rotationSmoothingAmount = 0.01f;
    [SerializeField] float attackStampMax = 1f;
    private float currentStamp = 0f;

    private Vector2 inputMovement;
    private Vector2 inputRot;
    private Vector3 DesiredRotation;


    [Header("Components")]
    [SerializeField] MeshRenderer mesh;
    public Weapon currentWeapon = null;
    private Rigidbody rb;

    [Header("Object reference")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject weaponMesh;
    [SerializeField] GameObject throwableWeaponPrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] Transform firePoint;

    [Header("States")]
    [SerializeField] PlayerStates currentState;
    private enum PlayerStates
    {
        running,
        attacking,
        defending,
        takingDamage,
        dying,
        dashing,
    }

    private bool isGrounded;


    #region Initialize
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        inputMovement = Vector2.zero;
        inputRot = Vector2.zero;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        PlayerStateMachine();
        GroundCheck();
    }

    private void HandleMovement(float movementSpeed)
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

        if (inputRot != Vector2.zero || currentStamp <= 0 && inputMovement != Vector2.zero)
        {
            if(inputRot != Vector2.zero)
                DesiredRotation = camForward * inputRot.y + camRight * inputRot.x;
            
            else if(currentStamp <= 0)
                DesiredRotation = camForward * inputMovement.y + camRight * inputMovement.x;

            Quaternion desiredRotation = Quaternion.LookRotation(new Vector3(DesiredRotation.x, 0, DesiredRotation.z));
            mesh.transform.rotation = Quaternion.Slerp(desiredRotation, mesh.transform.rotation, rotationSmoothingAmount);
        }
    }
    private void GroundCheck()
    {
        Ray groundRay = new Ray(gameObject.transform.position, gameObject.transform.up * -1);
        isGrounded = Physics.Raycast(groundRay,groundCheckDistance);
    }

    private void PlayerStateMachine()
    {
        switch (currentState)
        {
            case PlayerStates.running:
                HandleMovement(runningSpeed);
                HandleRotation();
                break;

            case PlayerStates.attacking:
                HandleMovement(attackingSpeed);
                HandleRotation();
                break;

            case PlayerStates.defending:
                HandleMovement(attackingSpeed);
                HandleRotation();
                break;

            case PlayerStates.takingDamage:
                HandleMovement(takingDamageSpeed);
                HandleRotation();
                break;
        }

        //Stamp fighting setting
        if (currentStamp > 0)
            currentStamp -= Time.deltaTime;

        else if (currentState != PlayerStates.running)
            SetCurrentState(PlayerStates.running);
    }

    private void SetCurrentState(PlayerStates state)
    {
        currentState = state;

        if(state == PlayerStates.attacking || state == PlayerStates.defending)
            currentStamp = attackStampMax;
    }

    public void ChangeWeapon (Weapon weaponToPick)
    {
        if(weaponToPick != null)
        {
            currentWeapon = weaponToPick;
            weaponMesh.GetComponent<MeshFilter>().mesh = weaponToPick.weaponMesh as Mesh;
            weaponMesh.GetComponent<MeshRenderer>().material = weaponToPick.weaponMaterial as Material;
            firePoint.transform.localPosition = currentWeapon.firePointOffset;
            currentWeapon.Reload();
        }

        else
        {
            currentWeapon = null;
            weaponMesh.GetComponent<MeshFilter>().mesh = null;
            weaponMesh.GetComponent<MeshRenderer>().material = null;
        }
    }


    //Input getting
    private void OnMovement(InputValue value) => inputMovement = value.Get<Vector2>();
    private void OnAiming(InputValue value) => inputRot = value.Get<Vector2>();


    private void OnDefense()
    {
        Debug.Log("Defense");
        SetCurrentState(PlayerStates.defending);
    }
    private void OnFire()
    {
        if (currentWeapon != null && currentWeapon.ammo > 0 && currentWeapon.canFire)
        {
            StartCoroutine(currentWeapon.Fire(firePoint));
            SetCurrentState(PlayerStates.attacking);
        }

        else if (currentWeapon == null)
            Debug.Log("No weapon equiped");

        else if (currentWeapon.ammo > 0)
            Debug.Log("No ammo");
    }
    private void OnDash()
    {
        Debug.Log("Dash");
        SetCurrentState(PlayerStates.dashing);
    }
    private void OnMelee()
    {
        SetCurrentState(PlayerStates.attacking);

        if (currentWeapon != null)
        {
            GameObject thrownWeapon = (GameObject)Instantiate(throwableWeaponPrefab);
            ThrowableWeapon thrownWeaponComponent = thrownWeapon.GetComponent<ThrowableWeapon>();

            thrownWeaponComponent.thrownWeapon = currentWeapon;
            thrownWeaponComponent.transform.position = throwPoint.position;
            thrownWeaponComponent.transform.rotation = throwPoint.rotation;

            currentWeapon.ReduceUsage();

            ChangeWeapon(null);
        }

        else
        {
            Debug.Log("Melee");
        }
    }
}
