using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Player Settings
    [Header("Movement Options")]
    public float MovementSpeed;
    public float WalkSpeed = 3f;
    public float RunSpeed = 8f;
    public float JumpHeight = 5f;
    public float JumpCoolDown = 0.25f;
    public float GroundDrag = 0f;
    public float AirMultiplier = 0f;
    public float PlayerHeight = 1f;
    [Space]
    [Header("Crawling Options")]
    public float CrawlSpeed = 1.5f;
    public float CrawlYScale = 0.5f;
    public float StartYScale;
    [Space]
    [Header("Movement Settings")]
    public KeyCode Jump = KeyCode.Space;
    public KeyCode Run = KeyCode.LeftShift;
    public KeyCode Crawl = KeyCode.LeftControl;
    [Space]
    //Object Connections
    [Header("Connections")]
    public Transform Orientation;
    public LayerMask Ground;
    [Space]
    //Slope Movement Settings
    [Header("Slope Settings")]
    public float MaxSlopeAngle = 40f;
    private RaycastHit SlopeHit;
    [Space]
    //Stamina
    [Header("Stamina Settings")]
    public Stamina stamina;
    public Image staminaFillImage;
    public float staminaDecreasePerSecond = 10f;
    public float staminaIncreasePerSecond = 1.5f;
    [Space]
    //Debuging And Helpful Tools
    [Header("DEBUG")]
    public float RayCastLength = 1.2f;
    public float RayCastStartPosition = -1f;
    //References
    private Rigidbody BearBody;
    float HorizontalInput;
    float VerticalInput;
    Vector3 MovementDirection;
    public MovementState State;
    //Enums
    public enum MovementState
    {
        Walking,
        Crawling,
        Running,
        Air,
    }

    //Bools
    bool Grounded;
    bool ReadyToJump;
    bool ExitingSlope;
    private void Start()
    {
        stamina = new Stamina(100); // Example max stamina value
        BearBody = GetComponent<Rigidbody>();
        BearBody.freezeRotation = true;
        ReadyToJump = true;
        StartYScale = transform.localScale.y;
    }




    private void Update()
    {
        //Raycast Origin
        Vector3 rayOrigin = transform.position + Vector3.down * RayCastStartPosition;
        //Ground Check With Raycast
        Grounded = Physics.Raycast(rayOrigin, Vector3.down, PlayerHeight * RayCastLength, Ground);
        //Draw Raycast
        Debug.DrawRay(rayOrigin, Vector3.down * (PlayerHeight * RayCastLength), Color.red);



        BearInput();
        BearWalkSpeed();
        StateHandler();


        //Handle Drag
        if (Grounded)
        {
            BearBody.linearDamping = GroundDrag;
        }
        else
        {
            BearBody.linearDamping = 0f;
        }

        if (State == MovementState.Running)
        {
            stamina.DecreaseStamina(staminaDecreasePerSecond * Time.deltaTime);
        }
        else
        {
            stamina.IncreaseStamina(staminaIncreasePerSecond * Time.deltaTime);
        }

        if (staminaFillImage != null)
        {
            staminaFillImage.fillAmount = stamina.CurrentStamina / stamina.MaxStamina;
        }

    }

    private void FixedUpdate()
    {
        BearMovement();
    }
    private void BearInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        //When To Jump
        if (Input.GetKey(Jump) && ReadyToJump && Grounded && stamina.CurrentStamina > 10f)
        {
            ReadyToJump = false;
            BearJump();
            stamina.DecreaseStamina(15f);
            Invoke(nameof(ResetJump), JumpCoolDown);
        }

        //Start Crawling
        if (Input.GetKeyDown(Crawl))
        {
            transform.localScale = new Vector3(transform.localScale.x, CrawlYScale, transform.localScale.z);
            BearBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        //Stop Crawling
        if (Input.GetKeyUp(Crawl))
        {
            transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
        }
    }


    private void BearMovement()
    {
        //Calculate Movement Direction
        MovementDirection = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;

        //On Slope
        if (OnSlope())
        {
            BearBody.AddForce(GetSlopeMovementDirection() * MovementSpeed * 20f, ForceMode.Force);

            if (BearBody.linearVelocity.y > 0)
            {
                BearBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

        }

        //On The Ground
        else if (Grounded)
        {
            BearBody.AddForce(MovementDirection.normalized * MovementSpeed * 5f, ForceMode.Force);
        }
        //In The Air
        else if (!Grounded)
        {
            BearBody.AddForce(MovementDirection.normalized * MovementSpeed * 5f * AirMultiplier, ForceMode.Force);
        }
        //Turn Off Gravity On Slopes
        BearBody.useGravity = !OnSlope();

    }

    private void BearWalkSpeed()
    {
        //Slope Speed Limit
        if (OnSlope() && !ExitingSlope)
        {
            if (BearBody.linearVelocity.magnitude > MovementSpeed)
                BearBody.linearVelocity = BearBody.linearVelocity.normalized * MovementSpeed;
        }
        //limiting speed on ground
        else
        {

            Vector3 flatvel = new Vector3(BearBody.linearVelocity.x, 0f, BearBody.linearVelocity.z);

            if (flatvel.magnitude > MovementSpeed)
            {
                Vector3 limitedVel = flatvel.normalized * MovementSpeed;
                BearBody.linearVelocity = new Vector3(limitedVel.x, BearBody.linearVelocity.y, limitedVel.z);
            }

        }

    }

    private void BearJump()
    {

        ExitingSlope = true;

        //Reset Y Velocity
        BearBody.linearVelocity = new Vector3(BearBody.linearVelocity.x, 0f, BearBody.linearVelocity.z);

        BearBody.AddForce(transform.up * JumpHeight, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        ReadyToJump = true;
        ExitingSlope = false;
    }

    private bool OnSlope()
    {
        //Raycast Origin
        Vector3 rayOrigin = transform.position + Vector3.down * RayCastStartPosition;
        //Slope Check Raycast
        if (Physics.Raycast(rayOrigin, Vector3.down, out SlopeHit, PlayerHeight * RayCastLength))

        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return angle < MaxSlopeAngle && angle != 0;
        }

        else return false;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(MovementDirection, SlopeHit.normal).normalized;
    }

    private void StateHandler()
    {
        //Mode Crawling
        if (Input.GetKey(Crawl))
        {
            MovementSpeed = CrawlSpeed;
        }
        //Mode Sprinting
        else if (Grounded && Input.GetKey(Run) && stamina.CurrentStamina > 0f)
        {
            State = MovementState.Running;
            MovementSpeed = RunSpeed;
        }
        //Mode Walking
        else if (Grounded)
        {
            State = MovementState.Walking;
            MovementSpeed = WalkSpeed;
        }
        //Mode Air
        else
        {
            State = MovementState.Air;
        }

    }
    public class Stamina
    {
        public float CurrentStamina { get; private set; }
        public float MaxStamina { get; private set; }

        public Stamina(float maxStamina)
        {
            MaxStamina = maxStamina;
            CurrentStamina = maxStamina;
        }

        public void DecreaseStamina(float amount)
        {
            CurrentStamina = Mathf.Max(CurrentStamina - amount, 0);
        }

        public void IncreaseStamina(float amount)
        {
            CurrentStamina = Mathf.Min(CurrentStamina + amount, MaxStamina);
        }
    }

    //To Help Animator
    public bool IsGrounded()
    {
        return Grounded;
    }

    public bool HasEnoughStamina(float amount = 0f)
    {
        return stamina.CurrentStamina >= amount;
    }
   
}