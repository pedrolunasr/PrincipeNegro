using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{

    HUDControl hControl;
    PlayerMovement2D pMove;

    public static BoxCollider2D bc;

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
            hControl.LessLife();
            bc.enabled = true;

            if (hControl.life > 0)
            {
                StartCoroutine(pMove.DamagePlayer());
            }

        }



        //ganhar vida
        if (collision.gameObject.tag == "MoreLife01")
        {
            hControl.MoreLife();
            //Control.Damage();

        }
    }

    void ManageCollision( GameObject coll )
    {

        if( coll.CompareTag("Damage01") ){

            hControl.LessLife();
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

}
