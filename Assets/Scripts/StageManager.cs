using UnityEngine;
using System.Collections;

public class StageManager {
	public int currentStage;
	public int maxStage;
	public int stageMonsterCounter;
	public GameManager gameManager;

	public StageManager(GameManager gm) {
		currentStage = 0;
		gameManager = gm;
	}

	public MonsterRank 	GetNewMonsterType(MonsterRank killedMonster) {
		stageMonsterCounter++;
		if (killedMonster == MonsterRank.BOSS) {
			currentStage++;
			stageMonsterCounter = 1;
			Debug.Log ("Current stage : " + currentStage);
		}
		if (stageMonsterCounter <= 10) 
			return MonsterRank.NORMAL;
		else if (stageMonsterCounter % 11 == 0)
			return MonsterRank.MINIBOSS;
		else if (stageMonsterCounter % 12 == 0)
			return MonsterRank.BOSS;
		else
			return MonsterRank.NORMAL;
	}

	public MonsterRank 	FailedBoss()
	{
		stageMonsterCounter = 10;
		Debug.Log("Failed Boss");
		return MonsterRank.NORMAL;
	}

	~StageManager() {
		Debug.Log ("StageManager Destroyed");
	}
}
