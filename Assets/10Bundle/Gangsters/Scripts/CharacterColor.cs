using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour {

	public Color[] hairColors;
	public Color[] skinColors;
	public Color[] torsoColors;
    public Color[] legColors;
    public Color[] shoeColors;

	public SpriteRenderer hair, head, torso, arm, leg, shoe;

	void Start() {
		DoRandomColors();
	}

	void DoRandomColors() {
		hair.color = hairColors[Random.Range(0, hairColors.Length)];
        head.color = arm.color= skinColors[Random.Range(0, skinColors.Length)];
        torso.color = torsoColors[Random.Range(0, torsoColors.Length)];
        leg.color = legColors[Random.Range(0, legColors.Length)];
        shoe.color = shoeColors[Random.Range(0, shoeColors.Length)];
	}
}
