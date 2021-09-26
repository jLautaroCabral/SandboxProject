using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour {

	public int width, height;
	public List<Color> coloursForLand;
	public Color edgeColor;
	Texture2D textureGenerated,disp;
	int startF = 2;
	public landType myType;
	public int screenX,screenY;

	float originalWidth = 1920.0f; //scaleing will make the gui proportional to what you see onscreen no matter the resolution, will cause stretching
	float originalHeight = 1080.0f;
	Vector3 scale;

	TownCreator tc;

	ContinentPlateGenerator myPlates;



	void initalise()
	{
		tc = this.GetComponent<TownCreator> ();
		myPlates = this.gameObject.AddComponent<ContinentPlateGenerator> ();
		edgeColor = Color.blue;

		if (coloursForLand.Count == 0) {
			for (int x = 0; x < 10; x++) {
				coloursForLand.Add (new Color (0.1f, Random.Range(0.1f,0.9f), 0.1f, 1.0f));
				//coloursForLand.Add (new Color (Random.Range(0.1f,0.9f), Random.Range(0.1f,0.9f),Random.Range(0.1f,0.9f), 1.0f));
			}

		}

		if (width < coloursForLand.Count) {
			width  = coloursForLand.Count;
		}

		if (height < coloursForLand.Count) {
			height = coloursForLand.Count;
		}
	}

	void createStartingTexture()
	{
		textureGenerated = new Texture2D(width,height,TextureFormat.ARGB32, false,false);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (myType == landType.continent) {
					if (areWeOnEdge (x, y) == true) {
						textureGenerated.SetPixel (x, y, edgeColor);
					} else {
						int r = Random.Range (0, coloursForLand.Count);
						textureGenerated.SetPixel (x, y, coloursForLand [r]);
					}
				} else if (myType == landType.islands) {
					if (areWeOnEdge (x, y) == true) {
						textureGenerated.SetPixel (x, y, edgeColor);
					} else {
						int r2 = Random.Range (0, 100);
						if (r2 < 30) {
							int r = Random.Range (0, coloursForLand.Count);
							textureGenerated.SetPixel (x, y, coloursForLand [r]);
						} else {
							textureGenerated.SetPixel (x, y, edgeColor);
						}
					}
				} else {
					int r = Random.Range (0, coloursForLand.Count);
					textureGenerated.SetPixel (x, y, coloursForLand [r]);
				}
			}
		}
		textureGenerated.filterMode = FilterMode.Point;
		textureGenerated.anisoLevel = 0;
		textureGenerated.Apply ();
		disp = textureGenerated;
	}


	void increaseTextureSize(int originalWidth,int originalHeight)
	{
		width = (originalWidth*2)-1;
		height =(originalHeight*2)-1;
		Texture2D temp = new Texture2D(width,height,TextureFormat.ARGB32, false,false);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				temp.SetPixel (x, y, Color.black);
			}
		}

		for (int x = 0; x < originalWidth; x++) {
			for (int y = 0; y < originalHeight; y++) {
				temp.SetPixel ((x * 2) , (y * 2) , textureGenerated.GetPixel (x, y));
			}
		}

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (temp.GetPixel (x, y).Equals(Color.black)) {
					List<Color> neighbours = getNeighbours (x, y, width, height,temp);
					temp.SetPixel (x, y, neighbours [Random.Range (0, neighbours.Count)]);
				}
			}
		}
		temp.Apply ();
		textureGenerated = temp;
		textureGenerated.filterMode = FilterMode.Point;
		textureGenerated.anisoLevel = 0;
		textureGenerated.Apply ();
		if (width > 150) {
			textureGenerated = erodeImage (textureGenerated);
			//drawMountains (ref textureGenerated);
		}

		disp = textureGenerated;
	}

	List<Color> getNeighbours(int x,int y,int width,int height,Texture2D textureGenerated)
	{
		List<Color> retVal = new List<Color> ();

		if (x == 0) {
			if (y == 0) {
				retVal.Add (textureGenerated.GetPixel (x + 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y+1));
			} else if (y==height - 1) {
				retVal.Add (textureGenerated.GetPixel (x + 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y-1));
			} else {
				retVal.Add (textureGenerated.GetPixel (x + 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y-1));
				retVal.Add (textureGenerated.GetPixel (x, y+1));
			}
		} else if (x == width - 1) {
			if (y == 0) {
				retVal.Add (textureGenerated.GetPixel (x - 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y+1));
			} else if (y==height - 1) {
				retVal.Add (textureGenerated.GetPixel (x - 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y-1));
			} else {
				retVal.Add (textureGenerated.GetPixel (x - 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y-1));
				retVal.Add (textureGenerated.GetPixel (x, y+1));
			}
		} else {
			if (y == 0) {
				retVal.Add (textureGenerated.GetPixel (x - 1, y));
				retVal.Add (textureGenerated.GetPixel (x + 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y+1));
			} else if (y==height - 1) {
				retVal.Add (textureGenerated.GetPixel (x - 1, y));
				retVal.Add (textureGenerated.GetPixel (x + 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y-1));
			} else {
				retVal.Add (textureGenerated.GetPixel (x - 1, y));
				retVal.Add (textureGenerated.GetPixel (x + 1, y));
				retVal.Add (textureGenerated.GetPixel (x, y-1));
				retVal.Add (textureGenerated.GetPixel (x, y+1));
			}
		}
		List<Color> filterdColours = new List<Color>();

		foreach(Color cl in retVal)
		{
			if (cl.Equals (Color.black) == false) {
				filterdColours.Add (cl);

			}
		}


		return filterdColours;
	}

	bool areWeOnEdge(int x, int y)
	{
		if (x <= 0 || y<=0|| x == width-1|| y == height-1) {
			return true;
		} else {
			return false;
		}
	}

	Texture2D getMountain()
	{
		Texture2D image = new Texture2D(width,height,TextureFormat.ARGB32, false,false);
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				//if (areColoursTheSame (image.GetPixel (x, y), edgeColor) == true) {
				if(textureGenerated.GetPixel(x,y)==edgeColor){
					//	Debug.LogError ("Trying to draw on water, breaking");
					image.SetPixel (x, y, Color.clear);
				} else {
					if (myPlates.getTexture ().GetPixel (x, y).Equals (Color.white)) {
						image.SetPixel (x, y, new Color (0.75f, 0.75f, 0.75f, 1.0f));
					} else if (getNeighbours (x, y, width, height, myPlates.getTexture ()).Contains (Color.white)) {
						image.SetPixel (x, y, Color.grey);
					} else {
						image.SetPixel (x, y, Color.clear);
					}
				}

			}
		}
		//image.alphaIsTransparency = true;
		image.filterMode = FilterMode.Point;
		image.Apply ();
		return image;
	}

	void drawTowns()
	{
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (textureGenerated.GetPixel (x, y) != edgeColor) {
					int r = Random.Range (0, 100000);

					if (r < 3) {
						tc.createTown ();
						Texture2D town = tc.getTown ();
						int townWidth = town.width;
						int townHeight = town.height;

						for (int x1 = x; x1 < x + townWidth; x1++) {
							for (int y1 = y; y1 < y + townHeight; y1++) {
								textureGenerated.SetPixel (x1, y1, town.GetPixel (x1 - x, y1 - x));
							}
						}
					}
				}

			}
		}
		textureGenerated.Apply ();
	}

	Texture2D erodeImage(Texture2D image)
	{
		Texture2D newImage = new Texture2D(width,height,TextureFormat.ARGB32, false,false);


		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				List<Color> neighbours = getNeighbours (x, y, width, height, image);
				int[] colCount = new int[4];


				for (int cInd = 0; cInd < neighbours.Count; cInd++) {
					foreach (Color cl in neighbours) {
						if (cl.Equals (neighbours [cInd])) {
							colCount [cInd]++;
						}
					}
				}
				int highestValue = 0;
				int highestIndex = 0;

				for (int intInd = 0; intInd < colCount.Length; intInd++) {
					if (colCount[intInd] > highestValue) {
						highestValue = colCount [intInd];
						highestIndex = intInd;
					}
				}

				newImage.SetPixel (x, y, neighbours [highestIndex]);

			}
		}
		newImage.filterMode = FilterMode.Point;
		newImage.Apply ();
		return newImage;
	}



	bool areColoursTheSame(Color a,Color b)
	{
		//Debug.Log (a.ToString () + " || " + b.ToString ());
		if (Mathf.Abs(a.r - b.r)<0.01f && Mathf.Abs(a.g - b.g)<0.01f && Mathf.Abs(a.b - b.b)<0.01f) {
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
				GUI.DrawTexture (new Rect (screenX, screenY, 1000, 1000), disp);
				//GUI.DrawTexture (new Rect (screenX, screenY, 500, 500), getMountain());
			}

		GUI.matrix = svMat;
	}


	void Update()
	{
		if (Input.GetKeyDown (KeyCode.I)) {
			if (disp == null) {
				initalise ();
				createStartingTexture ();
				//myPlates.initialise (width, height);

			} else {
				if (width < 500) {
					increaseTextureSize (width, height);

					//myPlates.initialise (width, height);

				} else {
					drawTowns ();
				}
			}
		}
	}
}

public enum landType{
	continent,
	land,
	islands
}
