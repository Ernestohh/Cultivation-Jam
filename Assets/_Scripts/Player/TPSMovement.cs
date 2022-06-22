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
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple DetectableTargetManager found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        AssignAnimationIDs();
        //keyTargetRotation.eulerAngles = new Vector3(keyTargetRotation.x, 10000f, keyTargetRotation.z);
        //key_spring.target_state = 1f;
    }
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    // Update is called once per frame
    void Update()
    {
        if (canControlPlayer)
        {
            Move();
        }

        //RotateKey();

    }

    
    Quaternion keyTargetRotation;
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
        _controller.Move(moveDir.normalized * _speed /*walkSpeed*/ * Time.deltaTime);

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
}
