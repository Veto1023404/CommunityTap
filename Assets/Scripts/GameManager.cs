using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Currency 
	public float veto;
	public int vetoDropped;
	public Text vetoDisplay;

	// Damage
	public float tapDamage;
	public int dps;
	public Text damageDisplay;
	
	// Heroes
	public Hero[] heroes;
	
	// Health
	public float health;
	public float maxHealth;
	private float baseHealth;
	public Image healthBar;
	public Text healthDisplay;
	
	// Monster 
	public Text monsterDisplay;
	private int monstersKilled;
	
	// Rounds
	public int stageAmount;
	public int currentStage;
	public int maxStage;
	
	void Start () {
		StartCoroutine (AutoTick ());
		baseHealth = maxHealth;	
	}
	
	void Update () {
		vetoDisplay.text = veto.ToString("n0");
		healthDisplay.text = health.ToString("n0");
		damageDisplay.text = tapDamage.ToString("n0") + " " + "damage";
		monsterDisplay.text = monstersKilled.ToString() + " " + "/" + " " + "12";
		
		healthBar.GetComponent<Image>().fillAmount = health / maxHealth;

		// healthBar.rectTransform.rect.width = maxHealth;
				
		if (health <= 0) {
			maxHealth = baseHealth * Mathf.Pow (1.2f, monstersKilled);
			health = 0;
			veto += vetoDropped;
			monstersKilled++;
			health = maxHealth;
		}
	}
	
	public float GetDPS () {
		float tick = 0;
		
		foreach (Hero hero in heroes) {
			tick += hero.amount * hero.dps;
		}
		return tick;
	}
	
	public void AutoDPS () {
		health -= GetDPS () / 10;
	}
	
	IEnumerator AutoTick () {
		while (true) {
			AutoDPS ();
			yield return new WaitForSeconds(0.20f);
		}
	}
}