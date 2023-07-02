using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    //Script de reconhecimento de ataque do personagem

    [SerializeField] private BoxCollider2D colliderAtkPlayer;

    void Start()
    {
        colliderAtkPlayer = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //inverção da posição do colisor de ataque baseado na posição do player
        if(PlayerMovement2D.move < 0)
        {
            colliderAtkPlayer.offset = new Vector2(-0.8f, 0);
        }
        else if (PlayerMovement2D.move > 0)
        {
            colliderAtkPlayer.offset = new Vector2(0.8f, 0);
        }
    }
}
