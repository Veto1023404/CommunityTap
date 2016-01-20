using UnityEngine;
using System.Collections;

public class StageManager {
	public int currentStage;
	public int maxStage;
	public int stageMonsterCounter;

	public StageManager() {
		currentStage = 0;
	}

	public MonsterType 	GetNewMonsterType(MonsterType killedMonster) {
		stageMonsterCounter++;
		if (killedMonster == MonsterType.BOSS) {
			currentStage++;
			stageMonsterCounter = 1;
			Debug.Log ("Current stage : " + currentStage);
		}
		if (stageMonsterCounter <= 10) 
			return MonsterType.NORMAL;
		else if (stageMonsterCounter % 11 == 0)
			return MonsterType.MINIBOSS;
		else if (stageMonsterCounter % 12 == 0)
			return MonsterType.BOSS;
		else
			return MonsterType.NORMAL;
	}

	public MonsterType 	FailedBoss()
	{
		stageMonsterCounter = 10;
		Debug.Log("Failed Boss");
		return MonsterType.NORMAL;
	}

	~StageManager() {
		Debug.Log ("StageManager Destroyed");
	}
}
