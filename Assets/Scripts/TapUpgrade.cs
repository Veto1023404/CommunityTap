using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TapUpgrade : MonoBehaviour {

	public GameManager gm;
	
	public Text info;
	public float cost;
	public int amount;
	public float damage;
	public string name;
	private float baseCost;
	private float baseDamage;
	
	private Color afford = Color.cyan;
	private Color normal = Color.white;
	
	void Start () {
		baseCost = cost;
		baseDamage = damage;
	}
	
	void Update () {
		info.text = name + " " + "(" + amount + ")" + " " + 
		"\nCost:" + " " + cost.ToString("n0") + 
		"\nDamage:" + " " + damage.ToString("n1");
		
		if (gm.gold >= cost)
			GetComponent<Image>().color = afford;
		else
			GetComponent<Image>().color = normal;
	}
	
	public void PurchasedUpgrade () {
		if (gm.gold >= cost) {
			gm.gold -= cost;
			amount++;
			gm.tapDamage += damage;
			damage = baseDamage * Mathf.Pow (1.16f, amount);
			cost = Mathf.Round(baseCost * Mathf.Pow (1.22f, amount));
		}
	}
}