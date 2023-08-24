using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{

    public static CheckPoint history { get; private set; }


    //Esse checkpoint é para começar o jogo, no inicio

    private GameMaster gm;
    private BoxCollider2D bc2d;

    PlayerMovement2D pMove;

    private PlayerLifeAndGold pItens;

    void Start()
    {
        pMove = PlayerMovement2D.pMove;
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        bc2d = GetComponent<BoxCollider2D>();
        pItens = GameObject.Find("Body").GetComponentInChildren<PlayerLifeAndGold>();            

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //history

            if( ! ( this.tag == "history" ) )
            {
                gm.lastCheckPointPos = transform.position;
                gm.GoldAtCheckPoint = pItens.hControl.gold;
            }

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            pMove.PauseHistory();
            Destroy(bc2d);
        }
    }


}
