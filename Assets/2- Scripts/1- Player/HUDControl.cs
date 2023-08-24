using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{

    public static HUDControl hControl { get; private set; }
    PlayerMovement2D pMove;


    //vida do personagem
    public int life = 3;
    public GameObject[] heartHUD = new GameObject[3];

    //Gold do personagem
    private static string ammountGoldPrefs = "ammountGoldPrefs";
    private int ammountGold;

    public Text goldText;
    //public Text goldTextLevelComplet;
    public int gold;


    void Start()
    {
        pMove = PlayerMovement2D.pMove;

        ammountGold = PlayerPrefs.GetInt(ammountGoldPrefs); //Pegar config salvas da quantidade total de ouro
        MoreGold(ammountGold);


    }

    private void Awake()
    {
        if (hControl == null)
        {
            hControl = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    public void LessLife()
    {

        pMove.playSoundNow(pMove.hitSoundFx);
        life--;
        if(life < 0)
        {
            life = 0;
        }
        LifeHud();
    }

    public void instaKill()
    {
        //life = 0;
        pMove.playSoundNow(pMove.hitSoundFx);
        while (life > 0)
        {
            LessLife();
        }
        LifeHud();
    }
    public void EndOfLife()
    {
        life = 0;
        LifeHud();
        
    }

    public void MoreLife()
    {
        
        if( life < 3 ){
            pMove.playSoundNow(pMove.getItemSoundFx);
            life++;
            LifeHud();
        }
        
    }

    public int Life()
    {
        return life;
    }

    public void MoreGold(int value)
    {
        pMove.getItemPlaySound();
        gold = gold + value;
        PlayerPrefs.SetInt(ammountGoldPrefs, gold);
        LifeHud();

        
    }

    public void LessGold()
    {
        gold =  0;
        PlayerPrefs.SetInt(ammountGoldPrefs, gold);
        LifeHud();
    }


    public void ResetGold()
    {

        PlayerPrefs.DeleteKey(ammountGoldPrefs);
        int oldGold = gold * -1;
        MoreGold( oldGold );
    }

    public void ResetarGold()
    {
        ammountGoldPrefs = "0";
    }

    /*public void ResetoreGoldStock()
    {
        ammountGold = PlayerPrefs.GetInt(ammountGoldPrefs); //Pegar config salvas da quantidade total de ouro
        MoreGold(ammountGold);
    }*/

    //Aqui � a troca de imagme da HUD dependendo da quantidade de vida do player
    public void LifeHud()
    {

        //Limitar a vida que o player perde ou adquirir ao limite de cora��es que possui, n ocaso 3
        if (life <= 0)
        {
            life = 0;
        }
        else if (life >= 3)
        {
            life = 3;
        }
 
        switch (life)
        {
            case 3:
                heartHUD[0].SetActive(false);
                heartHUD[1].SetActive(false);
                heartHUD[2].SetActive(true);
                break;

            case 2:
                heartHUD[0].SetActive(false);
                heartHUD[1].SetActive(true);
                heartHUD[2].SetActive(false);
                break;

            case 1:
                heartHUD[0].SetActive(true);
                heartHUD[1].SetActive(false);
                heartHUD[2].SetActive(false);
                break;

            case 0:
                heartHUD[0].SetActive(false);
                heartHUD[1].SetActive(false);
                heartHUD[2].SetActive(false);

                pMove.PlayerDead(); // Chama essa fun�ao responsavel por matar o persoangem
                break;

        }

        //atualizar quantidad de dinheiro na HUD e jogo completo
        goldText.text = gold.ToString();
        //goldTextLevelComplet.text = gold.ToString();
    }




}
