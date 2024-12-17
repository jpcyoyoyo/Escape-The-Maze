using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsScript : MonoBehaviour
{

    public void StartLevel1()
    {
        // "Main Menu" is the name of the scene you want to load.
        SceneManager.LoadScene("Maze Game 1");
    }

    public void StartLevel2()
    {
        // "Main Menu" is the name of the scene you want to load.
        SceneManager.LoadScene("Maze Game 2");
    }

    public void StartLevel3()
    {
        // "Main Menu" is the name of the scene you want to load.
        SceneManager.LoadScene("Maze Game 3");
    }

    public void MainMenu()
    {
        // "Main Menu" is the name of the scene you want to load.
        SceneManager.LoadScene("Main Menu");
    }
}
