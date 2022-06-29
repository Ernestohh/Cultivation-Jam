using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMovement : MonoBehaviour
{
    const float kKeyMoveSpringStrength = 0.1f;
    const float kKeyMoveSpringDamping = 0.001f;
    Spring key_spring = new Spring(0.0f, 0.0f, kKeyMoveSpringStrength, kKeyMoveSpringDamping, false);

    [SerializeField]List<AudioClip> footstepSounds;
    [SerializeField] GameObject keyBehindAutomaton;
    public static TPSMovement Instance = null;
    CharacterController _controller;
    Animator _animator;
    [SerializeField] Transform cam;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5.33f;
    [SerializeField] float turnSmoothTime = 0.1f;
    public float SpeedChangeRate = 10.0f;
    private float _speed;
    private float _animationBlend;
    private float targetAngle = 0.00f;

    float turnSmoothVelocity;
    public bool canControlPlayer = true;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDMotionSpeed;

    #region Fall Jump Gravity 
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;

    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;//didn't understand this

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

            _animator.SetBool(_animIDGrounded, Grounded);
    }
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;
           
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if(Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                _animator.SetBool(_animIDJump, true);
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }

        }
        else
        {
            _jumpTimeoutDelta = JumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _animator.SetBool(_animIDFreeFall, true);
            }
           // _input.jump = false; there is something like this which i didn't included. we will see if it causes any problems
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    #endregion

    private Inventory inventory;
    [SerializeField] UI_Inventory uiInventory;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple DetectableTargetManager found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        uiInventory.SetPlayer(this);
 
    }
    // Start is called before the first frame update
    void Start()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        AssignAnimationIDs();
        //keyTargetRotation.eulerAngles = new Vector3(keyTargetRotation.x, 10000f, keyTargetRotation.z);
        //key_spring.target_state = 1f;

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        ItemWorld.SpawnItemWorld(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 0.5f), new Item { itemType = Item.ItemType.SeedNutriball, amount = 1});
        ItemWorld.SpawnItemWorld(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 0.7f), new Item { itemType = Item.ItemType.SeedNutriball, amount = 1});
        ItemWorld.SpawnItemWorld(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 0.6f), new Item { itemType = Item.ItemType.SeedNutriball, amount = 1});

    }

    // Update is called once per frame
    void Update()
    {
        if (canControlPlayer)
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        //RotateKey();

    }

    
    Quaternion keyTargetRotation;
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }
    void RotateKey()
    {
        key_spring.Update();
        ApplyPoseSpecial(keyTargetRotation, key_spring.state);
        if (key_spring.target_state == 1 && key_spring.state >= 0.95f)
        {
            key_spring.state = 0f;
            keyTargetRotation.eulerAngles = new Vector3(keyTargetRotation.x, Random.Range(-10800f, 10800f), keyTargetRotation.z);
        }
    }
    public void ApplyPoseSpecial(Quaternion poseRotation, float amount)
    {
        if (amount == 0.0f)
        {
            return;
        }
        keyBehindAutomaton.transform.localRotation = mixRot(keyBehindAutomaton.transform.localRotation, poseRotation, amount);
    }
    void Move()
    {
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontal, vertical);
        if (inputVector == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * SpeedChangeRate);
            //Debug.Log("current horizontal speed " + _speed + " target speed " + targetSpeed + " art�� h�z� " + Time.deltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        //if (_animationBlend < 0.01f) _animationBlend = 0f;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
   
        if (inputVector != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _controller.Move(moveDir.normalized * _speed /*walkSpeed*/ * Time.deltaTime +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        _animator.SetFloat(_animIDSpeed, _animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, 1);
    }
    private void OnFootstep(AnimationEvent animationEvent)
    {
        //Debug.Log("weight bu " + animationEvent.animatorClipInfo.weight);
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footstepSounds.Count > 0)
            {
                var index = Random.Range(0, footstepSounds.Count);
                AudioSource.PlayClipAtPoint(footstepSounds[index], transform.TransformPoint(_controller.center), 0.8f * _speed);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }
    public Quaternion mixRot(Quaternion a, Quaternion b, float val)
    {
        float angle = 0.0f;
        Vector3 axis = new Vector3();
        (Quaternion.Inverse(b) * a).ToAngleAxis(out angle, out axis);
        if (angle > 180)
        {
            angle -= 360.0f;
        }
        if (angle < -180)
        {
            angle += 360.0f;
        }
        if (angle == 0)
        {
            return a;
        }
        return a * Quaternion.AngleAxis(angle * -val, axis);
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();

        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            default: 
                PlantSeed(item);
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1});
                break;
        }
    }
    public void PlantSeed(Item item)
    {

    }
}
