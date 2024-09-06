using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    [Header("Metrics")]
    public float damp = 10.0f;
    [Range(1, 20)]
    public float rotationSpeed = 5.0f;

    private float normalFov;
    public float sprintFov = 70.0f;

    private float _inputX;
    private float _inputY;
    private float _maxSpeed;

    public Transform Model;
    private Animator Anim;
    private CharacterController _controller;
    private Vector3 StickDirection;
    private Camera mainCam;

    public KeyCode sprintButton = KeyCode.LeftShift;
    public KeyCode walkButton = KeyCode.LeftControl;
    public KeyCode jumpButton = KeyCode.Space;  // Zıplama tuşu
    
    [Header("Jump Settings")]
    public float jumpHeight = 5.0f;
    private float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount = 0;  // Çift zıplama için sayaç
    public int maxJumpCount = 5;  // Maksimum zıplama sayısı (çift zıplama)

    static public bool dialogue = false;

    // Start is called before the first frame update
    void Awake()
    {
        Model = GetComponent<Transform>();
        Anim = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        mainCam = Camera.main;

        normalFov = mainCam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        Anim.speed = moveSpeed;
    }

    void LateUpdate()
    {
        if (!dialogue)
        {
            HandleGravity();
            HandleJumpInput();
            InputMove();
            InputRotation();
            Movement();
        }
        else
        {
            // Karakterin tüm hareketlerini ve animasyonunu durdur
            _inputX = 0;
            _inputY = 0;
            StickDirection = Vector3.zero;
            
            //velocity = Vector3.zero;
            Anim.SetFloat("Speed", 0f);  // Animasyon hızını sıfırla

            // Karakterin hareket etmesini durdur
            //_controller.Move(Vector3.zero);

            HandleGravity();
        }
    }

    void Movement()
    {
        StickDirection = new Vector3(_inputX, 0, _inputY);

        if (Input.GetKey(sprintButton))
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, sprintFov, Time.deltaTime * 2);

            _maxSpeed = 2f;

            _inputX = 2 * Input.GetAxis("Horizontal");
            _inputY = 2 * Input.GetAxis("Vertical");
        }
        else if (Input.GetKey(walkButton))
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFov, Time.deltaTime * 2);

            _maxSpeed = 0.2f;

            _inputX = Input.GetAxis("Horizontal");
            _inputY = Input.GetAxis("Vertical");
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFov, Time.deltaTime * 2);

            _maxSpeed = 1f;

            _inputX = Input.GetAxis("Horizontal");
            _inputY = Input.GetAxis("Vertical");
        }
    }

    void InputMove()
    {
        Anim.SetFloat("Speed", Vector3.ClampMagnitude(StickDirection, _maxSpeed).magnitude, damp, Time.deltaTime * 10);
    }

    void InputRotation()
    {
        Vector3 rotOffset = mainCam.transform.TransformDirection(StickDirection);
        rotOffset.y = 0;

        // if (velocity.y < 0)
        // {
        //     velocity.y = 0f;
        // }
        
        Model.forward = Vector3.Slerp(Model.forward, rotOffset, Time.deltaTime * rotationSpeed);
    }

    // Yer çekimini ve zıplama için eklenen kuvveti ele alan fonksiyon
    void HandleGravity()
    {
        // Yerde olup olmadığını kontrol et
        isGrounded = _controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f * -2.0f;  // Hafif yere çekme
            jumpCount = 0;  // Yere değdiğinde çift zıplama hakkını sıfırla
        }

        // Yer çekimi uygula
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }

    // Zıplama girdisini işleyen fonksiyon
    void HandleJumpInput()
    {
        if (Input.GetKeyDown(jumpButton) && jumpCount < maxJumpCount)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);  // Zıplama hızı
            jumpCount++;  // Zıplama sayısını artır
        }
        
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }
}
