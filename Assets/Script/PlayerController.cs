using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _controller;
    private Animator _anim;
    private Vector3 _direction;
    
    //[SerializeField] private float speed = 5.0f;
    [SerializeField] private float damp = 3.0f;

    [SerializeField] private Movement movement;

    [SerializeField] private float rotationSpeed = 500.0f;
    private Camera _mainCamera;
    
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float jumpPower = 4.0f;
    private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;
    
    void Awake() 
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    // Yerçekimi uygular.
    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        
        _direction.y = _velocity;
    }

    // Dönme uygular.
    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        _direction = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_input.x, 0.0f, _input.y);

        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        // var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        // transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    // Hareket uygular.
    private void ApplyMovement()
    {
        var targetSpeed = movement.isSprinting ? movement.speed * movement.multiplier : movement.speed;
        movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);
        
        _controller.Move(_direction * (movement.currentSpeed * Time.deltaTime));

        _anim.SetFloat("Speed", Vector3.ClampMagnitude(_direction, 2).magnitude, damp, Time.deltaTime * 10);
        //_anim.SetFloat("Speed", movement.currentSpeed);
        // UpdateAnimator();
    }
    
    private void UpdateAnimator()
    {
        // // Hareket hızına göre animator parametrelerini güncelle
        // float movementSpeed = new Vector3(_input.x, 0.0f, _input.y).magnitude;
        //
        // // Hızı maxSpeed ve minSpeed aralığına clamp'le
        // float clampedSpeed = Mathf.Clamp(movementSpeed * movement.speed, 0f, 2f);
        //
        // // Speed parametresini blend tree'e gönder
        // _anim.SetFloat("Speed", clampedSpeed);
        
        _anim.SetFloat("Speed", movement.currentSpeed);
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    // Zıplama uygular.
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps)  return;
        if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

        _numberOfJumps++;
        _velocity = jumpPower;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        movement.isSprinting = context.started || context.performed;
    }
    
    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        _numberOfJumps = 0;
    }

    // Karakter zeminde mi kontrol eder.
    private bool IsGrounded() => _controller.isGrounded;
}

[Serializable]
public struct Movement
{
    public float speed;
    public float multiplier;
    public float acceleration;
    
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public float currentSpeed;
}
