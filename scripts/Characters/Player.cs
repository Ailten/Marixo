using Godot;
using System;

public partial class Player : Character
{
	private CanWalk canWalk;
	private CanFall canFall;
	private CanCurveJump canJump;
	private CanShoot canShoot;

	private StatePlayer statePlayer = StatePlayer.Default;

	public override void _Ready()
	{
		base._Ready();
		intencityVelocityTaken = 0.85f;
		canWalk = new CanWalk(speedWalk: 4000f);
		canFall = new CanFall(this, mass: 12f);
		canJump = new CanCurveJump(this, jumpStrength: 420f, timeJump: 0.45f);
		canJump.canFall = canFall;
		canShoot = new CanPoolShoot(
			this,
			amountToLoad: 2,
			startMarker: GetNode<Marker2D>("FistShootStartMarker"),
			endMarker: GetNode<Marker2D>("FistShootEndMarker")
		);
	}

	//public override void _Process(double delta)
	//{
	//}

	public override void _PhysicsProcess(double delta)
	{
		// get previous velocity.
		Vector2 velocity = getVelocity();

		StatePlayer newStatePlayer = StatePlayer.Default;

		// get horizontal input.
		float horizontalInput = Input.GetAxis("left", "right");

		// eval if input horizontal is pressed.
		bool isHorizontalPress = Math.Abs(horizontalInput) > 0.2f;
		if (isHorizontalPress)
		{
			// apply walk.
			bool isWalkRight = horizontalInput > 0f;
			Vector2 directionWalk = isWalkRight ? Vector2.Right : Vector2.Left;
			velocity = canWalk.walk(velocity, (float)delta, directionWalk);

			// eval if it press to an oposite direction currently facing.
			bool isSwitchDirection = isWalkRight ^ isLookAtRight;
			if (isSwitchDirection)
			{
				isLookAtRight = !isLookAtRight;  // swap facing sprite.
			}

			// force to play anime walk.
			newStatePlayer = StatePlayer.Walk;

		}

		// apply sprite walk (or remove).
		if (isHorizontalPress ^ canWalk.isWalking)
		{
			canWalk.isWalking = !canWalk.isWalking;
		}

		// apply jump.
		bool isJumpInput = Input.IsActionJustPressed("up");
		if (isJumpInput)
		{
			velocity = canJump.jump(velocity);
		}
		velocity = canJump.updateJump(velocity, (float)delta);

		// apply shoot.
		bool isShootInput = Input.IsActionJustPressed("shoot");
		if (isShootInput)
		{
			canShoot.shoot();
		}
		canShoot.updateShoot((float)delta);

		// apply falling.
		if (!IsOnFloor())
		{
			// apply fall.
			velocity = canFall.fall(velocity, (float)delta);

			// apply sprite jump or fall.
			bool isMovingVerticaly = Math.Abs(velocity.Y) > 0.01f;
			if (isMovingVerticaly)
			{
				bool isFallingUp = velocity.Y > 0f;
				newStatePlayer = isFallingUp ? StatePlayer.Fall : StatePlayer.Jump;
			}
		}

		if (statePlayer != newStatePlayer)
		{
			animatedSprite.Play(
				newStatePlayer.ToString().ToLower()
			);
			statePlayer = newStatePlayer;
		}

		// apply new velocity.
		applyVelocity(velocity);
	}

	protected override void applyFlipH()
	{
		base.applyFlipH();
		GetNode<Marker2D>("CameraMarker").Position *= new Vector2(-1, 1);
		GetNode<Marker2D>("FistShootStartMarker").Position *= new Vector2(-1, 1);
		GetNode<Marker2D>("FistShootEndMarker").Position *= new Vector2(-1, 1);
	}

}
