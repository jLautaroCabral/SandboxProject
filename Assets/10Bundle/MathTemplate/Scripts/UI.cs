using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

    public Game game;
    public Canvas rightCanvas, levelCanvas, gameCanvas, menuCanvas;

	public void LevelMenu() {
        levelCanvas.gameObject.SetActive(true);
        menuCanvas.gameObject.SetActive(false);
	}

	public void Play() {
        menuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);

		int level = PlayerPrefs.GetInt("Level");
		game.LoadLevel(level);
	}

	public void BackToLevelMenu() {
        levelCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
	}

	public void BackToMenu() {
        levelCanvas.gameObject.SetActive(false);
		menuCanvas.gameObject.SetActive(true);
	}

	public void NextLevel() {
        rightCanvas.gameObject.SetActive(false);
	}

	public void LoadLevel() {
        levelCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
	}

	public void RightAnswer() {
        rightCanvas.gameObject.SetActive(true);
	}

	public void Exit() {
		Application.Quit();
	}
}
