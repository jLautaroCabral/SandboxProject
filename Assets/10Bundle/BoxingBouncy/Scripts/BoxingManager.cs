using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BoxingManager : MonoBehaviour {

	public Text scoreText1, scoreText2, winnerText;
	public Player player1, player2;
	public int maxScore;

	public GameObject playCanvas, gameCanvas, endCanvas;

	int score1, score2;
	int loadCount;
	bool end;

	[HideInInspector] public bool gameStarted;

	void Start() {
        Setup();
	}

	public void StartGame() {
        PlayerPrefs.DeleteAll();
        gameStarted = true;
		score2 = score1 = 0;
        Setup();
		ManageCanvas(false, true, false);
	}

	public void GameOver() {
        ManageCanvas(false, false, true);
		winnerText.text = (score1 > score2)? "You Won!" : "Opponent Won!";
	}

	void Setup() {
		loadCount = PlayerPrefs.GetInt("LoadCount");

		if (loadCount <= 0) {
            ManageCanvas(true, false, false);
		}
		else {
            ManageCanvas(false, true, false);
            gameStarted = true;
		}

        score1 = PlayerPrefs.GetInt("Score1");
        score2 = PlayerPrefs.GetInt("Score2");

        scoreText1.text = score1.ToString();
        scoreText2.text = score2.ToString();
	}

	public void PlayerScores(int playerIndex) {
		if (playerIndex == 2) {
			score2++;
		}
		if (playerIndex == 1) {
			score1++;
		}

		PlayerPrefs.SetInt("Score1", score1);
        PlayerPrefs.SetInt("Score2", score2);

		scoreText1.text = score1.ToString();
        scoreText2.text = score2.ToString();

		if (score1 >= maxScore || score2 >= maxScore) {
			GameOver();
			end = true;
		}

        loadCount++;
        PlayerPrefs.SetInt("LoadCount", loadCount);

		if (!end) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void ManageCanvas(bool play, bool game, bool end) {
		playCanvas.SetActive(play);
		gameCanvas.SetActive(game);
		endCanvas.SetActive(end);
	}

	public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		StartGame();
	}

	void OnAplicationQuit() {
		PlayerPrefs.DeleteAll();
	}

    void OnEnable() {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += StateChange;
#endif

    }
#if UNITY_EDITOR
    void StateChange(PlayModeStateChange state)  {
        PlayerPrefs.DeleteAll();
	}

#endif
}
