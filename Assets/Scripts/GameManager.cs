using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Currency 
	public float veto;
	public int	vetoDropped;
	public Text vetoDisplay;

	// Damage
	public float tapDamage;
	public int dps;
	public Text damageDisplay;

	// Heroes
	public Hero[] heroes;
	
	// Health
	public Image healthBar;
	public Text healthDisplay;
	
	// Monster 
	public Monster monster;
	public Text monsterDisplay;
	private int monstersKilled;
	private MonsterType NewMonsterType;

	// Rounds
	public StageManager stage_manager;

	void Start () {
		stage_manager = new StageManager ();
		monster = new Monster (stage_manager.currentStage, MonsterType.NORMAL);
		StartCoroutine (AutoTick ());
	}
	
	void Update () {
		vetoDisplay.text = veto.ToString("n0");
		healthDisplay.text = monster.health.ToString("n0");
		damageDisplay.text = tapDamage.ToString("n0") + " " + "damage";
		monsterDisplay.text = monstersKilled.ToString() + " " + "/" + " " + "12";
		
		healthBar.GetComponent<Image>().fillAmount = (float)(monster.health / monster.maxHealth);

		// healthBar.rectTransform.rect.width = maxHealth;
				
		if (monster.health <= 0) {
			NewMonsterType = stage_manager.GetNewMonsterType(monster.type);
			monster = new Monster(stage_manager.currentStage, NewMonsterType);
			veto += vetoDropped;

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
		monster.health -= GetDPS () / 10;
	}
	
	IEnumerator AutoTick () {
		while (true) {
			AutoDPS ();
			yield return new WaitForSeconds(0.20f);
		}
	}
}