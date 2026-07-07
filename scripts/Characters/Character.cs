using Godot;
using System;

public partial class Character : CharacterBody2D
{
	protected AnimatedSprite2D animatedSprite;
	private bool _isLookAtRight = true;
	protected bool isLookAtRight
	{
		get => _isLookAtRight;
		set
		{
			if (_isLookAtRight != value)
			{
				_isLookAtRight = value;
				applyFlipH();
			}
		}
	}

	protected float intencityVelocityTaken = 0.95f;

	public override void _Ready()
	{
		// get and set default value to AnimatedSprite2D child.
		this.animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.applyFlipH();
		this.animatedSprite.Play("default");
	}

	// ------>

	private void applyFlipH() {
		this.animatedSprite.FlipH = !this.isLookAtRight;
	}

	protected void evalFlipH(float moveH) {
		if (moveH > 0f ^ this.isLookAtRight)
		{
			this.isLookAtRight = !this.isLookAtRight;
			this.applyFlipH();
		}
	}

	protected Vector2 getVelocity()
	{
		return Velocity * intencityVelocityTaken;
	}

	protected void applyVelocity(Vector2 velocity)
	{
		Velocity = velocity;
		MoveAndSlide();
		//MoveAndCollide(Velocity);
	}

}
