using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroDPS : MonoBehaviour {

	public GameManager gm;
	
	public Text info;
	public float cost;
	public int amount;
	public float dps;
	public string name;
	private float baseCost;
	private float baseDPS;
	
	private Color afford = Color.cyan;
	private Color normal = Color.white;
	
	void Start () {
		baseCost = cost;
		baseDPS = dps;
	}
	
	void Update () {
		info.text = name + " " + "(" + amount + ")" + " " + 
					"\nCost:" + " " + cost.ToString("n0") + 
					"\nDPS:" + " " + dps.ToString("n1");
			
		if (gm.gold >= cost)
			GetComponent<Image>().color = afford;
		else
			GetComponent<Image>().color = normal;
	}
	
	// Applies to the item
	public void PurchasedUpgrade () {
		if (gm.gold >= cost) {
			gm.gold -= cost;
			amount++;
			dps = baseDPS * Mathf.Pow (1.18f, amount);
			cost = Mathf.Round(baseCost * Mathf.Pow(1.22f, amount));
		}
	}
}