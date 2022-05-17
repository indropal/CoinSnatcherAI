using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject Jammo_Player, AI_Agent, GamePlayPanel, GameOverPanel, AboutGamePanel;
    public Text countdownText, PlayerScoreText, AIScoreText, PlayerControlText, PlayervsAI, FinalScores;
    public Text gameSessionCountDownText; // Text to display text for counting down while playing the game
    public bool gamePlaying { get; private set; }
    public int countDownTime;
    public float timeRemaining = 15f;
    public bool gameEnd { get; private set; }

    //private float startTime, elapsedTime;
    private bool gameSessionTimerGoing;

    private void Awake()
    {
        gamePlaying = false; // Demarcate the beginning of the game
        gameEnd = false; // Demarcate the ending of the game
        instance = this; // instantiate the current object of the class

        // initialise the gameSession counter display
        gameSessionTimerGoing = false; // boolean value to initiate the game session countdown timer

        // Do not display GamePlayPanel Object until the Game Starts - after the Start Countdown Timer completes
        GamePlayPanel.SetActive(false);

        StartCoroutine(CountdownToStart()); // Start the Countdown before initiating the game...
    }

    private void BeginGame()
    {
        gamePlaying = true;
        BeginGameTimer(); // Initiating the Game Session Timer
    }

    private void BeginGameTimer()
    {
        gameSessionTimerGoing = true;
        StartCoroutine(UpdateGameSessionTimer());
    }

    IEnumerator UpdateGameSessionTimer()
    {
        while(gameSessionTimerGoing)
        {
            timeRemaining -= (1.6f * Time.deltaTime); 
            string gameSessionTimer = "Time Left: " + TimeSpan.FromSeconds(timeRemaining).ToString("ss'.'ff") + " s";
            gameSessionCountDownText.text = gameSessionTimer; 

            if(timeRemaining <= 0.0f)
            {
                gameSessionTimerGoing = false;

                yield return new WaitForSeconds(0.5f); 
                gameEnd = true; 
                gamePlaying = false; 
                EndGame();
            }

            yield return null;  
        }
    }

    private void EndGame()
    {
        PlayervsAI.text = "";

        int AIAgentScore = Int32.Parse( AIScoreText.text.Substring(AIScoreText.text.LastIndexOf(' ') + 1) ); // Int32.Parse( AIScoreText.text.Split(' ').Last() );
        int PlayerScore = Int32.Parse( PlayerScoreText.text.Substring(PlayerScoreText.text.LastIndexOf(' ') + 1) ); // Int32.Parse( PlayerScoreText.text.Split(' ').Last() );

        if (PlayerScore > AIAgentScore)
        {
            PlayervsAI.text = "Player Wins!";
        }
        else if (PlayerScore < AIAgentScore)
        {
            PlayervsAI.text = "AI Agent Wins!";
        }
        else
        {
            PlayervsAI.text = "Tied!";
        }

        FinalScores.text = "Player Score: " + PlayerScore + " AI Agent Score: " + AIAgentScore;

        Invoke("ShowGameOverScreen", 1.05f);

    }

    private void ShowGameOverScreen()
    {
        gameSessionCountDownText.gameObject.SetActive(false); // Disable the GamePlay Session Countdown Timer display text
        PlayerControlText.gameObject.SetActive(false); // Disable the display of Player Controls Text
        GamePlayPanel.SetActive(false); // Disable the Game Play Display Texts so that it does not show on-screen
        GameOverPanel.SetActive(true); // Show the Game Over Panel
   }

    IEnumerator CountdownToStart()
    {
        while (countDownTime > 0)
        {
            countdownText.text = countDownTime.ToString();

            yield return new WaitForSeconds(1.25f);

            countDownTime--;
        }

        // Display the GamePlay Panel
        GamePlayPanel.SetActive(true);

        BeginGame();
        countdownText.text = "GO!";

        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false); // disable count down text
    }

    public void OnButtonLoadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad); // Load a specific Scene Object which is passed as argument to the function
    }

    public void OnButtonAboutScreen( )
    {
        // Disable all previous Panels to enable the About Screen
        GameOverPanel.SetActive(false);

        // Display the About Game Screen
        AboutGamePanel.SetActive(true);
    }

    public void OnButtonBack( )
    {
        // Display the About Game Screen
        AboutGamePanel.SetActive(false);

        // Disable all previous Panels to enable the About Screen
        GameOverPanel.SetActive(true);

    }

}
