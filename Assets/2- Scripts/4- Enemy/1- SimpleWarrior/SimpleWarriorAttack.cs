using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleWarriorAttack : MonoBehaviour
{
    public GameObject player;

    public Animator anim;
    public SpriteRenderer sprite;
    public Material[] materialSprite;

    public int life = 3;
    public float moveSpeed = 1f;

    public Transform[] pointsToMove;
    public int startingPoint;

    public BoxCollider2D colliderAtk;
    public BoxCollider2D colliderCheckAtk;

    public BoxCollider2D bc2d;

    private HUDControl hControl;

    void Start()
    {
        player = GameObject.Find("Player");
        hControl = HUDControl.hControl;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();

    }

    private void Awake()
    {
        if (pointsToMove != null && pointsToMove.Length >= 3 )
        {
            transform.position = pointsToMove[2].transform.position;
        }
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2.5f)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 0.8f)
            {
                if(transform.position.x < player.transform.position.x)
                {
                    sprite.flipX = false;

                    transform.position = Vector2.MoveTowards(transform.position, pointsToMove[1].transform.position, moveSpeed * Time.deltaTime);
                }
                else
                {
                    sprite.flipX = true;

                    transform.position = Vector2.MoveTowards(transform.position, pointsToMove[0].transform.position, moveSpeed * Time.deltaTime);
                }
            }
        }else
        {
            if ( pointsToMove != null && pointsToMove.Length >= 3 )
            {

                transform.position = Vector2.MoveTowards(transform.position, pointsToMove[2].transform.position, moveSpeed * Time.deltaTime);


                if (gameObject.transform.position.x > pointsToMove[2].position.x)
                {
                    sprite.flipX = true;
                }
                if (gameObject.transform.position.x < pointsToMove[2].position.x)
                {
                    sprite.flipX = false;
                }

            }
            
        }

        if(sprite.flipX == true)
        {

            colliderAtk.offset = new Vector2(-0.75f, 0.3f);
            colliderCheckAtk.offset = new Vector2(-0.5f,0f);
        }
        else
        {
            colliderAtk.offset = new Vector2(0.75f, 0.3f);
            colliderCheckAtk.offset = new Vector2(0.5f, 0f);
        }

        if(gameObject.transform.position.x == pointsToMove[0].position.x ||
            gameObject.transform.position.x == pointsToMove[1].position.x ||
            gameObject.transform.position.x == pointsToMove[2].position.x ||
            Vector3.Distance(transform.position, player.transform.position) <= 1f || moveSpeed == 0)
        {

            anim.SetBool("Walking", false);
        }
        else
        {
            anim.SetBool("Walking", true);
        }

        if(AttackCheckSimpleWarrior.checkAttack == true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 3.5f)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > 0f)
                {
                    StartCoroutine("Attack");
                }
            }
            
        }
    }

    IEnumerator EnemyDamage()
    {

        moveSpeed = 0;
        anim.speed = 0;

        sprite.material = materialSprite[0];

        yield return new WaitForSeconds(0.2f);

        sprite.material = materialSprite[1];

        yield return new WaitForSeconds(0.3f);

        moveSpeed = 1f;
        anim.speed = 1;

        yield return new WaitForSeconds(0.5f);


    }

    private void EnemyDead()
    {
        anim.SetTrigger("Dead");
        anim.SetBool("Walking", false);
        life = 0;
        sprite.material = materialSprite[1];
        moveSpeed = 0;
        Destroy(transform.gameObject.GetComponent<BoxCollider2D>());
        Destroy(transform.gameObject.GetComponent<Rigidbody2D>());
        Destroy(colliderAtk);
        Destroy(colliderCheckAtk);
        Destroy(pointsToMove[0]);
        Destroy(pointsToMove[1]);
        Destroy(pointsToMove[2]);
        Destroy(this);
    }

    IEnumerator Attack()
    {
        AttackCheckSimpleWarrior.checkAttack = false;
        colliderCheckAtk.enabled = false;

        anim.SetTrigger("Attack");
        moveSpeed = 0;

        yield return new WaitForSeconds(1.5f); //Velocidade de ataque/recupera��o

        moveSpeed = 1.2f;
        colliderCheckAtk.enabled = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            life--;

            if ( life > 0)
            {
                StartCoroutine("EnemyDamage");

            }
            else if (life <= 0)
            {
                anim.SetBool("Walking", false);
                StopAllCoroutines();
                EnemyDead();
                hControl.MoreLife();
            }
        }
    }

}
