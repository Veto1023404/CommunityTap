using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TapUpgrade : MonoBehaviour {

	public GameManager gm;
	
	public Text     info;
    public double   cost;
	public int      amount;
	public string   name;
    public double   baseCost;
    public double   baseDamage;
	
	private Color afford = Color.cyan;
	private Color normal = Color.white;
	
	void Start () {
        cost = Math.Round(baseCost * Math.Pow (1.22f, amount), 2);
	}
	
	void Update () {
		info.text = name + " " + "(" + amount + ")" + " " + 
		"\nCost:" + " " + cost.ToString("n0") + 
        "\nDamage:" + " " + (gm.tapDamage + baseDamage);
		
		if (gm.gold >= cost)
			GetComponent<Image>().color = afford;
		else
			GetComponent<Image>().color = normal;
	}
	
	public void PurchasedUpgrade () {
		if (gm.gold >= cost) {
			gm.gold -= cost;
			amount++;
			gm.tapDamage += baseDamage;
			cost = Math.Round(baseCost * Math.Pow (1.22f, amount));
		}
	}
}