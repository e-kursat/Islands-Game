using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotateSpeed = 720f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float sprintFov = 70f;
    public float normalFov = 60f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isJumping = false;

    // Animator bileşeni
    private Animator anim;

    // Kamera ve hız kontrolü
    public Camera mainCam;
    public KeyCode sprintButton = KeyCode.LeftShift;
    public KeyCode walkButton = KeyCode.LeftControl;

    private float _inputX;
    private float _inputY;
    private float _maxSpeed = 1f;

    private Vector3 stickDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); // Animator bileşeni
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
        HandleJump();
        ApplyGravity();
        UpdateAnimations();
    }

    // Hareket ve hız kontrolü
    void HandleInput()
    {
        stickDirection = new Vector3(_inputX, 0, _inputY);
        
        if (Input.GetKey(sprintButton)) // Koşma
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, sprintFov, Time.deltaTime * 2);
            _maxSpeed = 2f; // Koşma hızı
            _inputX = 2 * Input.GetAxis("Horizontal");
            _inputY = 2 * Input.GetAxis("Vertical");
        }
        else if (Input.GetKey(walkButton)) // Yavaş yürüme
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFov, Time.deltaTime * 2);
            _maxSpeed = 0.2f; // Yavaş yürüme hızı
            _inputX = Input.GetAxis("Horizontal") / 2;
            _inputY = Input.GetAxis("Vertical") / 2;
        }
        else // Normal yürüme
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFov, Time.deltaTime * 2);
            _maxSpeed = 1f; // Normal yürüme hızı
            _inputX = Input.GetAxis("Horizontal");
            _inputY = Input.GetAxis("Vertical");
        }
    }

    // Karakter hareketini yöneten fonksiyon
    void HandleMovement()
    {
        Vector3 move = transform.forward * (_inputY * moveSpeed * Time.deltaTime);
        controller.Move(move);
    }

    // Karakter dönüşünü yöneten fonksiyon
    void HandleRotation()
    {
        // float rotation = _inputX * rotateSpeed * Time.deltaTime;
        // transform.Rotate(0, rotation, 0);

        if (stickDirection.magnitude > 0.1f)
        {
            Vector3 rotOffset = mainCam.transform.TransformDirection(stickDirection);
            rotOffset.y = 0;

            transform.forward = Vector3.Slerp(transform.forward, rotOffset, Time.deltaTime * rotateSpeed);
        }
    }

    // Zıplama işlevini yöneten fonksiyon
    void HandleJump()
    {
        if (controller.isGrounded && !isJumping)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isJumping = true;
            }
        }

        if (controller.isGrounded && isJumping)
        {
            isJumping = false;
        }
    }

    // Yerçekimi uygulayan fonksiyon
    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Animasyon güncellemesi
    void UpdateAnimations()
    {
        anim.SetFloat("Speed", Vector3.ClampMagnitude(stickDirection, _maxSpeed).magnitude, 0.1f, Time.deltaTime * 10);
    }
}
