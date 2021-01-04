using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public int playerIndex = 0;

    [Header("HP Values")]
    [SerializeField] int maxHP = 10;
    private int HP = 10;

    [Header("Dash Values")]
    [SerializeField] float dashAmount = 20;
    [SerializeField] float maxDashTime = 1;
    [SerializeField] float dashCooldown = 1;

    [Header("Speed Values")]
    [SerializeField] float runningSpeed = 10;
    [SerializeField] float attackingSpeed = 5;
    [SerializeField] float takingDamageSpeed = 5;
    [SerializeField] float gravityAmount = 1;

    [Header("Various Values")]
    [SerializeField] float groundCheckDistance = 5;
    [SerializeField] float rotationSmoothingAmount = 0.01f;
    [SerializeField] float attackStampMax = 1f;
    private float currentStamp = 0f;
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float meleeCooldown = 0.1f;
    [SerializeField] LayerMask attackLayers;
    [SerializeField] LayerMask dashLayer;

    private Vector2 inputMovement;
    private Vector2 inputRot;
    private Vector3 DesiredRotation;


    [Header("Components")]
    [SerializeField] Transform mesh;
    [SerializeField] Animator animator;
    public Weapon currentWeapon = null;
    private Rigidbody rb;

    [Header("Object reference")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject weaponMesh;
    [SerializeField] LineRenderer laserSight;
    [SerializeField] TrailRenderer dashTrail;
    [SerializeField] GameObject throwableWeaponPrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform meleePoint;
    [SerializeField] Transform rightHandle;
    [SerializeField] Transform leftHandle;
    [SerializeField] Rig rigConstraint;

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
    private bool canDash = true;
    private bool canMelee = true;

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
            animator.SetFloat("XSpeed", inputMovement.x);
            animator.SetFloat("YSpeed", inputMovement.y);
        }

        else
        {
            rb.velocity = new Vector3(DesiredPosition.x * movementSpeed, rb.velocity.y-gravityAmount, DesiredPosition.z * movementSpeed);
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
        animator.SetBool("Grounded", isGrounded);
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

            case PlayerStates.dashing:
                HandleRotation();
                break;
        }

        //Stamp fighting setting
        if (currentStamp > 0)
            currentStamp -= Time.deltaTime;

        else if (currentState != PlayerStates.running)
            SetCurrentState(PlayerStates.running,0);
    }
    private void SetCurrentState(PlayerStates state, float timeToRecover)
    {
        currentState = state;
        
        if(timeToRecover>0)
            currentStamp = timeToRecover;
    }


    //Change current weapon method
    public void ChangeWeapon (Weapon weaponToPick)
    {
        if(weaponToPick != null)
        {
            currentWeapon = weaponToPick;
            laserSight.enabled = true;
            weaponMesh.GetComponent<MeshFilter>().mesh = weaponToPick.weaponMesh as Mesh;
            weaponMesh.GetComponent<MeshRenderer>().material = weaponToPick.weaponMaterial as Material;
            firePoint.transform.localPosition = currentWeapon.firePointOffset;
            laserSight.gameObject.transform.localPosition = currentWeapon.firePointOffset;
            rightHandle.localPosition = weaponToPick.rightHandlePositionOffset;
            rightHandle.rotation.SetEulerAngles(weaponToPick.rightHandleRotationOffset);
            leftHandle.localPosition = weaponToPick.leftHandlePositionOffset;
            leftHandle.rotation.SetEulerAngles(weaponToPick.lefttHandleRotationOffset);
            currentWeapon.Reload();
            rigConstraint.weight = 1;
        }

        else
        {
            currentWeapon = null;
            laserSight.enabled = false;
            weaponMesh.GetComponent<MeshFilter>().mesh = null;
            weaponMesh.GetComponent<MeshRenderer>().material = null;
            rigConstraint.weight = 0;
        }
    }


    //Input getting
    private void OnMovement(InputValue value) => inputMovement = value.Get<Vector2>();
    private void OnAiming(InputValue value)
    {
        inputRot = value.Get<Vector2>();
    }

    //Input press
    private void OnDefense()
    {
        Debug.Log("Defense");
        SetCurrentState(PlayerStates.defending,attackStampMax);
    }
    private void OnFire()
    {
        if (currentWeapon != null && currentWeapon.ammo > 0 && currentWeapon.canFire)
        {
            StartCoroutine(currentWeapon.Fire(firePoint,gameManager));
            SetCurrentState(PlayerStates.attacking,attackStampMax);
        }

        else if (currentWeapon == null)
            Debug.Log("No weapon equiped");

        else if (currentWeapon.ammo > 0 && currentWeapon.canFire)
            Debug.Log("No ammo");
    }
    private void OnDash()
    {
        if (canDash)
        {
            Debug.Log("Dash");
            SetCurrentState(PlayerStates.dashing, maxDashTime);

            Vector3 moveDirection = new Vector3(inputMovement.x, 0, inputMovement.y).normalized * dashAmount;
            if (moveDirection == Vector3.zero)
                moveDirection = mesh.transform.forward*dashAmount;
        
            Vector3 lastPosition = transform.position;
            Vector3 nextPosition;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, moveDirection, out hit) && Vector3.Distance(hit.point, transform.position) < dashAmount)
                nextPosition = hit.point - (mesh.transform.forward * 0.5f);

            else
                nextPosition = transform.position + moveDirection;

            StartCoroutine(Dash(lastPosition, nextPosition));
        }
    }
    private void OnMelee()
    {
        SetCurrentState(PlayerStates.attacking,attackStampMax);

        if (currentWeapon != null)
            DropWeapon(true);

        else if(canMelee)
        {
            Debug.Log("Melee");
            StartCoroutine(MeleeAttack());
        }
    }


    private IEnumerator Dash(Vector3 destination, Vector3 origin)
    {
        canDash = false;
        float dashTime = maxDashTime;
        dashTrail.emitting = true;

        while (dashTime > 0)
        {
            float dashProgress = dashTime / maxDashTime;
            transform.position = Vector3.Lerp(origin, destination, dashProgress);
            dashTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        dashTrail.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private IEnumerator MeleeAttack()
    {
        canMelee = false;

        animator.SetTrigger("Melee");

        Collider[] targetHits;
        targetHits = Physics.OverlapSphere(meleePoint.position, attackRadius, attackLayers);

        foreach (Collider targetHit in targetHits)
        {
            targetHit.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController);
            if (playerController != null)
            {
                Debug.Log(playerController.gameObject.name + " hit");
                playerController.TakeDamage(attackDamage);
                StartCoroutine(HitStop(0.2f,0.2f));
            }
        }

        yield return new WaitForSeconds(meleeCooldown);

        canMelee = true;
    }
    public void TakeDamage(int damageToTake)
    {
        HP -= damageToTake;
        animator.SetTrigger("GetHit");
        StartCoroutine(HitStop(0.5f, 0.2f));

        if (HP <= 0)
        {
            if (currentWeapon != null && currentWeapon.numberOfUsage >= 1)
                DropWeapon(false);

            Debug.Log(gameObject.name + " is dead");
            gameManager.RemovePlayer(transform);
            Destroy(gameObject);
        }

        else
            SetCurrentState(PlayerStates.takingDamage, attackStampMax);
    }
    private void DropWeapon(bool isThrown)
    {
        GameObject thrownWeapon = (GameObject)Instantiate(throwableWeaponPrefab);
        ThrowableWeapon thrownWeaponComponent = thrownWeapon.GetComponent<ThrowableWeapon>();

        thrownWeaponComponent.thrownWeapon = currentWeapon;
        thrownWeaponComponent.transform.position = throwPoint.position;
        thrownWeaponComponent.transform.rotation = throwPoint.rotation;
        thrownWeaponComponent.isThrown = isThrown;
        
        if(isThrown)
            animator.SetTrigger("Throw");

        currentWeapon.ReduceUsage();

        ChangeWeapon(null);
    }

    private IEnumerator HitStop(float duration, float force)
    {
        yield return new WaitForSeconds(0.2f);
        animator.speed = force;
        yield return new WaitForSeconds(duration);
        animator.speed = 1;
    }


    //Draw various things on Gizmo
    private void OnDrawGizmos()
    {
        Vector3 movementEnd = new Vector3(transform.position.x + inputMovement.x*2, transform.position.y, transform.position.z + inputMovement.y*2);
        Vector3 lookEnd = new Vector3(transform.position.x + inputRot.x*4, transform.position.y, transform.position.z + inputRot.y*4);
        Gizmos.DrawLine(transform.position, movementEnd);
        Gizmos.DrawLine(transform.position, lookEnd);

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
