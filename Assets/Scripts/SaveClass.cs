using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public enum Action {Load, Save};

[Serializable]
public class SaveClass
{
    public StageSave        stageManager;
    public MonsterSave      monster;
    public GameManagerSave  manager;
    public HeroesSave       heroes;
    public TapSave          tap;
    public Header           header;

    public SaveClass()
    {
        stageManager = new StageSave();
        monster = new MonsterSave();
        manager = new GameManagerSave();
        heroes = new HeroesSave();
        tap = new TapSave();
        header = new Header();
    }
}

[Serializable]
public class GameManagerSave
{
    public double       gold;
    public float        bossTimer;

    public void Save(GameManager gameManager)
    {
        gold = gameManager.gold;
        bossTimer = gameManager.bossTimer;
    }

    public void Load(GameManager gameManager)
    {
        gameManager.gold = gold;
        gameManager.bossTimer = bossTimer;
    }
}


[Serializable]
public class StageSave
{
    public int          currentStage;
    public int          maxStage;
    public int          stageMonsterCounter;

    public void Save(StageManager sm)
    {
        currentStage = sm.currentStage;
        maxStage = sm.maxStage;
        stageMonsterCounter = sm.stageMonsterCounter;
    }

    public void Load(GameManager gameManager)
    {
        StageManager sm = new StageManager(gameManager);
        sm.currentStage = currentStage;
        sm.maxStage = maxStage;
        sm.stageMonsterCounter = stageMonsterCounter;
        gameManager.stageManager = sm;
    }
}


[Serializable]
public class MonsterSave
{
    public  double health;
    public  double maxHealth;
    public  MonsterRank type;

    public void Save(Monster m)
    {
        health = m.health;
        maxHealth = m.maxHealth;
        type = m.type;
    }

    public void Load(GameManager gameManager)
    {
        Monster m = new Monster();
        m.health = health;
        m.maxHealth = maxHealth;
        m.type = type;
        gameManager.monster = m;
    }
}

[Serializable]
public class HeroesSave
{
    public List<HeroSave> heroes;

    public HeroesSave()
    {
        heroes = new List<HeroSave>();
    }

    public void Save(List<GameObject> heroesList)
    {
        foreach (var component in heroesList)
        {
            HeroSave tmp = new HeroSave();
            Hero htmp = component.GetComponent<Hero>();
            tmp.cost = htmp.cost;
            tmp.amount = htmp.amount;
            tmp.dps = htmp.dps;
            tmp.name = htmp.name;
            tmp.baseCost = htmp.baseCost;
            tmp.baseDPS = htmp.baseDPS;
            heroes.Add(tmp);
        }
    }

    public void Load(GameManager gameManager)
    {
        List<GameObject> tmp = new List<GameObject>();

        foreach (var component in heroes)
        {
            GameObject btmp = gameManager.CreateButton();
            Hero htmp = btmp.GetComponent<Hero>();
            htmp.cost = component.cost;
            htmp.amount = component.amount;
            htmp.dps = component.dps;
            htmp.name = component.name;
            htmp.baseCost = component.baseCost;
            htmp.baseDPS = component.baseDPS;
            tmp.Add(btmp);
        }
        gameManager.heroes = tmp;
    }

}

[Serializable]
public class HeroSave
{
    public double   cost;
    public int      amount;
    public double   dps;
    public string   name;
    public double   baseCost;
    public double   baseDPS;
}

[Serializable]
public  class TapSave
{
    public double   tapDamage;
    public float    criticalChance;
    public double   cost;
    public int      amount;
    public string   name;
    public double   baseCost;
    public double   baseDamage;

    public void Save(GameManager gameManager)
    {
        TapUpgrade tUp = gameManager.tapUpgrade.GetComponent<TapUpgrade>();
        tapDamage = gameManager.tapDamage;
        criticalChance = gameManager.tapButton.GetComponent<Click>().criticalChance;
        cost = tUp.cost;
        amount = tUp.amount;
        name = tUp.name;
        baseCost = tUp.baseCost;
        baseDamage = tUp.baseDamage;
    }

    public void Load(GameManager gameManager)
    {
        TapUpgrade tUp = gameManager.tapUpgrade.GetComponent<TapUpgrade>();
        gameManager.tapDamage = tapDamage;
        gameManager.tapButton.GetComponent<Click>().criticalChance = criticalChance;
        tUp.cost = cost;
        tUp.amount = amount;
        tUp.name = name;
        tUp.baseCost = baseCost;
        tUp.baseDamage = baseDamage;
    }
}

[Serializable]
public class Header
{
    public DateTime saveTime;
    public String   version;

    public void Save()
    {
        saveTime = DateTime.Now;
        version = Application.version;
    }

    public void Load(GameManager gameManager)
    {
        double diff = (DateTime.Now - saveTime).TotalSeconds;
        double maxHp = new Monster(gameManager.stageManager.currentStage, MonsterRank.NORMAL, gameManager).maxHealth;
        double totalDPS = gameManager.GetDPS() * diff;
        double gold = gameManager.GetGoldFromMonster(MonsterRank.NORMAL, gameManager.stageManager.currentStage);
        int    monsterKilled = 0;

        while ((totalDPS - maxHp) > 0)
        {
            totalDPS -= maxHp;
            monsterKilled++;
        }
        Debug.Log("Gold: " + gold);
        Debug.Log("Game Gold: " + gameManager.gold);
        gold *= monsterKilled;
        gameManager.gold += gold;
        Debug.Log(saveTime);
        Debug.Log(version);
        Debug.Log("Difference: " + diff);
        Debug.Log("Hp: " + maxHp);
        Debug.Log("DPS: " + totalDPS);
        Debug.Log("MonsterKilled: " + monsterKilled);
        Debug.Log("Gold: " + gold);
        Debug.Log("Game Gold: " + gameManager.gold);
    }
}