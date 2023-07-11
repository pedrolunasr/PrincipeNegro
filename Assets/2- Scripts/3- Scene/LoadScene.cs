using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void InstagramMarks()
    {
        Application.OpenURL("https://www.instagram.com/reydner_/");
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

    public void SelectLevels()
    {
        SceneManager.LoadScene("SelectLevels");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
    }


    //ALL LEVELS

    public void Level01()
    {
        SceneManager.LoadScene("Level01");
    }

    public void Level02()
    {
        SceneManager.LoadScene("Level02");
    }
}
