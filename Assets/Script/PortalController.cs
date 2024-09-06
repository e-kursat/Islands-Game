using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform targetLocation; // Hedef adanın pozisyonu

    private void OnTriggerEnter(Collider other)
    {
        // Eğer oyuncu portala girdiyse
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            other.transform.position = targetLocation.position;
            other.transform.rotation = targetLocation.rotation;
            other.gameObject.SetActive(true);
        }
    }
}
