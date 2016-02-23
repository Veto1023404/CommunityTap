using UnityEngine;
using System.Collections;

public enum  MonsterRank { NORMAL, MINIBOSS, BOSS };

public class Monster
{

	const	double baseHealth = 12;
	public 	double health;
	public 	double maxHealth;
	public  MonsterRank type;
	public	GameManager gameManager;

    public Monster()
    {
    }

	public Monster(int stage, MonsterRank newtype, GameManager gm){
		maxHealth = baseHealth * Mathf.Pow (1.2f, stage);
		type = newtype;
		health = maxHealth;
		gameManager = gm;
		if (type == MonsterRank.MINIBOSS)
			maxHealth = health *= 1.2;
		else if (type == MonsterRank.BOSS)
			maxHealth = health *= 1.4;
	}
		
	~Monster() {
		if (type == MonsterRank.BOSS)
			gameManager.bossTimer = 30;
	}
}