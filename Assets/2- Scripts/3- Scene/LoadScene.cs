using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public void Intro()
    {
        MusicManager.mManager.PlaySound(0);
        SceneManager.LoadScene("Menu");
        
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
        MusicManager.mManager.PlaySound(1);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
        MusicManager.mManager.PlaySound(9);
    }

    public void InstagramMarks()
    {
        Application.OpenURL("https://www.instagram.com/reydner_/");
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
        MusicManager.mManager.PlaySound(1);
    }

    public void SelectLevels()
    {
        SceneManager.LoadScene("SelectLevels");
        MusicManager.mManager.PlaySound(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //LEVEL COMPLETED/INCOMPLETED OR PAUSE GAME

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
        MusicManager.mManager.PlaySound(1);
        
    }


    //ALL LEVELS

    public void Level01()
    {
        MusicManager.mManager.PlaySound(2);
        SceneManager.LoadScene("Level-01");
        
    }

    public void Level02()
    {
        SceneManager.LoadScene("Level-02");
    }
}
