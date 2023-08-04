using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMelee : MonoBehaviour
{

    public Animator anim;
    public SpriteRenderer sprite;

    private int life = 2;
    public float moveSpeed = 1f;

    public Transform[] pointsToMove;
    public int startingPoint;

    public BoxCollider2D colliderAtk;
    public BoxCollider2D colliderCheckAtk;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        transform.position = pointsToMove[startingPoint].transform.position;
    }

    void Update()
    {
        if (startingPoint == 0)
        {
            sprite.flipX = false;
            colliderAtk.offset = new Vector2(-0.3f, 0.9f);
            colliderCheckAtk.offset = new Vector2(-0.4f, 0.6f);
        }
        else
        {
            sprite.flipX = true;
            colliderAtk.offset = new Vector2(0.3f, 0.9f);
            colliderCheckAtk.offset = new Vector2(0.4f, 0.6f);
        }

        if(life == 0)
        {
            EnemyDead();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, pointsToMove[startingPoint].transform.position, moveSpeed * Time.deltaTime);

        if(BanditMeleeCheckAttack.checkAttack == true)
        {
            StartCoroutine("Attack");
        }

        if (transform.position == pointsToMove[startingPoint].transform.position)
        {
            startingPoint += 1;
        }

        if(startingPoint == pointsToMove.Length)
        {
            startingPoint = 0;
        }

        if(moveSpeed != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    private void EnemyDead()
    {
        life = 0;
        anim.SetTrigger("Dead");
        moveSpeed = 0;
        //Destroy(transform.gameObject.GetComponent<BoxCollider2D>());
        Destroy(transform.gameObject.GetComponent<Rigidbody>());
        Destroy(colliderAtk);
        Destroy(colliderCheckAtk);
        Destroy(this);
    }
    
    IEnumerator Attack()
    {
        anim.SetBool("Attack", true);
        moveSpeed = 0;

        yield return new WaitForSeconds(0.85f);
        anim.SetBool("Attack", false);
        moveSpeed = 1;

        BanditMeleeCheckAttack.checkAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            life--;

            if(life < 1)
            {
                StopCoroutine("Attack");
                EnemyDead();
            }
        }
    }
}