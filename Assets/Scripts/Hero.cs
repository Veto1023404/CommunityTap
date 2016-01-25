using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hero : MonoBehaviour {
	
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
		baseCost = cost = 10;
		baseDPS = dps = 2;
		name = "Sachulus the Great";
		info = this.gameObject.GetComponentInChildren<Text>();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
		GetComponent<Button>().onClick.AddListener(delegate { PurchasedUpgrade(); });
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

	public void SetupButton () {
		var button = transform.GetComponent<Button>();
		button.onClick.AddListener(this.PurchasedUpgrade);
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