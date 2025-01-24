using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandPortalController : MonoBehaviour
{
    public GameObject portal;           // Portal GameObject'i
    private bool isPortalActive = false;

    void Start()
    {
        // Portalı başlangıçta kapalı yapıyor
        if (portal != null)
        {
            portal.SetActive(false);  // Başlangıçta portal kapalı
        }
    }

    // Oyuncu adanın zeminine değdiğinde tetiklenecek
    private void OnTriggerEnter(Collider other)
    {
        // Oyuncu adaya ulaştı mı kontrol ediliyor
        if (other.gameObject.CompareTag("Player") && !isPortalActive)
        {
            ActivatePortal();
        }
    }

    // Portalı açmak ve materyali değiştirmek için kullanılan fonksiyon
    void ActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);  // Portal aktif hale geliyor
            isPortalActive = true;   // Portalın bir kere açıldıktan sonra tekrar tetiklenmemesi için kontrol
        }
    }
}
