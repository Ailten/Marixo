using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D animatedSprite;
	private bool isLookAtRight = true;
	private bool isWalking = false;
	
	public float speedWalk = 320f;
	
	public override void _Ready()
	{
		this.animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.animatedSprite.FlipH = !this.isLookAtRight;
		this.animatedSprite.Play("stand");
	}
	
	public override void _Process(double delta)
	{
	}
	
	public override void _PhysicsProcess(double delta)
	{
		float horizontalInput = (
			(Input.IsActionPressed("right"))? 1f :
			(Input.IsActionPressed("left"))? -1f :
			0f
		);
		bool isHorizontalPress = Math.Abs(horizontalInput) > 0.5f;
		if (isHorizontalPress){
			
			bool isSwitchDirection = horizontalInput > 0f ^ this.isLookAtRight;
			if (isSwitchDirection){
				this.isLookAtRight = !this.isLookAtRight;
				this.animatedSprite.FlipH = !this.isLookAtRight;
			}
			
		}
		
		if (isHorizontalPress ^ this.isWalking){
			this.isWalking = !this.isWalking;
			this.animatedSprite.Play(
				(this.isWalking)? "walk": "stand"
			);
		}
		
		Vector2 horizontalMovement = Vector2.Right * (speedWalk * horizontalInput);
		Velocity = horizontalMovement;
		MoveAndSlide();
		
	}
}
