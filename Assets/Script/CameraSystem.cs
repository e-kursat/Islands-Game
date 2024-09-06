using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    // int edgeScrollSize = 20;
    // private bool dragPanMoveActive;
    // private Vector2 last
    
    public Cinemachine.CinemachineFreeLook freeLookCamera;
    public float mouseSensitivity = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) // Sağ tık kontrolü
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            freeLookCamera.m_XAxis.Value += mouseX;
            //freeLookCamera.m_YAxis.Value -= mouseY;
            //freeLookCamera.m_YAxis.Value = Mathf.Clamp(freeLookCamera.m_YAxis.Value - mouseY, -1f, 1f);

        }
    }
}
