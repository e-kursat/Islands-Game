using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Image dialogueBox;
    public Image interactionIcon;  // Yeni işaretçi UI elementi

    public string[] lines;
    public float textSpeed;

    private int _index;
    private bool playerDetection = false;
    private bool dialogueActive = false;

    void Start()
    {
        textComponent.text = string.Empty;
        interactionIcon.gameObject.SetActive(false);  // İşaret başlangıçta gizli
    }

    void Update()
    {
        // Oyuncu NPC'ye yaklaşmışsa ve "F" tuşuna basılmadıysa, işareti göster
        if (playerDetection && !dialogueActive)
        {
            interactionIcon.gameObject.SetActive(true);  // Oyuncu yakın, işareti göster
            print("Canvas aktif!");
        }
        else
        {
            interactionIcon.gameObject.SetActive(false);  // Oyuncu uzak, işareti gizle
        }

        // Diyalog tetikleme ve oyuncu hareketini durdurma
        if (playerDetection && Input.GetKeyDown(KeyCode.F) && !dialogueActive && !PlayerMovement.dialogue)
        {
            PlayerMovement.dialogue = true; 
            dialogueActive = true;
            interactionIcon.gameObject.SetActive(false);  // İşaret gizlensin
            StartDialogue();
        }

        if (Input.GetMouseButtonDown(0) && dialogueActive)
        {
            if (textComponent.text == lines[_index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[_index]; // Tam satır yazılıyor
            }
        }
    }

    void StartDialogue()
    {
        dialogueBox.gameObject.SetActive(true);
        _index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        foreach (char c in lines[_index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (_index < lines.Length - 1)
        {
            _index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.gameObject.SetActive(false);
            dialogueActive = false;
            PlayerMovement.dialogue = false; 
            textComponent.text = string.Empty;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerDetection = false;
    }
}
