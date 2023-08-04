using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    PlayerMovement2D pMove;
    public Transform target;
    public float smoothSpeed = 0.5f;
    public float maxCameraHeightOffset = 0.1f; // Limite máximo de altura da câmera em relação à posição inicial do jogador
    public bool followXFromStart = true; // Define se a câmera deve seguir o jogador no eixo X desde o início

    private float initialPosition; // Variável para armazenar a posição inicial do jogador no eixo Y
    private float targetYPos; // Variável para armazenar a posição vertical alvo da câmera

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = target.position.y + 2.5f; // Armazena a posição inicial do jogador no eixo Y

        if (followXFromStart)
        {
            pMove = PlayerMovement2D.pMove;
            // Iniciar a câmera na posição exata do jogador no eixo X
            Vector3 startPosition = new Vector3(target.position.x, transform.position.y, -1f);
            transform.position = startPosition;
        }

        // Iniciar a posição vertical alvo da câmera na posição inicial do jogador
        targetYPos = initialPosition;
    }

    void FixedUpdate()
    {
        // Verificar a posição vertical atual do jogador
        float playerYPos = target.position.y;

        // Ajustar a posição vertical alvo da câmera apenas quando o jogador estiver subindo
        if (playerYPos != targetYPos && !pMove.jumping && pMove.isGrounded)
        {
            targetYPos = playerYPos;
        }

        

        // Calcular a posição alvo da câmera com base no jogador e na posição vertical alvo
        Vector3 targetPosition = new Vector3(target.position.x, targetYPos, -1f);

        // Suavizar o movimento da câmera em relação à posição alvo
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime / smoothSpeed  );
        transform.position = smoothPosition;
    }
}
