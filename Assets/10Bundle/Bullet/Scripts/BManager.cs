using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BManager : MonoBehaviour {

    public Canvas inGameCanvas, gameOverCanvas;

    public GameObject info;
    //UI gameOver

    public Text
        score,
        kills,
        headshoots,
        scoreBest,
        killsBest,
        headshootBest;

    public Image image;

    BulletControl bullet;

    private void Awake()
    {
        bullet = FindObjectOfType<BulletControl>();
    }

    private void Start()
    {
        inGameCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(false);

        info.gameObject.SetActive(false);
    }

    public void GameOverUI()
    {
        inGameCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(true);

        StartCoroutine("fade");
    }

    IEnumerator fade()
    {
        image.color = new Color(0, 0, 0, 0);

        while (image.color.a < 0.85f)
        {
            image.color = new Color(0,0,0, image.color.a + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }

        Best("score", "Score", bullet.score, score, scoreBest);
        Best("kills", "Kills", bullet.kills, kills, killsBest);
        Best("headshoot", "Headshoot", bullet.headshots, headshoots, headshootBest);

        info.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Best(string key, string name, int score, Text scoreText, Text bestScoreText)
    {
        int highscore = PlayerPrefs.GetInt(key);
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt(key, highscore);
            scoreText.text = name + ": " + score;
            bestScoreText.text = "Best: " + highscore;
        }
        else
        {
            scoreText.text = name + ": " + score;
            bestScoreText.text = "Best: " + highscore;
        }
    }
}
