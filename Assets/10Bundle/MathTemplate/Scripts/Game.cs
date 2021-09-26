using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public InputField resultDisplay;
	public Question[] questions; //ScriptableObject with all the math questions
	public UI uI;
	public Text wrongText;
	public Text questionText;
	public Text levelText;

	int currentLevel = 0;
	int userAnswer;

	//Read the value on the numbers buttons and display the result
	public void NumInput(int value) {
		resultDisplay.text += value.ToString();
	}

	// "X" button to clean answer field
	public void CleanDisplay() {
		resultDisplay.text = "";
	}

	// Check if the usser answer is correct
	public void CheckAnswer() {
        userAnswer = int.Parse(resultDisplay.text);
		if (userAnswer == questions[currentLevel].answer) {
			uI.RightAnswer();
		}
		else {
			StartCoroutine(WrongDisplay());
		}
	}

	//Text if the user answer is incorrect
	IEnumerator WrongDisplay() {
		wrongText.gameObject.SetActive(true);
		yield return new WaitForSeconds(1);
        wrongText.gameObject.SetActive(false);
	}

	//Next level button
	public void NextLevel() {
        uI.NextLevel();
		currentLevel++;
        LoadQuestion();
		PlayerPrefs.SetInt("Level", currentLevel);
	}

	//Load new level
	public void LoadLevel(int level) {
		currentLevel = level;
        LoadQuestion();
		uI.LoadLevel();
	}

	void LoadQuestion() {
		questionText.text = questions[currentLevel].questionText;
		levelText.text = "Level " + (currentLevel + 1);
        resultDisplay.text = "";
	}
}