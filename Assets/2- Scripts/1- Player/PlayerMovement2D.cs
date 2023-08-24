using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.IK;
using static UnityEngine.ParticleSystem;

public class PlayerMovement2D : MonoBehaviour
{
    public static PlayerMovement2D pMove { get; private set; }

    public static float move;

    [SerializeField] private float moveSpeed = 0f;

    [SerializeField] public bool jumping;
    [SerializeField] private float jumpSpeed = 4.8f;

    [SerializeField] private float ghostJump;

    [SerializeField] public bool isGrounded;
    public Transform feetPosition;
    [SerializeField] private Vector2 sizeCapsule;
    [SerializeField] private float angleCapsule = 180;
    public LayerMask whatIsGround;

    [SerializeField] private bool attackingBool;

    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animationPlayer;

    private bool isPaused;


    private GameMaster gm;
    CheckPoint history;
    private bool historyActive;


    [Header("Pause System")]
    public GameObject pausePanel;

    [Header("Death System")]
    public GameObject deathPanel;

    [Header("Pause System")]
    public GameObject[] pauseHistory;


    public GameObject[] checkPoints;

    public AudioClip attackSoundFx;
    public AudioClip jumpSoundFx;
    public AudioClip walkSoundFx;
    public AudioClip hitSoundFx;
    public AudioClip getItemSoundFx;



    float contadorDeath; //tempo ate mostrar tela de morte

    //ataque duplo
    //public bool doubleAtk, lockAtk = false;

    //Bloquear o Input do personagem
    public static bool blockInput = false;


    // tomar dano e ir pra tras
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;

    public bool KnockFromRight;

    private Animator AnimDamage;

    public ParticleSystem dust;

    private Vector2 rayOrigin; // Posição do início do raio (pés)
    private Vector2 rayDirection; // Direção do raio (para baixo)
    private float rayDistance = 0.5f; // Distância do raio (ajuste conforme necessário)
    private Vector2 surfaceNormal;
    private float slopeAngle;

    private HUDControl hControl;


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


        hControl = GameObject.Find("Canvas").GetComponent<HUDControl>();

        AnimDamage = GetComponentInChildren<Animator>();

        Time.timeScale = 1f;



        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationPlayer = GetComponent<Animator>();

        sizeCapsule = new Vector2(0.325f, 0.01f);


        //Referente ao mecanismo de checkpoint e GameMaster
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
        hControl.MoreGold(gm.GoldAtCheckPoint);

