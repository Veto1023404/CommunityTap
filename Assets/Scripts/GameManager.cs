using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

	//Canvas
	public GameObject 			Game;

	// Currency 
	public double 				gold;
	public Text					vetoDisplay;
	public	Sprite				CoinImage;
	private List<GameObject>	goldcoins = new List<GameObject>();


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
	private MonsterRank 		NewMonsterType;

	// Rounds
	public StageManager 		stageManager;
	public float				bossTimer;

    //Tap
    public GameObject           tapButton;
    public GameObject           tapUpgrade;
    public double               tapDamage;
    // Damage
    public int                  dps;
    public Text                 damageDisplay;

	public Transform			canvas;

    void Start () {
        panel = GameObject.Find("Content Panel").transform;
        canvas = GameObject.Find("Game").transform;
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            Load();
        }
        else
        {
            stageManager = new StageManager(this);
            monster = new Monster(stageManager.currentStage, MonsterRank.NORMAL, this);
            bossTimer = 30;
            heroes = new List<GameObject>();
            heroes.Add(CreateButton(10, 2, "Sachulus The Great"));
            heroes.Add(CreateButton(100, 20, "Vetolus The Great"));
            heroes.Add(CreateButton(1000, 200, "Johnulus The Great"));
            heroes.Add(CreateButton(10000, 2000, "Gjappulus The Great"));
            heroes.Add(CreateButton(100000, 20000, "Holymoonus The Great"));
            heroes.Add(CreateButton(1000000, 200000, "Lukalus The Great"));
        }
		StartCoroutine (AutoTick ());
	}

    public GameObject CreateButton(double newBaseCost, double newBaseDPS, string newName)
	{
		GameObject button = new GameObject();
		GameObject buttonText = new GameObject ();
		button.name = "Hero Button";
		buttonText.name = "Button Text";
		button.transform.parent = panel;
		button.AddComponent<RectTransform> ();
		button.GetComponent<RectTransform> ().localScale = new Vector3(1,1,1);
		button.AddComponent<Image> ();
		button.GetComponent<Image> ().sprite = ButtonImage;
		button.AddComponent<Button> ();
		button.AddComponent<LayoutElement> ();
		button.GetComponent<LayoutElement> ().minHeight = Screen.height / 10;
		button.GetComponent<LayoutElement> ().minWidth = Screen.width;
		buttonText.transform.parent = button.transform;
		buttonText.AddComponent<Text> ();
		buttonText.GetComponent<RectTransform> ().sizeDelta = new Vector2(button.GetComponent<LayoutElement> ().minWidth, button.GetComponent<LayoutElement> ().minHeight);
		buttonText.GetComponent<Text> ().text = "Hi i will be a hero";
		buttonText.GetComponent<Text> ().font = TextFont;
		buttonText.GetComponent<Text> ().fontSize = 24;
		buttonText.GetComponent<Text> ().color = Color.black;
		buttonText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		button.AddComponent<Hero> ();
        button.GetComponent<Hero> ().Set(newBaseCost, newBaseDPS, newName);

		return button;
	}

    public GameObject CreateButton()
    {
        GameObject button = new GameObject();
        GameObject buttonText = new GameObject ();
        button.name = "Hero Button";
        buttonText.name = "Button Text";
        button.transform.parent = panel;
        button.AddComponent<RectTransform> ();
        button.GetComponent<RectTransform> ().localScale = new Vector3(1,1,1);
        button.AddComponent<Image> ();
        button.GetComponent<Image> ().sprite = ButtonImage;
        button.AddComponent<Button> ();
        button.AddComponent<LayoutElement> ();
        button.GetComponent<LayoutElement> ().minHeight = Screen.height / 10;
        button.GetComponent<LayoutElement> ().minWidth = Screen.width;
        buttonText.transform.parent = button.transform;
        buttonText.AddComponent<Text> ();
        buttonText.GetComponent<RectTransform> ().sizeDelta = new Vector2(button.GetComponent<LayoutElement> ().minWidth, button.GetComponent<LayoutElement> ().minHeight);
        buttonText.GetComponent<Text> ().text = "Hi i will be a hero";
        buttonText.GetComponent<Text> ().font = TextFont;
        buttonText.GetComponent<Text> ().fontSize = 24;
        buttonText.GetComponent<Text> ().color = Color.black;
        buttonText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
        button.AddComponent<Hero> ();
        return button;
    }


    GameObject	CreateCoin(double value)
	{
		GameObject coin = new GameObject();

		coin.name = "Coin";
		coin.transform.parent = canvas;
		coin.AddComponent<RectTransform> ();
		coin.GetComponent<RectTransform> ().localScale = new Vector3(0.5f, 0.5f, 0.5f);
		coin.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3(UnityEngine.Random.Range((Screen.width / 4) * -1, (Screen.width / 4)), 370,0);
		coin.AddComponent<Image> ();
		coin.GetComponent<Image> ().sprite = CoinImage;
		coin.AddComponent<CoinDestroyer> ();
		coin.GetComponent<CoinDestroyer> ().SetValue(value);
		return coin;
	}

	void Update () {
		vetoDisplay.text = gold.ToString() + "\n" + stageManager.currentStage.ToString();
		healthDisplay.text = monster.health.ToString("n0");
		damageDisplay.text = tapDamage.ToString("n0") + " " + "damage";
		monsterDisplay.text = stageManager.stageMonsterCounter.ToString() + " / 12";
		
		healthBar.GetComponent<Image>().fillAmount = (float)(monster.health / monster.maxHealth);
		
		if (monster.type == MonsterRank.BOSS) {
			bossTimer -= Time.deltaTime;
			vetoDisplay.text += " " + bossTimer.ToString("F2"); 
			if (bossTimer <= 0) {
				NewMonsterType = stageManager.FailedBoss();
				monster = new Monster(stageManager.currentStage, NewMonsterType, this);
			}
		}
				
		if (monster.health <= 0) {
            GameObject tmp = CreateCoin(GetGoldFromMonster(monster.type, stageManager.currentStage));
            tmp.transform.SetParent(canvas);
			goldcoins.Add(tmp);
			NewMonsterType = stageManager.GetNewMonsterType(monster.type);
			monster = new Monster(stageManager.currentStage, NewMonsterType, this);
		}
	}

    public double GetGoldFromMonster (MonsterRank type, int stage) {
		if (type == MonsterRank.NORMAL)
            return Math.Round(1 * Mathf.Pow (1.1f, stage));
		else if (type == MonsterRank.MINIBOSS)
            return Math.Round(1 * Mathf.Pow (1.1f, stage) * 1.5f);
		else
            return Math.Round(1 * Mathf.Pow (1.1f, stage) * 2f);
	}

    public double GetDPS () {
        double	tick = 0;
		Hero	script;

		foreach (GameObject hero in heroes) {
			if (script = hero.GetComponent<Hero>())
                if (script.amount != 0) 
			        tick += script.dps;
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

    public  void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream savefile = File.Create(Application.persistentDataPath + "/save.dat");
        SaveClass save = new SaveClass();

        save.manager.Save(this);
        save.stageManager.Save(stageManager);
        save.monster.Save(monster);
        save.heroes.Save(heroes);
        save.tap.Save(this);
        save.header.Save();
        bf.Serialize(savefile, save);
        savefile.Close();
    }

    public  void Load()
    {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream savefile = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveClass save = (SaveClass)bf.Deserialize(savefile);
            savefile.Close();

            save.manager.Load(this);
            save.stageManager.Load(this);
            save.monster.Load(this);
            save.heroes.Load(this);
            save.tap.Load(this);
            save.header.Load(this);
        }
    }

    void    OnApplicationQuit()
    {
        Save();
    }

    void    OnApplicationPause(bool status)
    {
        Debug.Log(status);
        if (status == true)
            Save();
        else
            Load();
    }
}