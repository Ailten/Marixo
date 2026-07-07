using Godot;
using System;

public partial class Player : Character
{
	private CanWalk canWalk;
	private CanFall canFall;
	private CanJump canJump;

	private StatePlayer statePlayer = StatePlayer.Default;

	public override void _Ready()
	{
		base._Ready();
		this.intencityVelocityTaken = 0.9f;
		this.canWalk = new CanWalk(speedWalk: 2600f);
		this.canFall = new CanFall(this, mass: 5f);
		this.canFall.fallingMultiplication = 1.5f;
		this.canJump = new CanJump(this, jumpStrength: 3000f);
	}

	//public override void _Process(double delta)
	//{
	//}

	public override void _PhysicsProcess(double delta)
	{
		// get previous velocity.
		Vector2 velocity = this.getVelocity();

		// take input direction from user.
		Vector2 directionInput = Input.GetVector("left", "right", "up", "down");
		bool isJumpInput = Input.IsActionPressed("space");

		// eval if input horizontal is pressed.
		bool isHorizontalPress = Math.Abs(directionInput.X) > 0.2f;
		if (isHorizontalPress)
		{

			// eval if it press to an oposite direction currently facing.
			bool isSwitchDirection = directionInput.X > 0f ^ this.isLookAtRight;
			if (isSwitchDirection)
			{
				this.isLookAtRight = !this.isLookAtRight;  // swap facing sprite.
			}

			// force to play anime walk.
			if (IsOnFloor() && this.animatedSprite.Animation != "walk")
			{
				this.animatedSprite.Play("walk");
			}

		}

		// apply sprite walk (or remove).
		if (isHorizontalPress ^ this.canWalk.isWalking)
		{
			this.canWalk.isWalking = !this.canWalk.isWalking;
			if (IsOnFloor())
			{
				this.animatedSprite.Play(
					this.canWalk.isWalking ? "walk" : "default"
				);
			}
		}

		// apply walk.
		if (isHorizontalPress)
		{
			Vector2 directionWalk = directionInput.X > 0f ? Vector2.Right : Vector2.Left;
			velocity = this.canWalk.walk(velocity, (float)delta, directionWalk);
		}

		// apply jump.
		if (isJumpInput)
		{
			velocity = this.canJump.jump(velocity);
		}

		// apply falling.
		if (!IsOnFloor())
		{
			// apply fall.
			velocity = this.canFall.fall(velocity, (float)delta);

			// apply sprite jump or fall.
			bool isMovingVerticaly = Math.Abs(velocity.Y) > 0.01f;
			if (isMovingVerticaly)
			{
				bool isFallingUp = velocity.Y > 0f;
				this.animatedSprite.Play(
					isFallingUp ? "jump_down" : "jump_up"
				);
			}
		}

		// apply default sprite (if no falling).
		//if (IsOnFloor() && Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.01f)
		//{
		//	this.animatedSprite.Play("default");
		//}

		// apply new velocity.
		this.applyVelocity(velocity);
	}

}

public enum StatePlayer  // implement it as switch case (for play anime).
{
	Default,
	Walk,
	Jump,
	Fall,
	Atk,
}
