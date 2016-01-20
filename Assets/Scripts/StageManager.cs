using UnityEngine;
using System.Collections;

public class StageManager {
	public int currentStage;
	public int maxStage;
	public int monstersKilled;

	public StageManager() {
		currentStage = 0;
	}

	public MonsterType 	GetNewMonsterType(MonsterType killedMonster) {
		monstersKilled++;
		if (killedMonster == MonsterType.BOSS) {
			currentStage++;
			monstersKilled = 0;
			Debug.Log ("Current stage : " + currentStage);
		}
		if (monstersKilled <= 10) 
			return MonsterType.NORMAL;
		else if (monstersKilled % 11 == 0)
			return MonsterType.MINIBOSS;
		else if (monstersKilled % 12 == 0)
			return MonsterType.BOSS;
		else
			return MonsterType.NORMAL;
	}

	~StageManager() {
		Debug.Log ("StageManager Destroyed");
	}
}
