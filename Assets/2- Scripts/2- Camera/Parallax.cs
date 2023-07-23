using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject cameraPlayer;
    public float speedParallax;

    private Transform[] layers;
    private float[] startPos;
    private float[] length;

    void Start()
    {
        layers = new Transform[transform.childCount];
        startPos = new float[transform.childCount];
        length = new float[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
            startPos[i] = layers[i].position.x;
            length[i] = layers[i].GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            float temp = (cameraPlayer.transform.position.x * (1 - speedParallax));
            float dist = (cameraPlayer.transform.position.x * speedParallax);

            layers[i].position = new Vector3(startPos[i] + dist, layers[i].position.y, layers[i].position.z);

            if (temp > startPos[i] + length[i])
            {
                startPos[i] += length[i];
            }
            else if (temp < startPos[i] - length[i])
            {
                startPos[i] -= length[i];
            }
        }
    }
}
