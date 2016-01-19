using UnityEngine;
using System.Collections;

public enum  MonsterType { NORMAL, MINIBOSS, BOSS };

public class Monster
{

		const	double baseHealth = 12;
		public 	double health;
		public 	double maxHealth;
		public  GameManager gamemanager;

		public Monster(int stage, MonsterType type){
			maxHealth = health = baseHealth * Mathf.Pow (1.2f, stage);
			gamemanager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		}
		
		~Monster() {
		Debug.Log("Destroyed");
		}
}

