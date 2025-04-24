using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Player Movement Variables")]
    public float maxHealth = 100;
    public float health = 100;
    public float playerSpeed = 10f;
    public float rotationSpeed = 10.0f;
    public float SpeedModifier = 1f;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float camUp;
    [SerializeField] private float camsides;
    [SerializeField] private float camUpDown;
    [SerializeField] private float doubleJumpMult = 1.5f;
    [SerializeField] private float groundDistance = 0.5f;

    [Header("Gravity Settings")]
    public float gravity = 9.81f;
    public float fallMultiplier =9.81f;
    public float jumpHeight = 2f;
    private Vector3 velocity;
    public bool isGrounded;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool isCrouching;
    [SerializeField] private bool isSprinting;

    [Header("Input Variables")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;

    [Header("Player Elements")]
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Coroutine slidingRoutine;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Canvas uicanvas;
    public GameObject weaponsPawnpoint;
    public WeaponSlot[] weapons;
    public int weaponIndex =0;
    public Weapon currentWeapon;

    [Header("UIElements")]
    public Image healthbar;
    public TextMeshProUGUI HealthTx;
    
    public Transform groundChecker;
    public GameObject cameraRotator;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }
    //movement is relegated to regular update, so platforming and attaching works
    void Update()
    {
        ViewControls();
        MovePlayer();
        MovementReset();
    }

    // Fixed update avoid fame skipping everything that happens in fixed update supercedes update
    void FixedUpdate()
    {
        GroundCheck();
    }
    //********************* Input Related Methods**********************///
    public void OnMove(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();

    }

    public void OnLook(InputAction.CallbackContext value)
    {
        lookInput = value.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                if (isCrouching)
                {
                    playerController.height = 2;
                    SpeedModifier = 1f;
                    isCrouching = false;
                }
                Jump();
            }
            else if (canDoubleJump)
            {
                DoubleJump();
            }
        }




    }
   
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentWeapon != null)
            {
                
                currentWeapon.Fire();
             
            }
        }

        if (context.canceled)
        {
            currentWeapon.isFiring = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //if it's not crouching and the movement input is not zero, this ensures the sliding only happens if the character is moving
            if (!isCrouching && moveInput != Vector2.zero)
            {
                //if is not sprinting, sprint
                if (!isSprinting)
                {
                    SpeedModifier = 1.5f;
                    isSprinting = true;
                }
                //of if it is, stop sprinting
                if (moveInput == Vector2.zero)
                {

                    SpeedModifier = 1f;
                    isSprinting = false;
                }

            }
           
           


        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    { if (context.started)
        {
            //check if it is not crouching already
            if (!isCrouching)
            {
                //check if it is sprinting but not sliding
                if (isSprinting && slidingRoutine==null)
                {
                    //if is sprinting but not sliding, slide.
                    slidingRoutine = StartCoroutine(Slide());
                }
                //for other stuff, basically if it is crouching
                else 
                {
                    //if the sliding routine is not null
                    if ( slidingRoutine !=null)
                    {
                       //do nothing, or we can implement something later also this ensures the sliding is not interrupted
                    }

                    else
                    
                    {
                       //this is the actual crouching, math lerp ensures smooth sliding 
                        playerController.height = Mathf.Lerp(playerController.height, 1, 1);
                        SpeedModifier = 0.3f;
                        isCrouching = true;
                    }
                    
                }
                
            }
            //this is when it is already crouching, we return to our normal height
           else
            {
                playerController.height = Mathf.Lerp(playerController.height, 2, 1);
                SpeedModifier = 1f;
                isCrouching = false;
            }
        }
    }


    public void OnWeaponIndex(InputAction.CallbackContext context)
    {
        // If the input wasn't performed (i.e., canceled or idle), do nothing
        if (!context.performed) return;

        // Read the scroll input: positive = scroll up, negative = scroll down
        float scroll = context.ReadValue<float>();
        int direction = scroll > 0 ? 1 : -1;

        // Store the original index to detect if we’ve looped all the way around
        int originalIndex = weaponIndex;

        // This will be used to step through the weapon list
        int newIndex = weaponIndex;

        // Safety counter so we don't loop infinitely
        int attempts = 0;

        // Begin checking for the next available unlocked weapon
        do
        {
            // Move to the next index in the given scroll direction
            newIndex += direction;

            // Wrap around if we go past the array boundaries
            if (newIndex >= weapons.Length) newIndex = 0;
            if (newIndex < 0) newIndex = weapons.Length - 1;

            attempts++;

            // DEBUG: Show which weapon we're checking and whether it's unlocked
            Debug.Log($"Checking weapon index {newIndex}, isUnlocked: {weapons[newIndex].isUnlocked}");

            // If the weapon is unlocked, select it
            if (weapons[newIndex].isUnlocked)
            {
                weaponIndex = newIndex;
                ReplaceWeapon(weaponIndex); // Call your regular weapon replacement method
                return; // Stop searching once a valid weapon is found
            }

        } while (attempts < weapons.Length); // Keep looping until we’ve checked them all

        // If no unlocked weapons are found, log a message (optional)
        Debug.Log("No unlocked weapons found.");
    }

    public void MovePlayer()
    {
        // Get input for movement
        Vector3 move;

        // Default movement for normal orientation
        move = (transform.right * moveInput.x + transform.forward * moveInput.y);

        // Move the player
        playerController.Move(move * playerSpeed * Time.deltaTime * SpeedModifier);

        // Apply gravity with fall multiplier when not grounded
        if (!isGrounded)
        {
            velocity += -transform.up * gravity * fallMultiplier * Time.deltaTime;
        }
        else
        {
            velocity += -transform.up * gravity * Time.deltaTime; // Normal gravity when grounded
        }
        // Move the player based on velocity
        playerController.Move(velocity * Time.deltaTime);

    }
   
    private void ViewControls()
    {
        //read the values from the input
        rotationAmount = lookInput.x * rotationSpeed * Time.deltaTime;
        //convert it to a float so it can affect rotation
        camUp = lookInput.y * rotationSpeed * Time.deltaTime;
        // Update and clamp cumulative Y-axis rotation
        camsides += rotationAmount;
        camUpDown -= camUp;
        camUpDown = Mathf.Clamp(camUpDown, -35f, 70f);
        //Rotate the camera up and down
        cameraRotator.transform.localRotation = Quaternion.Euler(camUpDown, 0, 0);
        //rotate the player object
        transform.localRotation *= Quaternion.Euler(0f, rotationAmount, 0f);

    }
    //************** Actual methods called by inputs these are private ************************///
 
    private void Jump()
    {
        // Apply jump velocity relative to the player's local up
        velocity = transform.up * Mathf.Sqrt(jumpHeight * 2f * gravity);


    }

    private void DoubleJump()
    {
        // Apply double jump velocity relative to the player's local up
        velocity = transform.up * Mathf.Sqrt(jumpHeight * doubleJumpMult * 2f * gravity);
        canDoubleJump = false;
    }


   private IEnumerator Slide()
    {
        float initialSlideSpeed = 3f;//initial speed boost for slide
        float finalSlideSpeed = 0.5f;//final "Winddown" speed
        float slideDuration = 1f; //full slide duration
        float elapsedTime = 0f; //time passed during the coroutine
        float heightDifference; //to calculate what is missing from the sliderheight
        float standingHeight = 2f; //final target probably reduntant but hey redundancy is key
        float standingbackDuration = 0.15f; //time it takes to be fully back up
        float standingSpeed;
        
        //quickly set the minimum slide height so player can squeeze through things
        playerController.height = 1f;

        //actual sliding coroutine
        while (elapsedTime < slideDuration) 
        {
            //add time to the sliding timer
            elapsedTime += Time.deltaTime;
            // Gradually reduce speed
            SpeedModifier = Mathf.Lerp(initialSlideSpeed, finalSlideSpeed, elapsedTime / slideDuration);
            //wait till next frame to do over
            yield return null;

        }
        //calculate the height difference between current sliding height of character controller and full standing height
        heightDifference = standingHeight - playerController.height;
        //smoothly divide the difference between how fast we want the character to stand up
        standingSpeed =  heightDifference/standingbackDuration;
        
        //after slide player slowly backs up
        while (playerController.height < standingHeight)
        {
            playerController.height += standingSpeed*Time.deltaTime;

            yield return null;
        }
        //reset all stats, redundancy for full height
        playerController.height = 2f;
        SpeedModifier = 1f;
        isSprinting = false;
        isCrouching = false;
        slidingRoutine = null;
    }

    ///////Below are Funtioning methods for update controlled stuff///////
    private void Initialize()
    {
        health = maxHealth;
        healthbar = GameObject.Find("HealthBar").GetComponent<Image>();
        HealthTx = GameObject.Find("HealthUI").GetComponent<TextMeshProUGUI>();
        HealthTx.text = health.ToString();
        playerController = GetComponent<CharacterController>();
        uicanvas = UnityEngine.Object.FindFirstObjectByType<Canvas>();
        ReplaceWeapon(0);
        AddCams();
        
    }

    

    public void ReplaceWeapon(int index)
    {
        Transform parent = weaponsPawnpoint.transform;
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
              
        GameObject newWeapon = Instantiate(weapons[weaponIndex].weaponPrefab, parent.position, parent.rotation, parent);
        currentWeapon = newWeapon.GetComponent<Weapon>();
        newWeapon.SetActive(true);
        
    }

    private void GroundCheck()
    {
        // Check if the player is grounded based on the player's local down
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        if (isGrounded && Vector3.Dot(velocity, -transform.up) > 0)
        {
            velocity = -transform.up * 2f; // Reset velocity relative to local down
            canDoubleJump = true;
        }

    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (healthbar != null)
        {
            healthbar.fillAmount = health / maxHealth;
        }
        HealthTx.text = health.ToString();
        if (health <= 0)
        {
            Debug.Log(" player has been destroyed");
            PlayerUIBehavior playerUi = uicanvas.gameObject.GetComponent<PlayerUIBehavior>();
            Destroy(weaponHolder.gameObject);
            cameraRotator.transform.parent = null;
            playerUi.Death();

            //this will destroy the player
            Destroy(gameObject);
        }
    }


    ///************** Visualization stuff******************   /// 
    private void OnDrawGizmos()
    {
        if (groundChecker != null)
        {
            // Set the Gizmo color (use different colors based on isGrounded state for clarity)
            Gizmos.color = isGrounded ? Color.green : Color.red;

            // Draw a sphere to visualize the ground check
            Gizmos.DrawWireSphere(groundChecker.position, groundDistance);
        }
    }

    private void MovementReset()
    {
        if (moveInput == Vector2.zero && !isCrouching)
        {
            isSprinting = false;
            SpeedModifier = 1f;

            
        }


    }
    private void AddCams()
    {
        //find the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }
        //define additional camera data with the one in the main camera
        UniversalAdditionalCameraData mainCamData = mainCamera.GetComponent<UniversalAdditionalCameraData>();

        if (mainCamData == null)
        {
            Debug.LogError("Main Camera does not have UniversalAdditionalCameraData!");
            return;
        }

        // Find all cameras in the scene
        Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        //for each camera found in the array do a check
        foreach (Camera cam in allCameras)
        {
            //if it is the main camera, ignore and continue
            if (cam == mainCamera) continue; 

            //get its UACD
            UniversalAdditionalCameraData camData = cam.GetComponent<UniversalAdditionalCameraData>();
            //if that is not null and if its rendertype is overlay....
            if (camData != null && camData.renderType == CameraRenderType.Overlay)
            {
                //add it to the main camera stack
                if (!mainCamData.cameraStack.Contains(cam))
                {
                    mainCamData.cameraStack.Add(cam);
                    Debug.Log($"Added {cam.name} to Main Camera Stack");
                }
            }
        }
    }



}
