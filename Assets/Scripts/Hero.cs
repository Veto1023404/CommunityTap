using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Hero : MonoBehaviour {
	
	public GameManager gm;
	
	public Text     info;
    public double   cost;
	public int      amount;
    public double   dps;
	public string   name;
    public double   baseCost;
    public double   baseDPS;
	
	private Color afford = Color.cyan;
	private Color normal = Color.white;
	
	void Start () {
		info = this.gameObject.GetComponentInChildren<Text>();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
		GetComponent<Button>().onClick.AddListener(delegate { PurchasedUpgrade(); });
	}

    public void Set(double newBaseCost, double newBaseDPS, string newName)
    {
        baseCost = newBaseCost;
        baseDPS = newBaseDPS;
        dps = baseDPS * Math.Pow (1.18f, amount);
        cost = Math.Round(baseCost * Math.Pow(1.22f, amount));
        name = newName;
    }
	
	void Update () {
		info.text = name + " " + "(" + amount + ")" + " " + 
					"\nCost:" + " " + cost.ToString() + 
					"\nDPS:" + " " + dps.ToString();
		if (gm.gold >= cost)
			GetComponent<Image>().color = afford;
		else
			GetComponent<Image>().color = normal;
	}

	public void SetupButton () {
		var button = transform.GetComponent<Button>();
		button.onClick.AddListener(this.PurchasedUpgrade);
	}

	// Applies to the item
	public void PurchasedUpgrade () {
		if (gm.gold >= cost) {
			gm.gold -= cost;
			amount++;
            dps = baseDPS * amount;
			cost = Math.Round(baseCost * Math.Pow(1.22f, amount));
		}
	}
}