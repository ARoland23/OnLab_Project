using UnityEngine;

public static class GameLogic 
{
	public enum Difficulty { Easy,Medium,Hard }

	public static Difficulty difficulty = Difficulty.Easy;

	private static int score = 0;
	public static int enemyCount = 0;
	public static int Score{ get { return score; } }


	public static void ResetEnemies()
	{
		if(difficulty == Difficulty.Easy)
			enemyCount = 2;
		else if(difficulty == Difficulty.Medium)
			enemyCount = 4;
		else if(difficulty == Difficulty.Hard)
			enemyCount = 5;
	}

	public static void AddScore(int points)
	{
        switch (difficulty)
		{
            case Difficulty.Easy:
				score += points;
				break;
			case Difficulty.Medium:
                float newScore;
                newScore = points * 1.5f;
                score += Mathf.RoundToInt(newScore);
				break;
			case Difficulty.Hard:
                score += points * 2;
				break;
        }
	}
    public static void SetScore(int points)
    {
        score = points;
    }
    public static bool AllEnemiesDead()
	{
		if (enemyCount > 0)
			return false;
		else
			return true;
	}

}
