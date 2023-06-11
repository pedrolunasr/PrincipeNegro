using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour

{
    private float move;
    [SerializeField] private float moveSpeed = 0f;

    [SerializeField] private bool jumping;
    [SerializeField] private float jumpSpeed = 4.8f;

    [SerializeField] private float ghostJump;

    [SerializeField] private bool isGrounded;
    public Transform feetPosition;
    [SerializeField] private Vector2 sizeCapsule;
    [SerializeField] private float angleCapsule = 180;
    public LayerMask whatIsGround;

    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animationPlayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationPlayer = GetComponent<Animator>();

        sizeCapsule = new Vector2(0.325f, 0.01f);
    }

    void Update()
    {

        //Reconhecer o chão
        isGrounded = Physics2D.OverlapCapsule(feetPosition.position, sizeCapsule, CapsuleDirection2D.Horizontal, angleCapsule, whatIsGround);

        //Input de movimentação do personagem
        move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
        {
            moveSpeed += 15f * Time.deltaTime;

            if (moveSpeed >= 3.0f)
            {
                moveSpeed = 3.0f;
            }
        }
        else
        {
            moveSpeed = 0;
        }

        //Input do pulo do personagem
        if (Input.GetButtonDown("Jump") && ghostJump > 0)
        {
            jumping = true;
        }

        //Inveter posição do personagem
        if (move < 0)
        {
            sprite.flipX = true;
        }else if(move >0){
            sprite.flipX = false;
        }


        //Animação do personagem pulando, correndo e caindo


        if (isGrounded){
            animationPlayer.SetBool("Jumping", false);
            animationPlayer.SetBool("Falling", false);

            if (rb.velocity.x != 0 && move != 0)
            {
                animationPlayer.SetBool("Walking", true);
            }
            else
            {
                animationPlayer.SetBool("Walking", false);
            }
        }
        else
        {
            if(rb.velocity.x == 0)
            {
                animationPlayer.SetBool("Walking", false);

                if(rb.velocity.y > 0)
                {
                    animationPlayer.SetBool("Jumping", true);
                    animationPlayer.SetBool("Falling", false);
                }
                if(rb.velocity.y < 0)
                {
                    animationPlayer.SetBool("Jumping", false);
                    animationPlayer.SetBool("Falling", true);
                }
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    animationPlayer.SetBool("Jumping", true);
                    animationPlayer.SetBool("Falling", false);
                }
                if (rb.velocity.y < 0)
                {
                    animationPlayer.SetBool("Jumping", false);
                    animationPlayer.SetBool("Falling", true);
                }
            }
        }


        //Código do pulando e caindo

        if (isGrounded)
        {

            ghostJump = 0.05f;

        }
        else
        {
            ghostJump -= Time.deltaTime;

            if (ghostJump <= 0)
            {
                ghostJump = 0;
            }

        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(feetPosition.position, sizeCapsule);
    }

    void FixedUpdate()

    {
        //Movimentação do personagem
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        //Pulo do personagem
        if (jumping)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            //rb.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);

            //Desativar o pulo
            jumping = false;
        }
    }
}
