using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public enum ScoreType { XPosition, YPosition, Other}
    public ScoreType scoreBasedOn;
    public string highscoreKey;

    [Header("Drag stuffs here")]
    public Rigidbody2D playerRg2d;
    public GameObject player;
    public Canvas gameOverCanvas, gameplayCanvas;

    public Text inGameScore, finalScore, bestScore, tapToStart;
    private int scoreValue, bestValue;

    private void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(true);
        tapToStart.gameObject.SetActive(true);
        playerRg2d.bodyType = RigidbodyType2D.Static;
        Time.timeScale = 1;
    }

    private void FixedUpdate()
    {
        StartGame();
        InGameScore();
    }

    void StartGame()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKey)
        {
            playerRg2d.bodyType = RigidbodyType2D.Dynamic;
            tapToStart.gameObject.SetActive(false);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverCanvas.gameObject.SetActive(true);
        gameplayCanvas.gameObject.SetActive(false);
        BestScore();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        gameOverCanvas.gameObject.SetActive(false);
    }

    public void InGameScore()
    {
        if (scoreBasedOn == ScoreType.XPosition)
        {
            scoreValue = (int)player.transform.position.x / 10;
        }
        else if (scoreBasedOn == ScoreType.YPosition)
        {
            scoreValue = (int)player.transform.position.y / 10;
        }

        inGameScore.text = "Score: " + scoreValue;
    }

    public void BestScore()
    {
        int highscore = PlayerPrefs.GetInt(highscoreKey);
        if (scoreValue > highscore)
        {
            highscore = scoreValue;
            PlayerPrefs.SetInt(highscoreKey, highscore);
            finalScore.text = "Score: " + scoreValue;
            bestScore.text = "Best: " + highscore;
        }
        else
        {
            finalScore.text = "Score: " + scoreValue;
            bestScore.text = "Best: " + highscore;
            //PlayerPrefs.SetInt(highscoreKey, 0);
        }
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else Time.timeScale = 1;

        }
    }

    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }
}
