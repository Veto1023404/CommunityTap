using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	//Canvas
	public GameObject 			Game;

	// Currency 
	public double 				gold;
	public int					vetoDropped;
	public Text					vetoDisplay;
	private List<GameObject>	goldcoins = new List<GameObject>();

	// Damage
	public float 				tapDamage;
	public int 					dps;
	public Text 				damageDisplay;

	// Heroes
	public 	List<GameObject>	heroes;
	public	Sprite 				ButtonImage;
	public	Font				TextFont;
	private Transform			panel;

	// Health
	public Image 				healthBar;
	public Text 				healthDisplay;
	
	// Monster 
	public Monster 				monster;
	public Text 				monsterDisplay;
	private int 				monstersKilled;
	private MonsterType 		NewMonsterType;

	// Rounds
	public StageManager 		stageManager;
	public float				bossTimer;

	void Start () {
		stageManager = new StageManager (this);
		monster = new Monster (stageManager.currentStage, MonsterType.NORMAL, this);
		bossTimer = 30;
		panel = GameObject.Find("Content Panel").transform;
		heroes = new List<GameObject> ();
		for (int i = 0; i < 4; i++)
			heroes.Add(CreateButton());
		StartCoroutine (AutoTick ());
	}

	GameObject CreateButton()
	{
		GameObject button = new GameObject();
		GameObject buttonText = new GameObject ();
		button.name = "Hero Button";
		buttonText.name = "Button Text";
		button.transform.parent = panel;
		button.AddComponent<RectTransform> ();
		button.AddComponent<Image> ();
		button.GetComponent<Image> ().sprite = ButtonImage;
		button.AddComponent<Button> ();
		button.AddComponent<LayoutElement> ();
		button.GetComponent<LayoutElement> ().minHeight = 100;
		buttonText.transform.parent = button.transform;
		buttonText.AddComponent<Text> ();
		buttonText.GetComponent<Text> ().text = "Hi i will be a hero";
		buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100) ;
		buttonText.GetComponent<Text> ().font = TextFont;
		buttonText.GetComponent<Text> ().color = Color.black;
		buttonText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		button.AddComponent<Hero> ();


		return button;
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
			goldcoins.Add(Instantiate(Resources.Load("GoldCoin"), Vector3.zero, Quaternion.identity) as GameObject);
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
		float	tick = 0;
		Hero	script;

		foreach (GameObject hero in heroes) {
			if (script = hero.GetComponent<Hero>())
			tick += script.amount * script.dps;
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