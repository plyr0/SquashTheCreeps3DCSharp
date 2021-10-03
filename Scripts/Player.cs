using Godot;

public class Player : KinematicBody
{
	[Export] public float speed = 14f;
	[Export] public float jumpImpulse = 20f;
	[Export] public float fallAcceleration = 75f;
	[Export] public float bounceImpulse = 16f;

	[Signal] public delegate void Hit();

	public Vector3 velocity = Vector3.Zero;


	public void OnAreaBodyEntered(Node _) => Die();

	public void Die()
	{
		EmitSignal(nameof(Hit));
		QueueFree();
	}

	public override void _PhysicsProcess(float delta)
	{
		var direction = Vector3.Zero;
		if (Input.IsActionPressed("move_left")) direction.x -= 1;
		if (Input.IsActionPressed("move_right")) direction.x += 1;
		
		if (Input.IsActionPressed("move_forward")) direction.z -= 1;
		if (Input.IsActionPressed("move_back")) direction.z += 1;

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			GetNode<Spatial>("Pivot").LookAt(Translation + direction, Vector3.Up);
			GetNode<AnimationPlayer>("AnimationPlayer").PlaybackSpeed = 4f;
		}
		else
		{
			GetNode<AnimationPlayer>("AnimationPlayer").PlaybackSpeed = 1f;
		}

		if (IsOnFloor() && Input.IsActionPressed("jump")) velocity.y += jumpImpulse;

		velocity.x = direction.x * speed;
		velocity.z = direction.z * speed;
		velocity.y -= fallAcceleration * delta;

		velocity = MoveAndSlide(velocity, Vector3.Up);

		for (int i = 0; i < GetSlideCount(); i++)
		{
			KinematicCollision collision = GetSlideCollision(i);
			if (collision.Collider is Mob mob)
			{
				if (Vector3.Up.Dot(collision.Normal) > 0.1)
				{
					mob.Squash();
					velocity.y = bounceImpulse;
				}
			}
		}

		var jumpRotation = Mathf.Deg2Rad(30f) * velocity.y / jumpImpulse;
		var oldRotation = GetNode<Spatial>("Pivot").Rotation;
		var newRotation = new Vector3
		{
			x = jumpRotation,
			y = oldRotation.y,
			z = oldRotation.z
		};
		GetNode<Spatial>("Pivot").Rotation = newRotation;
	}
}
