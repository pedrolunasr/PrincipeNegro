using System;
using static Platformer.Core.Simulation;
using System.Collections;
using Platformer.Gameplay;
using UnityEngine;




namespace Platformer.Mechanics
{

    public class SimpleWarriorController : MonoBehaviour
    {

        
        public int life = 3;
        public float moveSpeed = 1f;
        public int startingPoint;

        public AudioClip ouch;
        public Animator animator;
        public GameObject playerTarget;
        public GameObject activeItem;
        public BoxCollider2D colliderAttack;
        public BoxCollider2D colliderCheckAttack;
        public BoxCollider2D boxCollider2D;
        public SpriteRenderer spriteRenderer;

        public Material[] materialSprite;
        public Transform[] pointsToMove;
        
        private HUDControl helfControl;

        internal AudioSource _audio;

        


        void Start()
        {
            playerTarget = GameObject.Find("Player");
            helfControl = HUDControl.hControl;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();

        }

        private void InitializeEnemie( Transform mainTransform, Transform[] pointsToMove )
        {
            if (pointsToMove != null && pointsToMove.Length >= 3)
            {
                mainTransform.position = pointsToMove[2].transform.position;
            }
        }

        private void Awake()
        {

            InitializeEnemie(transform, pointsToMove);

        }

        void Update()
        {

        }

        private void FixedUpdate()
        {
            Move(
                  transform
                , pointsToMove
                , playerTarget
                , spriteRenderer
                , colliderCheckAttack
                , colliderAttack
                , animator
                , gameObject
            );
        }

        private void Move( 
              Transform mainTransforme
            , Transform[] pointsToMove
            , GameObject playerTarget
            , SpriteRenderer spriteRenderer 
            , BoxCollider2D colliderCheckAttack
            , BoxCollider2D colliderAttack
            , Animator animator
            , GameObject targetMoveObject
        )
        {
            SimpleWarriorMele(
                mainTransforme
                , pointsToMove
                , playerTarget
                , spriteRenderer
                , colliderCheckAttack
                , colliderAttack
                , animator
                , targetMoveObject
            );

            if (AttackCheckSimpleWarrior.checkAttack == true)
            {
                if (Vector3.Distance(transform.position, playerTarget.transform.position) < 3.5f)
                {
                    if (Vector3.Distance(transform.position, playerTarget.transform.position) > 0f)
                    {
                        
                        var ev = Schedule<EnemyAttack>();
                        ev.enemy = this;
                    }
                }

            }
        }

        private void SimpleWarriorMele(
              Transform mainTransforme
            , Transform[] pointsToMove
            , GameObject playerTarget
            , SpriteRenderer spriteRenderer
            , BoxCollider2D colliderCheckAttack
            , BoxCollider2D colliderAttack
            , Animator animator
            , GameObject targetMoveObject
        )
        {
            if (Vector3.Distance(mainTransforme.position, playerTarget.transform.position) < 2.5f)
            {
                if (Vector3.Distance(transform.position, playerTarget.transform.position) > 0.8f)
                {
                    if (mainTransforme.position.x < playerTarget.transform.position.x)
                    {
                        spriteRenderer.flipX = false;

                        mainTransforme.position = Vector2.MoveTowards(mainTransforme.position, pointsToMove[1].transform.position, moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        spriteRenderer.flipX = true;

                        mainTransforme.position = Vector2.MoveTowards(mainTransforme.position, pointsToMove[0].transform.position, moveSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                if (pointsToMove != null && pointsToMove.Length >= 3)
                {

                    mainTransforme.position = Vector2.MoveTowards(mainTransforme.position, pointsToMove[2].transform.position, moveSpeed * Time.deltaTime);


                    if (targetMoveObject.transform.position.x > pointsToMove[2].position.x)
                    {
                        spriteRenderer.flipX = true;
                    }
                    if (targetMoveObject.transform.position.x < pointsToMove[2].position.x)
                    {
                        spriteRenderer.flipX = false;
                    }

                }

            }

            if (spriteRenderer.flipX == true)
            {

                colliderAttack.offset = new Vector2(-0.75f, 0.3f);
                colliderCheckAttack.offset = new Vector2(-0.5f, 0f);
            }
            else
            {
                colliderAttack.offset = new Vector2(0.75f, 0.3f);
                colliderCheckAttack.offset = new Vector2(0.5f, 0f);
            }

            if (targetMoveObject.transform.position.x == pointsToMove[0].position.x ||
                targetMoveObject.transform.position.x == pointsToMove[1].position.x ||
                targetMoveObject.transform.position.x == pointsToMove[2].position.x ||
                Vector3.Distance(transform.position, playerTarget.transform.position) <= 1f || moveSpeed == 0)
            {

                animator.SetBool("Walking", false);
            }
            else
            {
                animator.SetBool("Walking", true);
            }
        }



        IEnumerator Attack()
        {
            AttackCheckSimpleWarrior.checkAttack = false;
            colliderCheckAttack.enabled = false;

            animator.SetTrigger("Attack");
            moveSpeed = 0;

            yield return new WaitForSeconds(1.5f); //Velocidade de ataque/recupera��o

            moveSpeed = 1.2f;
            colliderCheckAttack.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Attack")
            {
                life--;

                if (life > 0)
                {
                    StartCoroutine("EnemyDamage");

                }
                else if (life <= 0)
                {
                    animator.SetBool("Walking", false);
                    StopAllCoroutines();
                    EnemyDead();
                    helfControl.MoreLife();
                }
            }
        }

        IEnumerator EnemyDamage()
        {

            moveSpeed = 0;
            animator.speed = 0;

            spriteRenderer.material = materialSprite[0];

            yield return new WaitForSeconds(0.2f);

            spriteRenderer.material = materialSprite[1];

            yield return new WaitForSeconds(0.3f);

            moveSpeed = 1f;
            animator.speed = 1;

            yield return new WaitForSeconds(0.5f);

            activeItem.SetActive(true);

        }

        private void EnemyDead()
        {
            animator.SetTrigger("Dead");
            animator.SetBool("Walking", false);

            life = 0;
            spriteRenderer.material = materialSprite[1];
            moveSpeed = 0;

            if (transform.gameObject.GetComponent<BoxCollider2D>() != null)
                Destroy(transform.gameObject.GetComponent<BoxCollider2D>());

            if (transform.gameObject.GetComponent<Rigidbody2D>() != null)
                Destroy(transform.gameObject.GetComponent<Rigidbody2D>());

            if (colliderAttack != null)
                Destroy(colliderAttack);

            if (colliderCheckAttack != null)
                Destroy(colliderCheckAttack);

            if (pointsToMove != null && pointsToMove.Length > 1)
            {

                foreach (var point in pointsToMove)
                {
                    Destroy(point);
                }

            }

            Destroy(this);
        }

    }
}