using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrap : MonoBehaviour
{


    private Rigidbody2D rb2d;
    private CapsuleCollider2D cc2d;

    [SerializeField] private Rigidbody2D rb2dBall;
    [SerializeField] private GameObject ball;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(ActiveTrap());
        }
    }

    IEnumerator ActiveTrap()
    {
        rb2dBall.bodyType = RigidbodyType2D.Dynamic;
        Destroy(cc2d);

        yield return new WaitForSeconds(0.1f);
        ball.gameObject.tag = "instaKill";

        gameObject.layer = LayerMask.NameToLayer("TriggerPlayer");

        yield return new WaitForSeconds(4f);
        ball.gameObject.tag = "Untagged";
        

        yield return new WaitForSeconds(2f);
        Destroy(ball.gameObject);
        Destroy(this);
   
    }
}
