using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created this script (Give credit to unity page on perlin noise)
//changed grid generator to use the texture
//changed execution order of scripts to make upgrades script be called before all the others
//added other tile types
//changed logic in Action_Scout
//removed colour change from tile master class in OnDeselect
public class LevelGenerator : MonoBehaviour {
	public static LevelGenerator me;
	public int pixWidth;
	public int pixHeight;
	public float xOrg;
	public float yOrg;
	public float scale = 1.0F;
	public int numberOfColours;
	public Texture2D dispTex,thresholdedTex,color,color2,coast;
	public Color grass,mountain,sea;
	[Range(0,1)]
	public float mountainLimit, grassLimit, seaLimit;

	private Color[] pix;

	public List<Color> colorsInThresh;
	public bool drawMaps = false;

	void Awake() {
		me = this;
		//xOrg = Random.Range (10, 100);
		//yOrg = Random.Range (10, 100);
		colorsInThresh = new List<Color> ();
		dispTex = CalcNoise ();
		thresholdedTex = thresholdedNoise ();
		color = ColorMap ();
		color2 = ColorMapPlusOffset ();
		coast = CoastalMap ();
	}

	Texture2D CalcNoise() {
		Texture2D noiseTex=new Texture2D(pixWidth, pixHeight);
		pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);
				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix);
		noiseTex.Apply();
		return noiseTex;
	}

	public Texture2D thresholdedNoise()
	{
		Texture2D noiseTex=new Texture2D(pixWidth, pixHeight);
		pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);

				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix);
		noiseTex.Apply();

		Texture2D t = noiseTex;
		for (int x1 = 0; x1 < t.width; x1++) {
			for (int y1 = 0; y1 < t.height; y1++) {
				Color cl = t.GetPixel (x1, y1);
				float r = cl.r;
				float g = cl.g;
				float b = cl.b;
				float a = cl.a;


				r *= numberOfColours;
				g *= numberOfColours;
				b *= numberOfColours;


				r = Mathf.Round (r);
				g = Mathf.Round (g);
				b = Mathf.Round (b);



				r /= numberOfColours;
				g /= numberOfColours;
				b /= numberOfColours;



				Color col = new Color (r, g, b, a);
				if (colorsInThresh.Contains (col) == false) {
					colorsInThresh.Add (col);
				}
				t.SetPixel(x1,y1,col);
			}
		}
		colorsInThresh=sortColourList (colorsInThresh);
		t.Apply ();
		return t;

	}

	Texture2D ColorMap()
	{

		//Generate original perlin noise texture
		Texture2D noiseTex=new Texture2D(pixWidth, pixHeight);
		pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord); //gives us a value between 0 and one

				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample); //gives us a shade of grey
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix); //creates the texture
		noiseTex.Apply();

		//go through each pixel and threshold to the number of colours, unity doesn't have a round to 1dp function (I think)
		Texture2D t = noiseTex;
		for (int x1 = 0; x1 < t.width; x1++) {
			for (int y1 = 0; y1 < t.height; y1++) {
				Color cl = t.GetPixel (x1, y1);
				float r = cl.r;
				float g = cl.g;
				float b = cl.b;
				float a = cl.a;

				r *= numberOfColours;
				g *= numberOfColours;
				b *= numberOfColours;

				r = Mathf.Round (r);
				g = Mathf.Round (g);
				b = Mathf.Round (b);

				r /= numberOfColours;
				g /= numberOfColours;
				b /= numberOfColours;


				Color col = new Color (r, g, b, a);
				if (colorsInThresh.Contains (col) == false) { //store the colours in a list for later reference
					colorsInThresh.Add (col);
				}
				t.SetPixel(x1,y1,col);
			}
		}
		colorsInThresh = sortColourList (colorsInThresh);
		t.Apply ();


		//go through each pixel and colour it in based on what its colour is originaly (based on three floats)
		for (int x2 = 0; x2 < t.width; x2++) {
			for (int y2 = 0; y2 < t.height; y2++) {
				Color cl = t.GetPixel (x2, y2);
				cl.a = 0;
				Color cl2;
				if (cl.r <= mountainLimit) {
					cl2= Color.gray - cl;
					t.SetPixel (x2, y2, cl2);
				} else if (cl.r > mountainLimit && cl.r <= grassLimit) {
					cl2 = Color.green - cl;
					t.SetPixel (x2, y2, cl2);
				} else if (cl.r > grassLimit && cl.r <= seaLimit) {
					cl2 = Color.blue - cl;
					t.SetPixel (x2, y2, Color.blue- cl);
				} else if (cl.r > seaLimit) {
					cl2 = Color.yellow - cl;
					t.SetPixel (x2, y2, cl2);
				}


			}
		}


		t.Apply ();
		return t;

	}

	Texture2D ColorMapPlusOffset()
	{
		Texture2D noiseTex=new Texture2D(pixWidth, pixHeight);
		pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = (xOrg+10) + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);

				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix);
		noiseTex.Apply();

		Texture2D t = noiseTex;
		for (int x1 = 0; x1 < t.width; x1++) {
			for (int y1 = 0; y1 < t.height; y1++) {
				Color cl = t.GetPixel (x1, y1);
				float r = cl.r;
				float g = cl.g;
				float b = cl.b;
				float a = cl.a;

				r *= numberOfColours;
				g *= numberOfColours;
				b *= numberOfColours;

				r = Mathf.Round (r);
				g = Mathf.Round (g);
				b = Mathf.Round (b);

				r /= numberOfColours;
				g /= numberOfColours;
				b /= numberOfColours;


				Color col = new Color (r, g, b, a);
				if (colorsInThresh.Contains (col) == false) {
					colorsInThresh.Add (col);
				}
				t.SetPixel(x1,y1,col);
			}
		}
		t.Apply ();

		for (int x2 = 0; x2 < t.width; x2++) {
			for (int y2 = 0; y2 < t.height; y2++) {
				Color cl = t.GetPixel (x2, y2);
				cl.a = 0;
				if (cl.r <= 0.0f) {

					t.SetPixel (x2, y2, Color.gray - cl);
				} else if (cl.r > 0.0f && cl.r <= 0.3f) {
					t.SetPixel (x2, y2, Color.green- cl);
				} else {
					t.SetPixel (x2, y2, Color.blue- cl);
				}


			}
		}
		t.Apply ();
		return t;

	}

	Texture2D CoastalMap()
	{
		mountain.a = 1;
		sea.a = 1;
		grass.a = 1;
		Texture2D noiseTex=new Texture2D(pixWidth, pixHeight);
		pix = new Color[noiseTex.width * noiseTex.height];

		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);

				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix);
		noiseTex.Apply();

		Texture2D t = noiseTex;
		for (int x1 = 0; x1 < t.width; x1++) {
			for (int y1 = 0; y1 < t.height; y1++) {
				Color cl = t.GetPixel (x1, y1);
				float r = cl.r;
				float g = cl.g;
				float b = cl.b;
				float a = cl.a;

				r *= numberOfColours;
				g *= numberOfColours;
				b *= numberOfColours;

				r = Mathf.Round (r);
				g = Mathf.Round (g);
				b = Mathf.Round (b);

				r /= numberOfColours;
				g /= numberOfColours;
				b /= numberOfColours;


				Color col = new Color (r, g, b, a);
				if (colorsInThresh.Contains (col) == false) {
					colorsInThresh.Add (col);
				}
				t.SetPixel(x1,y1,col);
			}
		}
		t.Apply ();

		for (int x2 = 0; x2 < t.width; x2++) {
			for (int y2 = 0; y2 < t.height; y2++) {
				Color cl = t.GetPixel (x2, y2);
				cl.a = 0;

				if (cl.r <= mountainLimit) {

					t.SetPixel (x2, y2, mountain);
				} else if (cl.r > mountainLimit && cl.r <= grassLimit) {
					t.SetPixel (x2, y2, grass);
				} else if (cl.r > grassLimit && cl.r <= seaLimit) {
					t.SetPixel (x2, y2, sea);
				} else if (cl.r > seaLimit) {
					t.SetPixel (x2, y2, Color.yellow- cl);
				}

				//in coastal region
				if (x2 > (t.width - (t.width * 0.5f)) && x2 < (t.width - (t.width * 0.2f))) {
					if (cl.grayscale.Equals (colorsInThresh [0].grayscale) || cl.grayscale.Equals (colorsInThresh [1].grayscale) || cl.grayscale.Equals (colorsInThresh [3].grayscale)) {
						//do nothing its a mountain
						t.SetPixel (x2, y2, grass);
					} else {
						t.SetPixel (x2, y2, sea);
					}
				}

				if (x2 > (t.width - (t.width * 0.2f))) {
					if (cl.grayscale.Equals (colorsInThresh [0].grayscale) || cl.grayscale.Equals (colorsInThresh [1].grayscale)) {
						//do nothing its a mountain
						t.SetPixel (x2, y2, grass);
					} else {
						t.SetPixel (x2, y2, sea);
					}
				}

				if (x2 > (t.width - (t.width * 0.1f))) {
					if (cl.grayscale.Equals (colorsInThresh [0].grayscale)) {
						//do nothing its a mountain
					} else {
						t.SetPixel (x2, y2, sea);
					}
				}

			}


		}
		t.Apply ();
		return t;

	}



	List<Color> sortColourList(List<Color> toSort) //takes a list of grayscale colours and sorts them by intensity (useful for referencing to we can assume the lower the index the darker the colour)
	{
		List<float> intensityVals = new List<float> ();
		foreach (Color c in toSort) {
			intensityVals.Add (c.grayscale);
		}
		intensityVals.Sort ();

		List<Color> orderedList = new List<Color> ();
		foreach (float f in intensityVals) {
			orderedList.Add (new Color (f, f, f, 1));
		}
		return orderedList;
	}



	void OnGUI()
	{
		if (drawMaps == true) {
			GUI.DrawTexture (new Rect (0, 500, 250, 250), dispTex);
			GUI.DrawTexture (new Rect (250, 500, 250, 250), thresholdedTex);
			GUI.DrawTexture (new Rect (500, 500, 250, 250), color);
			GUI.DrawTexture (new Rect (750, 500, 250, 250), color2);
		}
		//GUI.DrawTexture (new Rect (0, 250, 250, 250), coast);
	}
}
