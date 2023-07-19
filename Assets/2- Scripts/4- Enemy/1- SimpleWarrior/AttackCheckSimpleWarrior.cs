using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckSimpleWarrior : MonoBehaviour
{
    public static bool checkAttack = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject subObject = collision.transform.Find("Body").gameObject;
        if (collision.gameObject.tag == "Player" && subObject.tag == "Body")

        {
            BoxCollider2D collider = subObject.GetComponent<BoxCollider2D>();
            if (collider.enabled)
            {
                checkAttack = true;
            }
            else
            {
                checkAttack = false;
            }
        }

    }
}
