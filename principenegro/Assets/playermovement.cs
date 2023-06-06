using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float jumpForce = 60f;  // Força do salto
    public float moveSpeed = 30f;  // Velocidade de movimento

    private Rigidbody2D rb;  // Referência ao componente Rigidbody2D

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  // Obtém a referência ao Rigidbody2D
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Verifica se a tecla de espaço foi pressionada
        {
            Jump();
        }
        float moveInput = Input.GetAxis("Horizontal");  // Obtém a entrada do jogador no eixo horizontal (-1 a 1)
         rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);  // Aplica uma força vertical para o salto
    }
}

