using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

	public GameManager  gm;
    public float        criticalChance;
    private double      critical;

    public void Clicked () {
        if (Random.Range(0f, 100f) <= criticalChance)
            critical = 8;
        else
            critical = 1;
        gm.monster.health -= gm.tapDamage * critical;
	}
}