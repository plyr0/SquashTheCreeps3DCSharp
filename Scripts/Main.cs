using Godot;

public class Main : Node
{
	[Export] public PackedScene mobScene = ResourceLoader.Load<PackedScene>("res://Scenes/Mob.tscn");

	public override void _Ready() => GD.Randomize();

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UI/Retry").Visible)
		{
			GetTree().ReloadCurrentScene();
		}
	}

	public void OnPlayerHit()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<ColorRect>("UI/Retry").Show();
	}

	public void OnMobTimerTimeout()
	{
		var mob = mobScene.Instance<Mob>();
		var mobSpawnLoction = GetNode<PathFollow>("SpawnPath/SpawnLocation");
		mobSpawnLoction.UnitOffset = GD.Randf();

		var playerPosition = GetNode<Player>("Player").Transform.origin;
		AddChild(mob);
		mob.Initialize(mobSpawnLoction.Translation, playerPosition);
		mob.Connect(
			signal: nameof(Mob.Squashed),
			target: GetNode<ScoreLabel>("UI/ScoreLabel"),
			method: ScoreLabel.OnSquashMonsterName);
	}
}
