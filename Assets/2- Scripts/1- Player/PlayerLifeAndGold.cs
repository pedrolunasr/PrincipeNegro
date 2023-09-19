using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerLifeAndGold : MonoBehaviour
{
    public static PlayerLifeAndGold pItens { get; private set; }
    
    public HUDControl hControl;
    PlayerMovement2D pMove;

    public static BoxCollider2D bc;

    public GameObject ImpactEffect;

    public int valueGold = 1;

    void Start()
    {
        hControl = HUDControl.hControl;
        pMove = PlayerMovement2D.pMove;

        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Damage01")
        {
            //Instantiate(ImpactEffect, transform.position, Quaternion.identity);
            hControl.LessLife();
            bc.enabled = true;

            if (hControl.life > 0)
            {
                StartCoroutine(pMove.DamagePlayer());

            }
            else if (hControl.life <= 0)
            {
                Destroy(this);
            }

            ImpactEffectDamagePlayer();



        }

        if (collision.gameObject.tag == "instaKill")
        {

            hControl.instaKill();
            bc.enabled = true;

            if (hControl.life > 0)
            {
                StartCoroutine(pMove.DamagePlayer());
            }

            ImpactEffectDamagePlayer();
            Destroy(this);

        }

        //ganhar vida
        if (collision.gameObject.tag == "Health")
        {
            if (hControl.life < 3)
            {
                hControl.MoreLife();
                Destroy(collision.gameObject);
            }
        }


        //ganhar moeda
        if (collision.gameObject.tag == "Coin")
        {
            hControl.MoreGold(valueGold);
            pMove.getItemPlaySound();
            Destroy(collision.gameObject);
        }

        //perder todas as moedas
        if (collision.gameObject.tag == "Zerar")
        {
            hControl.LessGold();
            Destroy(collision.gameObject);
        }
    }

private void ImpactEffectDamagePlayer()
    {

        Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        //Destroy(ImpactEffect);

    }

    public void initGold()
    {
        hControl.ResetGold();
    }

    /*public void restoreGoldValue()
    {
        hControl.ResetoreGoldStock();
    }*/

    /*
    void ManageCollision( GameObject coll )
    {

        if( coll.CompareTag("Damage01") ){

            hControl.LessLife();
            
            if (hControl.life > 0)
            {
                StartCoroutine(pMove.DamagePlayer());
            }


        }

        if( coll.CompareTag("sea") ){

            hControl.EndOfLife();
            
            if (hControl.life > 0)
            {
                StartCoroutine(pMove.DamagePlayer());
            }

        }

    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        ManageCollision( collision.gameObject );
    }

    */

}