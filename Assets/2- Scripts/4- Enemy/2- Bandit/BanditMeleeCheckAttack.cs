using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMeleeCheckAttack : MonoBehaviour
{
    public static bool checkAttack = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            checkAttack = true;
        }
    }
}
