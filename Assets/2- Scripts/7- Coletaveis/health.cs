using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public GameObject heartPrefab;
    public int randomHeart;

    void Start()
    {
        DropHeart();
    }


    void DropHeart()
    {
        randomHeart = Random.Range(0, 10);

        if(randomHeart >= 3)
        {
            Instantiate(heartPrefab, gameObject.transform.position, transform.rotation);
        }
    }
}
