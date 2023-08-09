using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTimeStopOnTrigger : MonoBehaviour
{
    public PlayerMovement2D playerMovement;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            playerMovement.KBCounter = playerMovement.KBTotalTime;

            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovement.KnockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.KnockFromRight = false;
            }

            collision.gameObject.GetComponent<TimeStopOnDamage>().StopTime(0.05f, 10, 0f);
        }

    }
}
