using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioColor : MonoBehaviour {

	public Color[] buildColors;
	public SpriteRenderer[] buildings;

	void Start() {
		RandCity();
	}

	void RandCity() {
		for (int i = 0; i < buildings.Length; i++) {
			int colorIndex = Random.Range(0, buildColors.Length);
			buildings[i].color = buildColors[colorIndex];
		}
	}
}
