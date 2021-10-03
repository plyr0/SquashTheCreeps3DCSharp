using Godot;

public class ScoreLabel : Label
{
	public static string OnSquashMonsterName = nameof(OnSquashMonster);
	
	private int score = 0;

	public void OnSquashMonster()
	{
		++score;
		Text = $"Score: {score}";
	}
}
