using Godot;
using System;

public partial class Player : Character
{
	private CanWalk canWalk;
	private CanFall canFall;
	private CanCurveJump canJump;

	private StatePlayer statePlayer = StatePlayer.Default;

	public override void _Ready()
	{
		base._Ready();
		this.intencityVelocityTaken = 0.85f;
		this.canWalk = new CanWalk(speedWalk: 4000f);
		this.canFall = new CanFall(this, mass: 12f);
		this.canJump = new CanCurveJump(this, jumpStrength: 400f, timeJump: 0.4f);
		this.canJump.canFall = this.canFall;
	}

	//public override void _Process(double delta)
	//{
	//}

	public override void _PhysicsProcess(double delta)
	{
		// get previous velocity.
		Vector2 velocity = this.getVelocity();

		StatePlayer newStatePlayer = StatePlayer.Default;

		// take input direction from user.
		Vector2 directionInput = Input.GetVector("left", "right", "up", "down");

		// eval if input horizontal is pressed.
		bool isHorizontalPress = Math.Abs(directionInput.X) > 0.2f;
		if (isHorizontalPress)
		{
			// apply walk.
			bool isWalkRight = directionInput.X > 0f;
			Vector2 directionWalk = isWalkRight ? Vector2.Right : Vector2.Left;
			velocity = this.canWalk.walk(velocity, (float)delta, directionWalk);

			// eval if it press to an oposite direction currently facing.
			bool isSwitchDirection = isWalkRight ^ this.isLookAtRight;
			if (isSwitchDirection)
			{
				this.isLookAtRight = !this.isLookAtRight;  // swap facing sprite.
			}

			// force to play anime walk.
			newStatePlayer = StatePlayer.Walk;

		}

		// apply sprite walk (or remove).
		if (isHorizontalPress ^ this.canWalk.isWalking)
		{
			this.canWalk.isWalking = !this.canWalk.isWalking;
		}

		// apply jump.
		bool isJumpInput = Input.IsActionJustPressed("space");
		if (isJumpInput)
		{
			velocity = this.canJump.jump(velocity);
		}
		velocity = this.canJump.updateJump(velocity, (float)delta);

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
				newStatePlayer = isFallingUp ? StatePlayer.Fall : StatePlayer.Jump;
			}
		}

		if (this.statePlayer != newStatePlayer)
		{
			this.animatedSprite.Play(
				newStatePlayer.ToString().ToLower()
			);
			this.statePlayer = newStatePlayer;
		}

		// apply new velocity.
		this.applyVelocity(velocity);
	}

	protected override void applyFlipH()
	{
		base.applyFlipH();
		GetNode<Marker2D>("CameraMarker").Position *= new Vector2(-1, 1);
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
