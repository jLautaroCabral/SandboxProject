using UnityEngine;
using System.Collections;



public class ResourceManager : MonoBehaviour {
	public static ResourceManager me;

	public int food = 0, wood = 0, stone = 0, iron = 0, gold = 0;

	void Awake()
	{
		me = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReduceResources(string resource,int amount)
	{
		alterResourceVal (resource, amount * -1);
	}

	public void IncreaseResources(string resource,int amount)
	{
		alterResourceVal (resource, amount);
	}

	public bool canWeConstructBuilding(Building toCheck)
	{ //single and means that all have to return true for it to return true
		return (resourceAmountCheck ("wood", toCheck.woodCost)
		&& resourceAmountCheck ("food", toCheck.foodCost)
		&& resourceAmountCheck ("stone", toCheck.stoneCost)
		&& resourceAmountCheck ("iron", toCheck.ironCost)
		&& resourceAmountCheck ("gold", toCheck.ironCost));
	}

	public bool canWeDoBuildingAction(BuildingAction ba)
	{
		return (resourceAmountCheck ("wood", ba.woodCost)
			&& resourceAmountCheck ("food", ba.foodCost)
			&& resourceAmountCheck ("stone", ba.stoneCost)
			&& resourceAmountCheck ("iron", ba.ironCost)
			&& resourceAmountCheck ("gold", ba.goldCost));
	}

	public void buildBuilding(Building toBuild)
	{
		alterResourceVal ("wood", toBuild.woodCost * -1);
		alterResourceVal ("food", toBuild.foodCost * -1);
		alterResourceVal ("stone", toBuild.stoneCost * -1);
		alterResourceVal ("iron", toBuild.ironCost * -1);
		alterResourceVal ("gold", toBuild.goldCost * -1);
	}


	public bool resourceAmountCheck(string resource,int amount)//just a check for if we have enough resources of a type, should be called before allowing for construction and recruitment
	{
		switch (resource) {
		case "food":
			if (food >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "wood":
			if (wood >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "stone":
			if (stone >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "iron":
			if (iron >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		case "gold":
			if (gold >= amount) {
				return true;
			} else {
				return false;
			}
			break;
		default:
			break;
		}

		return false;//if something goes wrong and an invalid resource is passed in
	}

	void alterResourceVal(string resource,int amount) //have one method to alter the value, just have to multiply decreases by -1 when passing it through
	{
		switch (resource) {
		case "food":
			food += amount;
				break;
		case "wood":
			wood += amount;
				break;
		case "stone":
			stone += amount;
				break;
		case "iron":
			iron += amount;
				break;
			case "gold":
			gold+=amount;
				break;
			default:
				break;
		}
	}

	float originalWidth = 1920.0f; //scaleing will make the gui proportional to what you see onscreen no matter the resolution, will cause stretching
	float originalHeight = 1080.0f;
	Vector3 scale;
	float dispWidth = 1920.0f / 6;
	void OnGUI()
	{
		GUI.depth = 0;
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z =1;
		var svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);

		for (int x = 0; x < 5; x++) {
			Rect pos = new Rect (0 + (dispWidth * x), 0, dispWidth, 100);

			switch (x) {
			case 0:
				GUI.Box (pos, "Food " + food.ToString ());
				break;
			case 1:
				GUI.Box (pos, "Wood " + wood.ToString ());
				break;
			case 2:
				GUI.Box (pos, "Stone " + stone.ToString ());
				break;
			case 3:
				GUI.Box (pos, "Iron " + iron.ToString ());
				break;
			case 4:
				GUI.Box (pos, "Gold " + gold.ToString ());
				break;
			default:
				break;
			}
		}

		GUI.matrix = svMat;
	}


}
