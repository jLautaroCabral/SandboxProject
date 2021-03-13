using UnityEngine;
using System.Collections;

public class Wheat : Resource{

	void Awake()
	{
		ResourceStore.me.food.Add (this.gameObject);
	}
}
