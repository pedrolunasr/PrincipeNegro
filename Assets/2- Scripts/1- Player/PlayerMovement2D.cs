using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement2D : MonoBehaviour

{
    public static PlayerMovement2D pMove { get; private set; }

    public static float move;

    [SerializeField] private float moveSpeed = 0f;

    [SerializeField] private bool jumping;
    [SerializeField] private float jumpSpeed = 4.8f;

    [SerializeField] private float ghostJump;

    [SerializeField] private bool isGrounded;
    public Transform feetPosition;
    [SerializeField] private Vector2 sizeCapsule;
    [SerializeField] private float angleCapsule = 180;
    public LayerMask whatIsGround;

    [SerializeField] private bool attackingBool;

    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animationPlayer;

    private bool isPaused;
    private bool isPausedToDeath;


    [Header("Pause System")]
    public GameObject pausePanel;

    [Header("Death System")]
    public GameObject deathPanel;


    float contadorDeath; //tempo ate mostrar tela de morte


    public bool doubleAtk, lockAtk = false;

    //Bloquear o Input do personagem
    public static bool blockInput = false;

    private void Awake()
    {
        if(pMove == null)
        {
            pMove = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        //Tirar cursor do mouse da tela
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        Time.timeScale = 1f;



        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationPlayer = GetComponent<Animator>();

        sizeCapsule = new Vector2(0.325f, 0.01f);
    }

    void Update()
    {

        //Contador Tempo apos morte (condicao que o blockinput esteja ligado, tomar cuidado)
        if (blockInput == true)
        {
            contadorDeath = contadorDeath + Time.deltaTime;
            Debug.Log(contadorDeath);

            if(contadorDeath >= 1.5f)
            {
                PauseDeath();
            }



        }

        //Pausar jogo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen();
        }


        //Reconhecer o chão
        isGrounded = Physics2D.OverlapCapsule(feetPosition.position, sizeCapsule, CapsuleDirection2D.Horizontal, angleCapsule, whatIsGround);

        if (blockInput == false)
        {

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

            //Input do ataque do personagem
            if (Input.GetButtonDown("Fire3") && lockAtk == false)
            {

                attackingBool = true;

                if (isGrounded)
                {
                    animationPlayer.SetBool("SingleAttackGround", true);
                    animationPlayer.SetBool("AttackJump", false);

                    animationPlayer.SetBool("DoubleAttack", false);
                }
                else
                {
                    animationPlayer.SetBool("AttackJump", true);
                    animationPlayer.SetBool("SingleAttackGround", false);

                    animationPlayer.SetBool("DoubleAttack", false);
                }

                if (doubleAtk == true)
                {
                    animationPlayer.SetBool("DoubleAttack", true);
                    animationPlayer.SetBool("SingleAttackGround", false);
                }
            }

            if (attackingBool == true && isGrounded)
            {
                move = 0;

            }


            //Inveter posição do personagem
            if (move < 0)
            {
                sprite.flipX = true;
            }
            else if (move > 0)
            {
                sprite.flipX = false;
            }


            //Animação do personagem pulando, correndo e caindo


            if (isGrounded)
            {
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
                if (rb.velocity.x == 0)
                {
                    animationPlayer.SetBool("Walking", false);

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

    public void PlayerDead()
    {
        animationPlayer.SetBool("Dead", true);
        blockInput = true;
        moveSpeed = 0;

        PlayerLife.bc.enabled = false;

    }

    void EndAnimationATK()
    {
        animationPlayer.SetBool("SingleAttackGround", false);
        animationPlayer.SetBool("AttackJump", false);

        attackingBool = false;
    }

    void EndAnimationDoubleAtk()
    {
        animationPlayer.SetBool("DoubleAttack", false);
        doubleAtk = false;
        attackingBool = false;
    }

    //Personagem levando dano

    public IEnumerator DamagePlayer()
    {
        animationPlayer.SetBool("Damage", true);
        sprite.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1f, 1f, 1f, 1f);
        animationPlayer.SetBool("Damage", false);

        for (int i=0; i< 7; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.15f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        PlayerLife.bc.enabled = true;
    }

    void PauseScreen()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
    }


    public IEnumerator DeathPlayerTime()
    {
        yield return new WaitForSeconds(0.2f);
    }

        void PauseDeath()
    {


            isPausedToDeath = true;
            Time.timeScale = 0f;
            deathPanel.SetActive(true);

            blockInput = false;
            moveSpeed = 3.0f;

            PlayerLife.bc.enabled = true;
    }


}
