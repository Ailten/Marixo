using Godot;

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
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.FlipH = false;
		animatedSprite.Play("default");
	}

	// ------>

	protected virtual void applyFlipH()
	{
		animatedSprite.FlipH = !isLookAtRight;
	}

	protected void evalFlipH(float moveH) {
		if (moveH > 0f ^ isLookAtRight)
		{
			isLookAtRight = !isLookAtRight;
			applyFlipH();
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
	}

}