        history = CheckPoint.history;

    }

    void Update()
    {

        //Contador Tempo apos morte (condicao que o blockinput esteja ligado, tomar cuidado)
        if (blockInput == true)
        {
            contadorDeath = contadorDeath + Time.deltaTime;
            Debug.Log(contadorDeath);

            if(contadorDeath >= 1.8f)
            {
                PauseDeath();
            }

        }

        //Pausar jogo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (historyActive == false)
            {
                PauseScreen();
            }

        }



        //Reconhecer o ch�o
        isGrounded = Physics2D.OverlapCapsule(feetPosition.position, sizeCapsule, CapsuleDirection2D.Horizontal, angleCapsule, whatIsGround);

        rayOrigin = feetPosition.position; // Posição do início do raio (pés)
        rayDirection = Vector2.down; // Direção do raio (para baixo)
        rayDistance = 0.5f; // Distância do raio (ajuste conforme necessário)

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, whatIsGround);

        if (hit.collider != null)
        {
            surfaceNormal = hit.normal;
            slopeAngle = Vector2.Angle(Vector2.up, surfaceNormal);

            if (surfaceNormal.x > 0)
            {
                slopeAngle = -slopeAngle;
            }

        }


        if (blockInput == false)
        {

            //Input de movimenta��o do personagem
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
                this.playSound(jumpSoundFx);
            }

            //Input do ataque do personagem3

            // ataque duplo
            //if (Input.GetButtonDown("Fire3") && lockAtk == false)
            if (Input.GetButtonDown("Fire3")) 
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

                this.playSound(attackSoundFx);
                /*   //ataque duplo
                if (doubleAtk == true)
                {
                    animationPlayer.SetBool("DoubleAttack", true);
                    animationPlayer.SetBool("SingleAttackGround", false);
                }
                */
            }

            if (attackingBool == true && isGrounded)
            {
                move = 0;

            }


            //Inveter posi��o do personagem
            if (move < 0)
            {
                 sprite.flipX = true;
                
            }
            else if (move > 0)
            {
                sprite.flipX = false;

            }


            //Anima��o do personagem pulando, correndo e caindo


            if (isGrounded)
            {
                animationPlayer.SetBool("Jumping", false);
                animationPlayer.SetBool("Falling", false);

                if (rb.velocity.x != 0 && move != 0)
                {
                    animationPlayer.SetBool("Walking", true);
                    this.playSound(walkSoundFx);
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
        


            //C�digo do pulando e caindo

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
        //Tomar knockback ao levar dano
        if(KBCounter <= 0)
        {
            //Movimenta��o do personagem
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
            GameObject playerBody = GameObject.Find("Body");
            BoxCollider2D boxColliderPlayerBody = playerBody.GetComponent<BoxCollider2D>();
            boxColliderPlayerBody.sharedMaterial.friction = 0f;

            if ( playerBody != null && boxColliderPlayerBody != null )
            {
                if (slopeAngle != 0 )
                {
                    boxColliderPlayerBody.sharedMaterial.friction = 1f;
                }
                else
                {
                    boxColliderPlayerBody.sharedMaterial.friction = 0f;
                }

                if ( jumping )
                {
                    boxColliderPlayerBody.sharedMaterial.friction = 0f;
                }
            }
            
            
        }
        else
        {
            if(KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForce, KBForce);
            }
            if (KnockFromRight == false)
            {
                rb.velocity = new Vector2(KBForce, KBForce);
            }

            KBCounter -= Time.deltaTime;
        }
            

        //Pulo do personagem
        if (jumping)
        {

            float jumpAngleRadians = slopeAngle;
            float jumpSpeedX = Vector2.up.x * jumpSpeed;
            float jumpSpeedY = Vector2.up.y * jumpSpeed;

            // Calcular as componentes horizontal e vertical da velocidade
            if (move > 0)
            {
                jumpSpeedX += Mathf.Cos(jumpAngleRadians);
                jumpSpeedY += Mathf.Sin(jumpAngleRadians);
           
            }
            else if ( move < 0)
            {
                jumpSpeedX -= Mathf.Cos(jumpAngleRadians);
                jumpSpeedY -= Mathf.Sin(jumpAngleRadians);
               
            }
           

            // Definir a velocidade do pulo
            Vector2 jumpVelocity = new Vector2(jumpSpeedX, jumpSpeedY);
            rb.velocity = jumpVelocity;


            //Desativar o pulo
            jumping = false;


        }

        if (move > 0 && jumping)
        {
            rb.SetRotation(slopeAngle);
        }
        else if (move < 0 && jumping)
        {
            rb.SetRotation(slopeAngle * move);
        }
        else if (move == 0 && jumping)
        {
            rb.SetRotation(0);
        }
        else
        {
            rb.SetRotation(0);
        }

    }

    public void PlayerDead()
    {
        blockInput = true;
        
        animationPlayer.SetBool("SingleAttackGround", false);
        animationPlayer.SetBool("AttackJump", false);
        animationPlayer.SetBool("DoubleAttack", false);
        animationPlayer.SetBool("Jumping", false);
        animationPlayer.SetBool("Falling", false);
        animationPlayer.SetBool("Walking", false);
        animationPlayer.SetBool("Dead", true);

        moveSpeed = 0;

        PlayerLifeAndGold.bc.enabled = false;

        //deixar por enquanto
        Destroy(transform.gameObject.GetComponent<BoxCollider2D>());

        //Destroy(this);

    }

    void EndAnimationATK()
    {
        animationPlayer.SetBool("SingleAttackGround", false);
        animationPlayer.SetBool("AttackJump", false);

        attackingBool = false;
    }


    //ataque duplo
    /*
    void EndAnimationDoubleAtk()
    {
        animationPlayer.SetBool("DoubleAttack", false);
        doubleAtk = false;
        attackingBool = false;
    }
    */

    //Personagem levando dano
    public IEnumerator DamagePlayer()
    {
        
        PlayerLifeAndGold.bc.enabled = false;
        animationPlayer.SetBool("Damage", true);
        animationPlayer.SetBool("SingleAttackGround", false);
        animationPlayer.SetBool("AttackJump", false);

        sprite.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1f, 1f, 1f, 1f);
        animationPlayer.SetBool("Damage", false);

        for (int i=0; i< 4; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.15f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
            
        PlayerLifeAndGold.bc.enabled = true;
        
    }

    void PauseScreen()
    {
        if (isPaused)
        {
            
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            isPaused = false;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            isPaused = true;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
    }

    public void PauseHistory()
    {

        float DistanceToChekPoint01 = 0;
        
        for(int i = 0; i < checkPoints.Length; i++)
        {
            DistanceToChekPoint01 = Vector3.Distance(transform.position, checkPoints[i].transform.position);

            if (DistanceToChekPoint01 <= 10f)
            {
                Time.timeScale = 0f;
                pauseHistory[i].SetActive(true);
                historyActive = true;
                break;
            }
        }


    }

    public void HistoryActive()
    {
        historyActive = false;
    }

    public IEnumerator DeathPlayerTime()
    {
        yield return new WaitForSeconds(0.2f);
    }

    void PauseDeath()
    {

        Time.timeScale = 0f;
        deathPanel.SetActive(true);

        blockInput = false;
        moveSpeed = 3.0f;

        PlayerLifeAndGold.bc.enabled = true;
    }

    void CreateDust()
    {
        dust.Play();
    }

    public void playSound( AudioClip sound )
    {

        AudioSource aSoudFx = GameObject.Find("SoundFx").GetComponent<AudioSource>();
        aSoudFx.clip = sound;

        if ( ! aSoudFx.isPlaying)
        {
            aSoudFx.Play();
        }
        
    }

    public void playSoundNow(AudioClip sound )
    {

        AudioSource aSoudFx = GameObject.Find("SoundFx").GetComponent<AudioSource>();

        if (!aSoudFx.isPlaying)
        {
            aSoudFx.Stop();
        }

        aSoudFx.clip = sound;
        aSoudFx.Play();

    }

    public void getItemPlaySound()
    {

        AudioSource aSoudFx = GameObject.Find("SoundFx").GetComponent<AudioSource>();
        
        if ( ! aSoudFx.isPlaying)
        {
            aSoudFx.Stop();
        }

        aSoudFx.clip = getItemSoundFx;
        aSoudFx.Play();

    }



}
