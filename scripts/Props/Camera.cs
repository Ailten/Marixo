using Godot;
using System;

public partial class Camera : Camera2D
{
	public Node2D target;
	public float lerpCamera = 0.08f;

	public override void _Ready()
	{
		this.target = GetParent().GetNode<Node2D>("Player").GetNode<Node2D>("CameraMarker");
		this.focusTarget();
	}

	public override void _Process(double delta)
	{
		if (target != null)
		{
			GlobalPosition = GlobalPosition.Lerp(
				target.GlobalPosition,
				lerpCamera
			);
		}
	}

	public void focusTarget()
	{
		if (target != null)
		{
			GlobalPosition = target.GlobalPosition;
		}
	}
}
