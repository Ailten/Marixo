using Godot;
using System;

public partial class Player : Character
{
	private CanWalk canWalk;
	private CanFall canFall;
	public CanJump canJump;
	private CanShoot canShoot;
	private CanRespawn canRespawn;

	private StatePlayer statePlayer = StatePlayer.Default;

	public override void _Ready()
	{
		base._Ready();
		intencityVelocityTaken = 0.85f;
		setMaxHp = 3;

		canWalk = new CanWalk(speedWalk: 4000f);
		canFall = new CanFall(this, mass: 12f);
		canJump = new CanCurveJump(this, canFall, jumpStrength: 420f, timeJump: 0.45f);
		canShoot = new CanPoolShoot(
			this,
			amountToLoad: 2,
			startMarker: GetNode<Marker2D>("FistShootStartMarker"),
			endMarker: GetNode<Marker2D>("FistShootEndMarker")
		);
		canBeHit.cooldownDamaged = 0.3f;
		canBeHit.intencityRepeal = 70f;
		canBeHit = CanBeHitInvuFrame.evolvFrom(canBeHit);
		canRespawn = new CanRespawn(this);
	}

	//public override void _Process(double delta)
	//{
	//}

	public override void _PhysicsProcess(double delta)
	{
		// get previous velocity.
		Vector2 velocity = getVelocity();

		StatePlayer newStatePlayer = StatePlayer.Default;

		// check end hit anime.
		if (statePlayer == StatePlayer.Be_Hit && !canBeHit.isCooldownDamaged)
		{
			if (!isLiving)
			{
				death(canBeHit.characterWhoAtk);
				return;
			}

			if (canBeHit is CanBeHitInvuFrame canBeHitInvu) {

				// set invu skin (opacity).
				animatedSprite.Modulate = animatedSprite.Modulate * new Color(1, 1, 1, 0.4f);  // 40% opacity.

				// set data invu.
				canBeHitInvu.isWasSetToInvu = true;
			}
		}

		// check end invu frame.
		if (canBeHit is CanBeHitInvuFrame canBeHitInvuFrame)
		{
			// end invu frame.
			if (!canBeHitInvuFrame.isInvu && canBeHitInvuFrame.isWasSetToInvu)
			{
				// set vulné skin (opacity).
				animatedSprite.Modulate = animatedSprite.Modulate + new Color(0, 0, 0, 1);

				// set data.
				canBeHitInvuFrame.isWasSetToInvu = false;
			}
		}

		// get horizontal input.
		float horizontalInput = Input.GetAxis("left", "right");

		// eval if input horizontal is pressed.
		bool isHorizontalPress = Math.Abs(horizontalInput) > 0.2f;
		if (isHorizontalPress && !canBeHit.isCooldownDamaged)
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

		// anime hit.
		bool isHited = canBeHit.isCooldownDamaged;
		if (isHited)
		{
			newStatePlayer = StatePlayer.Be_Hit;  // set state (for sprite).
			velocity = canBeHit.getRepealVelocityUpdate(velocity);  // set repeal velocity.
		}

		// apply jump.
		bool isJumpInput = Input.IsActionJustPressed("jump");
		if (isJumpInput && !isHited)
		{
			velocity = canJump.jump(velocity);
		}
		velocity = canJump.updateJump(velocity, (float)delta);

		// apply shoot.
		bool isShootInput = Input.IsActionJustPressed("shoot");
		if (isShootInput && !isHited)
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
			if (isMovingVerticaly && !isHited)
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

	// ------>

	public override bool takeDamage(int damage, Character damageMaker = null, bool isCheckDeath = true)
	{
		// if take damage when invu frame.
		if (canBeHit is CanBeHitInvuFrame canBeHitInvuFrame)
		{
			if (canBeHitInvuFrame.isInvu)
				return false;
		}

		return base.takeDamage(damage, damageMaker, isCheckDeath: false);
	}

	public override void death(Character killer = null)
	{
		canRespawn.teleportRespawn();
		refillLive();
	}

}
