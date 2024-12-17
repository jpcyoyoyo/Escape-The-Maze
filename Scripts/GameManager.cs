using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUI;       // Reference to the pause menu UI GameObject
    public GameObject winMenuUI;         // Reference to the win menu UI GameObject
    public AudioSource bgMusic;          // Reference to the background music AudioSource
    public Text timerText;               // Reference to the live timer UI Text
    public Text finalTimerText;          // Reference to the final timer UI Text

    private float levelTime = 0f;        // Track the time spent on the level
    private bool isTimerRunning = true;  // Control if the timer is running

    void Start()
    {
        // Start background music
        if (bgMusic != null)
        {
            bgMusic.loop = true;
            bgMusic.Play();
        }
    }

    void Update()
    {
        // Update the timer if the game is not paused
        if (isTimerRunning)
        {
            levelTime += Time.deltaTime;
            UpdateLiveTimer(levelTime);
        }
    }

    // Pause the game
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);   // Show the pause menu
        Time.timeScale = 0f;           // Freeze the game
        isTimerRunning = false;        // Stop the timer
        if (bgMusic != null)
        {
            bgMusic.Pause();           // Pause background music
        }
    }

    // Resume the game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);  // Hide the pause menu
        Time.timeScale = 1f;           // Resume the game
        isTimerRunning = true;         // Resume the timer
        if (bgMusic != null)
        {
            bgMusic.Play();            // Resume background music
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Main Menu");
        ResetGameState();
        winMenuUI.SetActive(false);
    }

    public void WinGame()
    {
        winMenuUI.SetActive(true);      // Show win menu UI
        Time.timeScale = 0f;            // Freeze the game
        isTimerRunning = false;         // Stop the timer
        if (bgMusic != null)
        {
            bgMusic.Stop();             // Stop background music on win
        }

        DisplayFinalTime(levelTime);    // Show final time on win screen
    }

    private void ResetGameState()
    {
        Time.timeScale = 1f;            // Ensure game is unpaused if exiting from a paused state
        levelTime = 0f;                 // Reset timer
    }

    private void UpdateLiveTimer(float timeToDisplay)
    {
        // Display ongoing time on the main game UI in seconds.milliseconds format
        int seconds = (int)timeToDisplay;
        int milliseconds = (int)((timeToDisplay - seconds) * 1000);
        timerText.text = $"Time: {seconds}.{milliseconds:00}";
    }

    private void DisplayFinalTime(float timeToDisplay)
    {
        // Display the final time on the win screen in seconds.milliseconds format
        int seconds = (int)timeToDisplay;
        int milliseconds = (int)((timeToDisplay - seconds) * 1000);
        finalTimerText.text = $"{seconds}.{milliseconds:00}";
    }

    public void StartLevel2()
    {
        ResetGameState();
        SceneManager.LoadScene("Maze Game 2");
    }

    public void StartLevel3()
    {
        ResetGameState();
        SceneManager.LoadScene("Maze Game 3");
    }
}
