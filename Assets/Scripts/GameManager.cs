using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Currency 
	public double gold;
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
	public StageManager stageManager;
	public float		bossTimer;

	void Start () {
		stageManager = new StageManager (this);
		monster = new Monster (stageManager.currentStage, MonsterType.NORMAL, this);
		bossTimer = 30;
		StartCoroutine (AutoTick ());
	}
	
	void Update () {
		vetoDisplay.text = gold.ToString("n0") + "\n" + stageManager.currentStage.ToString();
		healthDisplay.text = monster.health.ToString("n0");
		damageDisplay.text = tapDamage.ToString("n0") + " " + "damage";
		monsterDisplay.text = stageManager.stageMonsterCounter.ToString() + " " + "/" + " " + "12";
		
		healthBar.GetComponent<Image>().fillAmount = (float)(monster.health / monster.maxHealth);

		if (monster.type == MonsterType.BOSS) {
			bossTimer -= Time.deltaTime;
			vetoDisplay.text += " " + bossTimer.ToString("F2"); 
			if (bossTimer <= 0) {
				NewMonsterType = stageManager.FailedBoss();
				monster = new Monster(stageManager.currentStage, NewMonsterType, this);
			}
		}
				
		if (monster.health <= 0) {
			NewMonsterType = stageManager.GetNewMonsterType(monster.type);
			gold += GetGoldFromMonster(monster.type, stageManager.currentStage);
			monster = new Monster(stageManager.currentStage, NewMonsterType, this);
		}
	}

	public float GetGoldFromMonster (MonsterType type, int stage) {
		if (type == MonsterType.NORMAL)
			return (1 * Mathf.Pow (1.1f, stage));
		else if (type == MonsterType.MINIBOSS)
			return (1 * Mathf.Pow (1.1f, stage) * 1.5f);
		else
			return (1 * Mathf.Pow (1.1f, stage) * 2f);
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