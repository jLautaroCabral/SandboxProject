using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public GameObject character, spawner;
    public Canvas gameOverCanvas, startCanvas, settingCanvas;
    public Camera myCam;
    public SpriteRenderer effects;
    public ParticleSystem explosion;
    public Text currentScore, finalScore, bestScore;
    public Image panel;

    int score;
    int best;

    private void Start()  {
        gameOverCanvas.gameObject.SetActive(false);
        startCanvas.gameObject.SetActive(true);
        settingCanvas.gameObject.SetActive(false);

        character.SetActive(false);
        spawner.SetActive(false);

        //PlayerPrefs.DeleteAll();
    }

    public void StartGame() {
        startCanvas.gameObject.SetActive(false);
        character.SetActive(true);
        spawner.SetActive(true);
        currentScore.text = "0";
    }

    public void CallGameOver() {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver() {
        explosion.Play();
        character.SetActive(false);
        panel.color = myCam.backgroundColor;
        
        yield return new WaitForSeconds(1); 
        Time.timeScale = 0;
        BestScore();
        gameOverCanvas.gameObject.SetActive(true);
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit() {
        Application.Quit();
    }

    public void AddScore(int num) {
        score += num;
        currentScore.text = score.ToString();
        StartCoroutine(Effect());
    }

    void BestScore() {
        best = PlayerPrefs.GetInt("Best");
        if (score > best) {
            best = score;
            PlayerPrefs.SetInt("Best", best);
        }

        finalScore.text = "Score: " + score;
        bestScore.text = "Best: " + best;
    }

    public void ShowSettings() {
        settingCanvas.gameObject.SetActive(true);
    }

    public void HideSettings() {
        settingCanvas.gameObject.SetActive(false);
    }

    IEnumerator Effect() {
        Color color = effects.color;
        float size = 0.1f;
        float a = 0;
        for (int i = 0; i < 10; i++){
            a += 0.05f;
            effects.color = new Color(1, 1, 1, a);
            currentScore.transform.localScale += new Vector3(size, size, 0);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++){
            a -= 0.05f;
            effects.color = new Color(1, 1, 1, a);
            currentScore.transform.localScale -= new Vector3(size, size, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
