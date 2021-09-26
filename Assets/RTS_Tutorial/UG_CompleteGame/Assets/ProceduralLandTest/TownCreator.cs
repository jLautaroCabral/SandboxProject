using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCreator : MonoBehaviour {

	public int width, height;
	public Color buildingColor, roadColor;
	public Color edgeColor;
	Texture2D textureGenerated,disp;

	public int numberOfBuildings = 5;

	float originalWidth = 1920.0f; //scaleing will make the gui proportional to what you see onscreen no matter the resolution, will cause stretching
	float originalHeight = 1080.0f;
	Vector3 scale;

	public void initialise(int width,int height){
		this.width = width;
		this.height = height;
		textureGenerated = new Texture2D (width, height, TextureFormat.ARGB32, false, false);

		numberOfBuildings = (width * height)/100;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				textureGenerated.SetPixel (x, y, Color.black);
			}
		}

	}


	void drawBuildings()
	{
		

		for (int b = 0; b < numberOfBuildings; b++) {

			int widthOfBuilding = Random.Range (3, 7);
			int heightOfBuilding = Random.Range (3, 7);
			int x = Random.Range (1, (width-1) - widthOfBuilding);
			int y = Random.Range (1, (height-1) - heightOfBuilding);

			List<Color> clList = new List<Color> ();

			bool valid = true;

			for (int x1 = x-1; x1 < x+widthOfBuilding+1; x1++) {
				for (int y1 = y-1; y1 < y + heightOfBuilding+1; y1++) {
					if (areWeOnEdge (x1, y1) == false) {
						clList.Add (textureGenerated.GetPixel (x1, y1));
					} else {
						valid = false;
					}
				}
			}

			if (clList.Contains (buildingColor) == false && valid==true) {
				for (int x1 = x; x1 < x + widthOfBuilding; x1++) {
					for (int y1 = y; y1 < y + heightOfBuilding; y1++) {
						textureGenerated.SetPixel (x1, y1, buildingColor);
					}
				}
			}
		}
	}

	void drawRoads()
	{
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if(textureGenerated.GetPixel(x,y)==Color.black && areWeOnEdge(x,y)==false){ 
					textureGenerated.SetPixel(x,y,roadColor);
				}
			}
		}
		textureGenerated.Apply();
	}

	void drawWalls()
	{
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if(areWeOnEdge(x,y)==true){ 
					textureGenerated.SetPixel(x,y,edgeColor);
				}
			}
		}
		textureGenerated.Apply ();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.O)) {
			createTown ();
		}
	}

	public void createTown()
	{
		initialise (30,30);
		drawBuildings();
		drawRoads();
		drawWalls();
		textureGenerated.filterMode = FilterMode.Point;
		disp = textureGenerated;
	}

	bool areWeOnEdge(int x, int y)
	{
		if (x <= 0 || y<=0|| x == width-1|| y == height-1) {
			return true;
		} else {
			return false;
		}
	}

	public Texture2D getTown(){
		return textureGenerated;
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
			GUI.DrawTexture (new Rect (1700, 500, 60, 60), disp);
		}

		GUI.matrix = svMat;
	}
}
