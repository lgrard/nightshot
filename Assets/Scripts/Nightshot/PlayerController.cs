using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] int maxHP = 10;
    private int HP = 10;
    [SerializeField] float runningSpeed = 10;
    [SerializeField] float attackingSpeed = 5;
    [SerializeField] float takingDamageSpeed = 5;
    [SerializeField] float groundCheckDistance = 5;
    [SerializeField] float rotationSmoothingAmount = 0.01f;
    [SerializeField] float attackStampMax = 1f;
    private float currentStamp = 0f;
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] LayerMask attackLayers;

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
    [SerializeField] Transform meleePoint;

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

    [Header("Preview zone and points")]
    [Tooltip("Red = Melee box, Green = Throw point, Blue = Fire point")]
    [SerializeField] bool displayBoxes;


    #region Initialize
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        inputMovement = Vector2.zero;
        inputRot = Vector2.zero;
        HP = maxHP;
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


    //State machine methods
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


    //Change current weapon method
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

    //Input press
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
            DropWeapon(true);

        else
        {
            Debug.Log("Melee");
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        Collider[] targetHits;
        targetHits = Physics.OverlapSphere(meleePoint.position, attackRadius, attackLayers);

        foreach (Collider targetHit in targetHits)
        {
            targetHit.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
            if (playerController != null)
            {
                Debug.Log(playerController.gameObject.name + " hit");
                playerController.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(int damageToTake)
    {
        HP -= damageToTake;

        if (HP <= 0)
        {
            if (currentWeapon != null && currentWeapon.numberOfUsage >= 1)
                DropWeapon(false);

            Debug.Log(gameObject.name + " is dead");
            gameManager.RemovePlayer(transform);
            Destroy(gameObject);
        }
    }

    private void DropWeapon(bool isThrown)
    {
        GameObject thrownWeapon = (GameObject)Instantiate(throwableWeaponPrefab);
        ThrowableWeapon thrownWeaponComponent = thrownWeapon.GetComponent<ThrowableWeapon>();

        thrownWeaponComponent.thrownWeapon = currentWeapon;
        thrownWeaponComponent.transform.position = throwPoint.position;
        thrownWeaponComponent.transform.rotation = throwPoint.rotation;
        thrownWeaponComponent.isThrown = isThrown;

        currentWeapon.ReduceUsage();

        ChangeWeapon(null);
    }


    //Draw various things on Gizmo
    private void OnDrawGizmos()
    {
        if (displayBoxes)
        {

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(throwPoint.position, 0.1f); //Player throw point

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(firePoint.position, 0.1f); //Player fire point

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleePoint.position, attackRadius); //Player fire point
        }
    }
}
