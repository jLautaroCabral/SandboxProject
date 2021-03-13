using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinentPlateGenerator : MonoBehaviour {

	public int width, height;
	public List<Color> coloursForLand;
	public Color edgeColor;
	Texture2D textureGenerated,disp;
	public int screenX,screenY;

	float originalWidth = 1920.0f; //scaleing will make the gui proportional to what you see onscreen no matter the resolution, will cause stretching
	float originalHeight = 1080.0f;
	Vector3 scale;


	public void initialise(int width,int height){
		this.width = width;
		this.height = height;
		if (textureGenerated == null) {
			textureGenerated = new Texture2D (width, height, TextureFormat.ARGB32, false, false);
			textureGenerated.filterMode = FilterMode.Point;
		} else {

			Debug.Log ("Kept last texture");
			Texture2D temp = new Texture2D(width, height, TextureFormat.ARGB32, false, false);


			int lastWidth = textureGenerated.width;
			int lastHeight = textureGenerated.height;

			for (int x = 0; x < width-1; x++) {
				for (int y = 0; y < height-1; y++) {
					temp.SetPixel (x, y, textureGenerated.GetPixel (x/2, y/2));
					temp.SetPixel ((x ) + 1, (y), textureGenerated.GetPixel (x/2, y/2));
					temp.SetPixel ((x ), (y )+1, textureGenerated.GetPixel (x/2, y/2));
					temp.SetPixel ((x )+1, (y )+1, textureGenerated.GetPixel (x/2, y/2));
				}
			}
			temp.filterMode = FilterMode.Point;
			temp.Apply ();
			textureGenerated = temp;
		}
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				textureGenerated.SetPixel (x, y, Color.black);
			}
		}

		generatePlates ();
	}

	void generatePlates()
	{
		for (int x = 0; x < (width / 100); x++) {
			plateRightToLeft ();

			//plateLeftToRight ();

			//plateBottomToTop ();

			plateTopToBottom ();

			plateDiagBottomLeft ();
			plateDiagBottomRight ();
		}
		disp = textureGenerated;
	}


	void plateTopToBottom()
	{
		int xCoord = Random.Range(2,width-2);
		int yCoord = height-2;

		while (areWeOnEdge (xCoord, yCoord) == false) {

			int r = Random.Range (0, 120);

			if (r < 60) {
				if (r < 30) {
					xCoord--;
				}
				else{
					xCoord++;
				}
			} else {
				yCoord--;
			}
			textureGenerated.SetPixel (xCoord, yCoord, Color.white);

		}
		textureGenerated.Apply ();
	}

	void plateBottomToTop()
	{
		int xCoord = Random.Range(2,width-2);
		int yCoord = 2;

		while (areWeOnEdge (xCoord, yCoord) == false) {

			int r = Random.Range (0, 120);

			if (r < 60) {
				if (r < 30) {
					xCoord--;
				}
				else{
					xCoord++;
				}
			} else {
				yCoord++;
			}
			textureGenerated.SetPixel (xCoord, yCoord, Color.white);

		}
		textureGenerated.Apply ();
	}

	void plateLeftToRight()
	{
		int xCoord = 2;
		int yCoord = Random.Range (2, height - 2);

		while (areWeOnEdge (xCoord, yCoord) == false) {

			int r = Random.Range (0, 120);

			if (r < 60) {
				if (r < 30) {
					yCoord--;
				}
				else{
					yCoord++;
				}
			} else {
				xCoord++;
			}
			textureGenerated.SetPixel (xCoord, yCoord, Color.white);

		}
		textureGenerated.Apply ();
	}

	void plateRightToLeft()
	{
		int xCoord = width-2;
		int yCoord = Random.Range (2, height - 2);

		while (areWeOnEdge (xCoord, yCoord) == false) {
			int r = Random.Range (0, 120);

			if (r < 60) {
				if (r < 30) {
					yCoord--;
				}
				else{
					yCoord++;
				}
			} else {
				xCoord--;
			}
			textureGenerated.SetPixel (xCoord, yCoord, Color.white);

		}
		textureGenerated.Apply ();
	}

	void plateDiagBottomLeft()
	{
		int xCoord = 2;
		int yCoord = 2;

		while (areWeOnEdge (xCoord, yCoord) == false) {
			int r = Random.Range (0, 120);

			if (r < 60) {
				if (r < 30) {
					yCoord+=2;
				}
				else{
					yCoord++;
				}
			} else {
				xCoord-=2;
			}
			textureGenerated.SetPixel (xCoord, yCoord, Color.white);

		}
		textureGenerated.Apply ();
	}

	void plateDiagBottomRight()
	{
		int xCoord = width-2;
		int yCoord = 2;

		while (areWeOnEdge (xCoord, yCoord) == false) {
			int r = Random.Range (0, 120);

			if (r < 60) {
				if (r < 30) {
					yCoord+=2;
				}
				else{
					yCoord++;
				}
			} else {
				xCoord-=2;
			}
			textureGenerated.SetPixel (xCoord, yCoord, Color.white);

		}
		textureGenerated.Apply ();
	}

	public Texture2D getTexture()
	{
		return textureGenerated;
	}
	//void plateTopToBottom

	bool areWeOnEdge(int x, int y)
	{
		if (x <= 0 || y<=0|| x == width-1|| y == height-1) {
			return true;
		} else {
			return false;
		}
	}

	void OnGUI()
	{
		GUI.depth = 0;
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z =1;
		var svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);

		if (disp == null) {

		} else {
			GUI.DrawTexture (new Rect (1000, 0, 500,500), disp);
		}

		GUI.matrix = svMat;
	}
}
