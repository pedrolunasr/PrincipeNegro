using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTimeStop : MonoBehaviour
{

    public PlayerMovement2D playerMovement;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
           
            playerMovement.KBCounter = playerMovement.KBTotalTime;

            if(collision.transform.position.x <= transform.position.x)
            {
                playerMovement.KnockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.KnockFromRight = false;
            }

            collision.gameObject.GetComponent<TimeStopOnDamage>().StopTime(0.05f, 10, 0.1f);
        }

    }
}
