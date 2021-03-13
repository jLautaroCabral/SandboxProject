using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCollisionCheck : MonoBehaviour {
	public static CursorCollisionCheck me;
	SpriteRenderer sr;
	BoxCollider2D myCol;
	void Awake()
	{
		me = this;
		sr = this.GetComponent<SpriteRenderer> ();
		myCol = this.GetComponent<BoxCollider2D> ();
	}

	public void setSprite(Sprite toSet)
	{
		sr.sprite = toSet;
		Vector2 size = toSet.bounds.size;
		myCol.size = size;
		myCol.offset = new Vector2 (0,0);
	}

	public bool overUnit = false;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Unit") {
			overUnit = true;
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Unit") {
			overUnit = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Unit") {
			overUnit = false;
		}
	}
}
