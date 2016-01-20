using UnityEngine;
using System.Collections;

public enum  MonsterType { NORMAL, MINIBOSS, BOSS };

public class Monster
{

		const	double baseHealth = 12;
		public 	double health;
		public 	double maxHealth;
		public  MonsterType type;

		public Monster(int stage, MonsterType newtype){
			maxHealth = baseHealth * Mathf.Pow (1.2f, stage);
			type = newtype;
			health = maxHealth; 
			if (type == MonsterType.MINIBOSS)
				maxHealth = health *= 1.2;
			else if (type == MonsterType.BOSS)
				maxHealth = health *= 1.4;
			Debug.Log ("Monster Type : " + type);
		}
		
		~Monster() {
			Debug.Log("Monster Destroyed");
		}
}

