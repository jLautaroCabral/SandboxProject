using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Create new question using the right mouse button in the project tab

[CreateAssetMenu(fileName = "New Riddle", menuName = "Riddle")]
public class Question : ScriptableObject {

	[TextArea(1, 10)]
	public string questionText;
	public int answer;
}
