using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    private Rigidbody rb; // Referência para o componente Rigidbody do objeto

    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtém o componente Rigidbody do objeto
        rb.isKinematic = true; // Define o Rigidbody como cinemático inicialmente para que não seja afetado pela física
    }

    // Quando um collider entra em contato com o collider deste objeto
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.isKinematic = false; // Ativa o Rigidbody, permitindo que ele seja afetado pela física
        }
    }
}
