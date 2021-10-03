using Godot;

public class Mob : KinematicBody
{
	[Export] public float minSpeed = 10f;
	[Export] public float maxSpeed = 18f;

	[Signal] public delegate void Squashed();

	public Vector3 velocity = Vector3.Zero;


	public void OnVisibilityNotifierScreenExited() => QueueFree();

	public override void _PhysicsProcess(float _) => MoveAndSlide(velocity);

	public void Initialize(Vector3 startPosition, Vector3 playerPosition)
	{
		Translation = startPosition;
		LookAt(playerPosition, Vector3.Up);
		RotateY((float)GD.RandRange(Mathf.Deg2Rad(-45f), Mathf.Deg2Rad(45f)));
		
		var randomSpeed = (float)GD.RandRange(minSpeed, maxSpeed);
		velocity = Vector3.Forward * randomSpeed;
		velocity = velocity.Rotated(Vector3.Up, Rotation.y);
	}

	public void Squash()
	{
		EmitSignal(nameof(Squashed));
		QueueFree();
	}
}
