using Godot;
using System;

public partial class Character : CharacterBody2D
{
	protected AnimatedSprite2D animatedSprite;
	public bool isGravity = true;
	public float speedWalk = 320f;
	//public float jumpStrength = 1000f;
	public bool isLookAtRight = true;
	
	public override void _Ready()
	{
		this.animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.animatedSprite.FlipH = !this.isLookAtRight;
		this.animatedSprite.Play("stand");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (this.isGravity && !IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		//if (Input.IsActionJustPressed("space") && IsOnFloor())
		//{
		//	velocity.Y = jumpStrength;
		//}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		direction.Y = 0f;
		direction = direction.Normalized();
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speedWalk;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speedWalk);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
