using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLook : MonoBehaviour
{
    private Animator anim;
    private Camera mainCam;
    
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtWeight(.6f, .2f, 1.2f, .5f, .5f);
        
        Ray lookAtRay = new Ray(transform.position, mainCam.transform.forward);
        anim.SetLookAtPosition(lookAtRay.GetPoint(25));
        
    }
}
