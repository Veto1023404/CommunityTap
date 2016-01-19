using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

	public GameManager gm;
	
	public void Clicked () {
		gm.monster.health -= gm.tapDamage;
	}
}