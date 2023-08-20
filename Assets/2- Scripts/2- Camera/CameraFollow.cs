using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    PlayerMovement2D pMove;
    private Transform target;
    public float smoothSpeed = 1f;
    public bool followXFromStart = false; // Define se a câmera deve seguir o jogador no eixo X desde o início

    private float initialPosition; // Variável para armazenar a posição inicial do jogador no eixo Y
    private float targetYPos; // Variável para armazenar a posição vertical alvo da câmera

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = target.position.y + 4f; // Armazena a posição inicial do jogador no eixo Y

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
        float playerYPos = target.position.y + 1f;
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

        string[] names = {
            "front_sky_0",
            "front_sky_1",
            "front_sky_2",
            "front_sky_3",
            "front_sky_4",
            "front_sky_5",
            "front_sky_6",
            "front_sky_7",
            "front_sky_8",
            "front_sky_9",
            "back_sky_0",
            "back_sky_1",
            "back_sky_2",
            "back_sky_3",
            "back_sky_4",
            "back_sky_5",
            "back_sky_6",
            "back_sky_7",
            "back_sky_8",
            "clouds_parallax",
            "clouds_parallax_front",
            "clouds_parallax_front_child",
            "clouds_parallax_back",
            "clouds_parallax_back_child",
            "far_grounds_parallax",
            "far_grounds_parallax_front",
            "far_grounds_parallax_back"
        };

        if (transform.position.x >= 190 )
        {
            float delta = (transform.position.x - 190) / 50; // Delta para variar de 0 a 1
            float r = Mathf.Lerp(1f, 0.33f, delta); // Começa com 1f (branco) e diminui para 0.53f
            float g = Mathf.Lerp(1f, 0.16f, delta); // Começa com 1f (branco) e diminui para 0.36f
            float b = Mathf.Lerp(1f, 0.60f, delta); // Começa com 1f (branco) e diminui para 0.80f
            float a = 0.255f;

            foreach ( string name in names )
            {
                GameObject.Find(name).GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
            }

        }else
        {
            foreach (string name in names)
            {
                GameObject.Find(name).GetComponent<SpriteRenderer>().color = Color.white;
            }
        } 
    }
}
