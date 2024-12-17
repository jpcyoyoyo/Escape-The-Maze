using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void StartGame()
    {
        // "Main Menu" is the name of the scene you want to load.
        SceneManager.LoadScene("Levels");
    }
}
