using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{

    [Header("Pause System")]
    public GameObject pauseHistory;


    //Esse checkpoint é para começar o jogo, no inicio

    private GameMaster gm;
    PlayerMovement2D pmove;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gm.lastCheckPointPos = transform.position;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            pauseHistory.SetActive(true);
        }
    }
}
